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
    }

    public override void Enter()
    {
        base.Enter();
    }
    public override void UpdateNode()
    {
        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        leaningRotation.SetWeight(Mathf.MoveTowards(leaningRotation.weight,1,Time.deltaTime),leaningScriptableObject);

        if (TargetHit(weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position, weaponAdvanceUser.pointingPos) == false)
        {
            if (player.curShoulderSide == Player.ShoulderSide.Left)
                leaningRotation.SetLeaningLeftRight(leaningRotation.GetLeaningLeftRight() - leaningScriptableObject.weightAdd * Time.deltaTime);
            else if (player.curShoulderSide == Player.ShoulderSide.Right)
                leaningRotation.SetLeaningLeftRight(leaningRotation.GetLeaningLeftRight() + leaningScriptableObject.weightAdd * Time.deltaTime);
        }
        else
        {
            if(RecoveryCheck(weaponAdvanceUser._currentWeapon.bulletSpawnerPos.position, weaponAdvanceUser.pointingPos))
            {
                leaningRotation.SetLeaningLeftRight(Mathf.MoveTowards(leaningRotation.GetLeaningLeftRight(),0,leaningScriptableObject.weightAdd*Time.deltaTime));
            }
        }
        base.FixedUpdateNode();
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool TargetHit(Vector3 startCastPos,Vector3 targetPos)
    {
        Vector3 castDir = targetPos - startCastPos;
        if(Physics.Raycast(startCastPos,castDir.normalized,out RaycastHit hit, leaningScriptableObject.distanceCheck, leaningScriptableObject.castingCheckLayer.value))
        {
            if(Vector3.Distance(hit.point,targetPos) < 0.1f)
                return true;
            else
                return false;
        }

        return true;
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

        Vector3 recoveryCheckStart = startCastPos + (recoveryDir * leaningScriptableObject.recoveryStepCheck);


        
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
