using UnityEngine;

public partial class PlayerConstrainAnimationManager : IObserverPlayer
{
    public void OnNotify<T>(Player player, T node)
    {
        if(node is GunFuExecute_Single_NodeLeaf gunFuExecuteNodeLeaf)
        {
            gunFuExecuteNodeLeaf.animationTriggerEventPlayer.SubscribeEvent("EnableIK", this.EnableIK);
            if(gunFuExecuteNodeLeaf.curPhase == PlayerStateNodeLeaf.NodePhase.Exit)
                isEnableIK = false;
        }

        if(node is SubjectPlayer.NotifyEvent playerEvent
            && playerEvent == SubjectPlayer.NotifyEvent.Firing)
        {

            Debug.Log("PLAYER Constraint firing");
            this.rightHand_AimDownSight_ProneUp_Primary_Constraint_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_ProneUp_Secondary_Constraint_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_ProneDown_Primary_Constraint_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_ProneDown_Secondary_Constraint_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_QuickSwitch_Constraint_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_CAR_Constraint_PrimaryWeapon_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_Constraint_PrimaryWeapon_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_CAR_Constraint_SecondaryWeapon_NodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_AimDownSight_Constraint_SecondaryWeapon_NodeLeaf.TriggeRecoilWeight(1);

            this.rightHand_ADS_humanShield_rifle_AnimationConstraintNodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_ADS_humanShield_secondary_AnimationConstraintNodeLeaf.TriggeRecoilWeight(1);

            this.rightHand_ADS_restrict_rifle_AnimationConstraintNodeLeaf.TriggeRecoilWeight(1);
            this.rightHand_ADS_restrict_pistol_AnimationConstraintNodeLeaf.TriggeRecoilWeight(1);
        }
    }
}
