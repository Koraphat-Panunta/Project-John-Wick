
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class AimingProceduralAnimate:IObserverPlayer 
{
    private IWeaponAdvanceUser weaponAdvancer;
    private IAimingProceduralAnimate proceduralAnimator;
    private Transform aimPosReference;
    private MultiAimConstraint aimConstraint;


   public AimingProceduralAnimate(IWeaponAdvanceUser weaponAdvanceUser,IAimingProceduralAnimate aimingProceduralAnimate,Transform aimPosReference,MultiAimConstraint aimCronstrain,Player player)
   {
        this.weaponAdvancer = weaponAdvanceUser;
        this.proceduralAnimator = aimingProceduralAnimate;
        this.aimPosReference = aimPosReference;
        this.aimConstraint = aimCronstrain;

        player.AddObserver(this);
   }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(player.currentWeapon == null)
        {
            aimConstraint.weight = weaponAdvancer.weaponManuverManager.aimingWeight;
            return;
        }


        if(playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            //aimPosReference.position = weaponAdvancer.pointingPos;
            aimConstraint.weight = weaponAdvancer.weaponManuverManager.aimingWeight;
        }
        else if(playerAction == SubjectPlayer.PlayerAction.QuickDraw)
        {
            aimConstraint.weight = weaponAdvancer.weaponManuverManager.aimingWeight;
        }
        else if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            aimConstraint.weight = weaponAdvancer.weaponManuverManager.aimingWeight;
        }
    }

    public void OnNotify(Player player)
    {
    }
}
