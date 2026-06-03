using UnityEngine;

public partial class PlayerConstrainAnimationManager : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
        this.Body_Look_ConstrainCondition(player, node);
        this.Body_Lean_ConstrainCondition(player, node);

        this.RightHand_ConstrainCondition(player,node);

        this.LegsConstrainCondition(player,node);

        if (node is OCM_Execute_Single_NodeLeaf gunFuExecuteNodeLeaf)
        {
            if(gunFuExecuteNodeLeaf._curPhase == NodePhase.Exit)
                isEnableIK = false;
        }

        if(node is SubjectPlayer.NotifyEvent playerEvent
            && playerEvent == SubjectPlayer.NotifyEvent.Firing)
        {
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.TriggeRecoilWeight();
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.TriggeRecoilWeight();
            this.bodyLookConstraintNodeLeaf.TriggerRecoil();
        }

        if (this.player._currentWeapon is AutomaticShotgunModel)
            this.bodyLookConstraintNodeLeaf.SetRecoilScriptableObject(this.shotGun_BodyRecoil_SCRP);
        else
            this.bodyLookConstraintNodeLeaf.SetRecoilScriptableObject(null);
    }

    private void Body_Look_ConstrainCondition<T>(Player player,T obj)
    {
        if(this.bodyLookConstraintNodeLeaf.Precondition() == false)
            return;

       

        switch (obj)
        {
            case PlayerProneStateNodeLeaf proneStateNodeLeaf:
                {
                    if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject
                        == this.body_ADS_Prone_Constrain_SCRP)
                        return;
                    this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(this.body_ADS_Prone_Constrain_SCRP);
                    this.bodyLookConstraintNodeLeaf.SetWeight(0);
                }
                break;
            case IQuickSwitchNode quickSwitchNode:
                {
                    if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject
                        == this.quickSwitchAimSplineLookConstrainScriptableObject)
                        return;
                    this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(this.quickSwitchAimSplineLookConstrainScriptableObject);
                    this.bodyLookConstraintNodeLeaf.SetWeight(0);
                }
                break;
            case AimDownSightWeaponManuverNodeLeaf:
                {
                    if(this.player._currentWeapon == null)
                        return;

                    if(this.player._currentWeapon != null
                        && this.player._currentWeapon is PrimaryWeapon
                        && this.playerAnimationManager.isIn_C_A_R_aim)
                    {
                        if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject
                            == this.standRifleAim_CAR_SplineLookConstrainScriptableObject)
                            return;

                        this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(this.standRifleAim_CAR_SplineLookConstrainScriptableObject);
                        this.bodyLookConstraintNodeLeaf.SetWeight(0);
                        return;
                    }

                    if (this.player._currentWeapon != null
                        && this.player._currentWeapon is PrimaryWeapon
                       )
                    {
                        if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject
                            == this.standRifleAimSplineLookConstrainScriptableObject)
                            return;

                        this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(this.standRifleAimSplineLookConstrainScriptableObject);
                        this.bodyLookConstraintNodeLeaf.SetWeight(0);
                        return;
                    }

                    if(this.player._currentWeapon != null
                        && this.player._currentWeapon is SecondaryWeapon
                        && this.playerAnimationManager.isIn_C_A_R_aim)
                    {
                        if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject
                            == this.standPistolAim_CAR_SplineLookConstrainScriptableObject)
                            return;

                        this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(this.standPistolAim_CAR_SplineLookConstrainScriptableObject);
                        this.bodyLookConstraintNodeLeaf.SetWeight(0);
                        return;
                    }

                    if (this.player._currentWeapon != null
                       && this.player._currentWeapon is SecondaryWeapon
                       )
                    {
                        if (this.bodyLookConstraintNodeLeaf.bodyRotationConstrainScriptableObject
                            == this.standPistolAimSplineLookConstrainScriptableObject)
                            return;

                        this.bodyLookConstraintNodeLeaf.SetBodyRotationConstrainSCRP(this.standPistolAimSplineLookConstrainScriptableObject);
                        this.bodyLookConstraintNodeLeaf.SetWeight(0);
                        return;
                    }

                }
                break;
        }

    }
    private void Body_Lean_ConstrainCondition<T>(Player player,T obj)
    {
        if(this.leaningRotationConstrainNodeLeaf.Precondition() == false)
            return;

        if (this.leaningRotationConstrainNodeLeaf.leaningScriptableObject
        == this.leaningConstrainScriptableObject)
            return;

        this.leaningRotationConstrainNodeLeaf.SetLeaningRotaionSCRP(this.leaningConstrainScriptableObject);
        this.leaningRotationConstrainNodeLeaf.SetTargetLeanWeight(0);


    } 
    private void SetLeanSCRP(LeaningRotaionScriptableObject leaningRotaionScriptableObject)
    {
        if (this.leaningRotationConstrainNodeLeaf.leaningScriptableObject
                == leaningRotaionScriptableObject)
            return;

        this.leaningRotationConstrainNodeLeaf.SetLeaningRotaionSCRP(leaningRotaionScriptableObject);
        this.leaningRotationConstrainNodeLeaf.SetTargetLeanWeight(0);
    }

   
    private void RightHand_ConstrainCondition<T>(Player player,T obj)
    {
        if(this.player._currentWeapon == null)
            return;
        //Update RightHand Block & Recoil Data
        switch (this.player._currentWeapon)
        {
            case AutomaticShotgunModel automaticShotgunModel:
                {
                    this.SetRightHandRecoilData(this.shotgunHandRecoilData);
                    this.SetRightHandRecoilBlockData(this.primaryWeaponBlockData);
                }break;
            case PrimaryWeapon primaryWeapon:
                {
                    this.SetRightHandRecoilData(this.rifileHandRecoilData);
                    this.SetRightHandRecoilBlockData(this.primaryWeaponBlockData);
                }break;
            case SecondaryWeapon secondaryWeapon:
                {
                    this.SetRightHandRecoilData(this.pistolHandRecoilData);
                    this.SetRightHandRecoilBlockData(this.secondaryWeaponBlockData);
                }
                break;
        }

        //Update RightHand ScriptableObjectData



        if(obj is HumanShield_GunFu_NodeLeaf humanShield
            && humanShield.curIntphase == HumanShield_GunFu_NodeLeaf.HumanShieldInteractionPhase.Stay)
        {
            if (this.player._currentWeapon is PrimaryWeapon)
                SetRightHandSCRP(this.rightHand_AimDownSight_HumanShield_Primary_SCRP);
            else if (this.player._currentWeapon is SecondaryWeapon)
                SetRightHandSCRP(this.rightHand_AimDownSight_HumanShield_Secondary_SCRP);
        }
        else if(obj is RestrainGunFuStateNodeLeaf restrainGunFuStateNodeLeaf
            && restrainGunFuStateNodeLeaf.curRestrictGunFuPhase == RestrainGunFuStateNodeLeaf.RestrictGunFuPhase.Stay)
        {
            if (this.player._currentWeapon is PrimaryWeapon)
                SetRightHandSCRP(this.rightHand_AimDownSight_Restrain_Primary_SCRP);
            else if (this.player._currentWeapon is SecondaryWeapon)
                SetRightHandSCRP(this.rightHand_AimDownSight_Restrain_Secondary_SCRP);
        }
        else if(this.player._currentWeapon != null
            && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IReloadNode>() == false
            && this.playerStateManager.TryGetCurNodeLeaf<I_OCM_Node>() == false)
        {
            if (this.isProne && this.player._currentWeapon is PrimaryWeapon)
            {

                if (this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.handIK_ConstraintSCRP 
                    == this.rightHand_AimDownSight_Prone_PrimaryWeapon_SCRP)
                    return;

                this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetHandIKConstraintSCRP(this.rightHand_AimDownSight_Prone_PrimaryWeapon_SCRP);
                this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
            }
            else if (this.isProne && this.player._currentWeapon is SecondaryWeapon)
            {
                
                if (this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.handIK_ConstraintSCRP 
                    == this.rightHand_AimDownSight_Prone_SecondaryWeapon_SCRP)
                    return;

                this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetHandIKConstraintSCRP(this.rightHand_AimDownSight_Prone_SecondaryWeapon_SCRP);
                this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
            }
            else if(this.player._currentWeapon is PrimaryWeapon
                && this.playerAnimationManager.isIn_C_A_R_aim)
            {
                this.SetRightHandSCRP(this.rightHand_Target_AimDownSight_CAR_PrimaryWeapon_SCRP);
            }
            else if(this.player._currentWeapon is PrimaryWeapon)
            {
                this.SetRightHandSCRP(this.rightHand_Target_AimDownSight_PrimaryWeapon_SCRP);
            }
            else if(this.player._currentWeapon is SecondaryWeapon
                && this.playerWeaponManuverStateManager.TryGetCurNodeLeaf<IQuickSwitchNode>())
            {
                this.SetRightHandSCRP(this.rightHand_AimDownSight_QuickSwitch_SCRP);
            }
            else if(this.player._currentWeapon is SecondaryWeapon
                && this.playerAnimationManager.isIn_C_A_R_aim)
            {
                this.SetRightHandSCRP(this.rightHand_Target_AimDownSight_CAR_SecondaryWeapon_SCRP);
            }
            else if(this.player._currentWeapon is SecondaryWeapon)
            {
                this.SetRightHandSCRP(this.rightHand_Target_AimDownSight_SecondaryWeapon_SCRP);
            }
        }
    }

   
    private void SetRightHandSCRP(TwoBoneIK_ConstraintSCRP handIK_ConstraintSCRP)
    {
        if (this.rightHandWeaponAimAtIKCinstrainNodeLeaf.handIK_ConstraintSCRP != handIK_ConstraintSCRP)
        {
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetHandIKConstraintSCRP(handIK_ConstraintSCRP);
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }

        if(this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.handIK_ConstraintSCRP != handIK_ConstraintSCRP)
        {
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetHandIKConstraintSCRP(handIK_ConstraintSCRP);
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }
    }
    private void SetRightHandRecoilData(WeaponHandRecoilSCRP weaponHandRecoilSCRP)
    {
        if (this.rightHandWeaponAimAtIKCinstrainNodeLeaf.recoilData != weaponHandRecoilSCRP)
        {
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetHandRecoilData(weaponHandRecoilSCRP);
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }

        if (this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.recoilData != weaponHandRecoilSCRP)
        {
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetHandRecoilData(weaponHandRecoilSCRP);
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }
    }
    private void SetRightHandRecoilBlockData(WeaponHandBlockSCRP weaponHandBlockSCRP)
    {
        if (this.rightHandWeaponAimAtIKCinstrainNodeLeaf.blockData != weaponHandBlockSCRP)
        {
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetHandBlockData(weaponHandBlockSCRP);
            this.rightHandWeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }

        if (this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.blockData != weaponHandBlockSCRP)
        {
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetHandBlockData(weaponHandBlockSCRP);
            this.rightHand_Prone_WeaponAimAtIKCinstrainNodeLeaf.SetWeight(0);
        }
    }

    private void LegsConstrainCondition<T>(Player player,T obj) 
    { 
        if(obj is PlayerProneStateNodeLeaf proneStateNodeLeaf
            && proneStateNodeLeaf._curPhase == NodePhase.Enter)
        {
            this.proneLegsConstrainNodeLeaf.SetSCRP(this.proneLegsBlendingConstrainSCRP);
            this.proneLegsConstrainNodeLeaf.SetTransitionSpeed(50);
            this.legsEnableWeightConstraintNodeLeaf.SetWeight(1);
        }

        
    }
}
