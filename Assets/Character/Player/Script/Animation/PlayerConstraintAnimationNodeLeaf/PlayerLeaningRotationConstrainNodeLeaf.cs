using System;
using UnityEngine;

public class PlayerLeaningRotationConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    private LeaningRotaionScriptableObject leaningScriptableObject;
    private LeaningRotation leaningRotation;
    private IWeaponAdvanceUser weaponAdvanceUser;
    private Player player;
    public PlayerLeaningRotationConstrainNodeLeaf(Player player,LeaningRotaionScriptableObject leaningScriptableObject,LeaningRotation leaningRotation,IWeaponAdvanceUser weaponAdvanceUser,Func<bool> precondition) : base(precondition)
    {
        this.leaningScriptableObject = leaningScriptableObject;
        this.leaningRotation = leaningRotation;
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void UpdateNode()
    {
        leaningRotation.SetWeight(weaponAdvanceUser._weaponManuverManager.aimingWeight, leaningScriptableObject);
        if (player.curShoulderSide == Player.ShoulderSide.Left)
        {
            Vector3 castDir = weaponAdvanceUser._pointingPos - weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position;
            Vector3 castPos = weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position + Vector3.Cross(castDir.normalized, Vector3.down) * (leaningScriptableObject.recoveryStepCheck-leaningScriptableObject.weightAdd)*(1-leaningRotation.weight);

            if (PointingBlock(castPos, weaponAdvanceUser._pointingPos))
                leaningRotation.SetLeaningLeftRight(leaningRotation.GetLeaningLeftRight() - leaningScriptableObject.weightAdd * Time.deltaTime);
            
            else
            {
                if (RecoveryCheck(weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position, weaponAdvanceUser._pointingPos))
                    leaningRotation.SetLeaningLeftRight(Mathf.MoveTowards(leaningRotation.GetLeaningLeftRight(), 0, leaningScriptableObject.weightAdd * Time.deltaTime));   
            }

        }
        else if(player.curShoulderSide == Player.ShoulderSide.Right)
        {
            Vector3 castDir = weaponAdvanceUser._pointingPos - weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position;
            Vector3 castPos = weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position + Vector3.Cross(castDir.normalized, Vector3.up) * (leaningScriptableObject.recoveryStepCheck - leaningScriptableObject.weightAdd) * (1 - leaningRotation.weight);

            if (PointingBlock(weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position, weaponAdvanceUser._pointingPos))
                leaningRotation.SetLeaningLeftRight(leaningRotation.GetLeaningLeftRight() + leaningScriptableObject.weightAdd * Time.deltaTime);

            else
            {
                if (RecoveryCheck(weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position, weaponAdvanceUser._pointingPos))
                    leaningRotation.SetLeaningLeftRight(Mathf.MoveTowards(leaningRotation.GetLeaningLeftRight(), 0, leaningScriptableObject.weightAdd * Time.deltaTime));
            }
        }
            
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
       
        base.FixedUpdateNode();
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool PointingBlock(Vector3 startCastPos,Vector3 targetPos)
    {
        Vector3 castDir = targetPos - startCastPos;
        Debug.DrawLine(startCastPos, targetPos, Color.green);

        if(Physics.Raycast(startCastPos,castDir.normalized, out RaycastHit hit, leaningScriptableObject.distanceCheck, leaningScriptableObject.castingCheckLayer.value))
        {
            if (Vector3.Distance(hit.point, targetPos) > 0.67f)
                return true;
            else
                return false;
        }
        //if(Physics.Raycast(startCastPos,castDir.normalized,out RaycastHit hit, leaningScriptableObject.distanceCheck, leaningScriptableObject.castingCheckLayer.value))
        //{

           
        //}

        return false;
    }
    private bool RecoveryCheck(Vector3 startCastPos, Vector3 targetPos)
    {
        Vector3 castDir = targetPos - startCastPos;

        Vector3 recoveryDir =Vector3.zero;


        if (leaningRotation.GetLeaningLeftRight() > 0)//LeanRight
        {
            recoveryDir = Vector3.Cross(castDir.normalized, Vector3.up);
        }
        else if (leaningRotation.GetLeaningLeftRight() < 0)//LeanLeft
        {
            recoveryDir = Vector3.Cross(castDir.normalized, Vector3.down);
        }

        Vector3 recoveryCheckStart = startCastPos + (recoveryDir * leaningScriptableObject.recoveryStepCheck*leaningRotation.weight);


        
        if (Physics.Raycast(recoveryCheckStart,(targetPos-recoveryCheckStart).normalized , out RaycastHit hit, leaningScriptableObject.distanceCheck, leaningScriptableObject.castingCheckLayer.value))
        {
            if (Vector3.Distance(hit.point, targetPos) < 0.1f)
                return true;
            else
                return false;
        }
        return true;


    }
   
}
