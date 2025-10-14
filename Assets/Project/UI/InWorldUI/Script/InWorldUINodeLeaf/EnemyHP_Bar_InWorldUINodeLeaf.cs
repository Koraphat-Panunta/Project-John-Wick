using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP_Bar_InWorldUINodeLeaf : InWorldUINodeLeaf
{
    private LayerMask enemyMask;
    private Camera camera;
    private IWeaponAdvanceUser weaponAdvanceUser;
    private Vector3 pointingPos => weaponAdvanceUser._pointingPos;
    private float range => Mathf.Lerp(3, 8, weaponAdvanceUser._weaponManuverManager.aimingWeight);

    private EnemyHPInWorldUI enemyHPInWorldUI;
    public EnemyHP_Bar_InWorldUINodeLeaf(Func<bool> preCondition
        ,Camera camera
        ,IWeaponAdvanceUser weaponAdvanceUser
        ,EnemyHPInWorldUI enemyHPInWorldUIPrefab) : base(preCondition)
    {

       
        this.camera = camera;
        this.enemyMask = LayerMask.GetMask("Enemy") | LayerMask.GetMask("Default");
        this.weaponAdvanceUser = weaponAdvanceUser;

        this.enemyHPInWorldUI = GameManager.Instantiate(enemyHPInWorldUIPrefab);
        this.enemyHPInWorldUI.gameObject.SetActive(false);
    }
    public override void FixedUpdateNode()
    {
        Enemy detectedEnemy = null;

        if(Physics.Raycast(this.camera.transform.position,(this.pointingPos - this.camera.transform.position).normalized,out RaycastHit hitInfo, range, this.enemyMask))
        {
            if(hitInfo.collider.TryGetComponent<BodyPart>(out BodyPart bodyPart)
                && bodyPart.enemy.isDead == false 
                && bodyPart.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<GotRestrictNodeLeaf>() == false
                && bodyPart.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<HumandShield_GotInteract_NodeLeaf>() == false)
                detectedEnemy = bodyPart.enemy;
        }
        else if(Physics.SphereCast(this.camera.transform.position,1.25f, (this.pointingPos - this.camera.transform.position).normalized, out RaycastHit hitInfoSphere, range, this.enemyMask))
        {
            if (hitInfoSphere.collider.TryGetComponent<BodyPart>(out BodyPart bodyPart)
                && bodyPart.enemy.isDead == false
                && bodyPart.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<GotRestrictNodeLeaf>() == false
                && bodyPart.enemy.enemyStateManagerNode.TryGetCurNodeLeaf<HumandShield_GotInteract_NodeLeaf>() == false)
                detectedEnemy = bodyPart.enemy;
        }

        if (detectedEnemy != null)
        {
            enemyHPInWorldUI.gameObject.SetActive(true);
            enemyHPInWorldUI.SetValue(detectedEnemy.GetHP(),detectedEnemy.GetMaxHp());
            enemyHPInWorldUI.SetAnchorPosition(detectedEnemy.head.transform.position);
            enemyHPInWorldUI.SetCameraLookAt(this.camera);
            
        }
        else
            enemyHPInWorldUI.gameObject.SetActive(false);

        base.FixedUpdateNode();
    }
    
}
