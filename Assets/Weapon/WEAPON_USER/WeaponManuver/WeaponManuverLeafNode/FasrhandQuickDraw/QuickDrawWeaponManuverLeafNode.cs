using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class QuickDrawWeaponManuverLeafNode : WeaponManuverLeafNode
{
    Weapon weapon => weaponAdvanceUser.currentWeapon;
    private Weapon secondHandWeapon;
    public enum QuickDrawPhase
    {
        Draw,
        Stay,
        HolsterSecondary,
        HolsterPrimary,
        Exit
    }
    public QuickDrawPhase quickDrawPhase { get; protected set; }
    private bool isComplete;
    WeaponManuverManager weaponManuverManager => weaponAdvanceUser.weaponManuverManager;
    protected float elapseDrawTime;
    protected float elapseHolsterTime;

    private Vector3 beforeEnter_AimProcedural;
    public QuickDrawWeaponManuverLeafNode(IWeaponAdvanceUser weaponAdvanceUser, Func<bool> preCondition) : base(weaponAdvanceUser, preCondition)
    {

    }

    public override void Enter()
    {
        this.secondHandWeapon = weaponAdvanceUser.currentWeapon;
        if(weaponAdvanceUser.weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
        {
            weapon.AttachWeaponToSecondHand(weaponAdvanceUser.leftHandSocket);
            quickDrawPhase = QuickDrawPhase.Draw;
            weaponAfterActionPlayer.QuickDraw(weapon, quickDrawPhase);
            weapon.ChangeActionManualy(weapon.restNode);

            MultiAimConstraint multiAim = (weaponAdvanceUser as IAimingProceduralAnimate)._aimConstraint;

            beforeEnter_AimProcedural = multiAim.data.offset;
        }
        isComplete = false;
        elapseDrawTime = 0f;
        elapseHolsterTime = 0f;
    }

    public override void Exit()
    {
        secondHandWeapon = null;
        quickDrawPhase = QuickDrawPhase.Exit;
        if (weaponAdvanceUser.weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
            weaponAfterActionPlayer.QuickDraw(weapon, quickDrawPhase);

        MultiAimConstraint multiAim = (weaponAdvanceUser as Player)._aimConstraint;

        multiAim.data.offset = beforeEnter_AimProcedural;
    }

    public override void FixedUpdateNode()
    {
       
    }

    public override bool IsComplete()
    {
        return isComplete;
    }

    public override bool IsReset()
    {
        if (IsComplete())
            return true;

        return false;

    }

    public override bool Precondition()
    {
        return base.Precondition();
    }

    public override void UpdateNode()
    {
        MultiAimConstraint multiAim = (weaponAdvanceUser as IAimingProceduralAnimate)._aimConstraint;



        switch (quickDrawPhase)
        {
            case QuickDrawPhase.Draw:
                {
                    float drawGrab = 0.184f;
                    float drawAim = 0.34f;
                    elapseDrawTime += Time.deltaTime;

                    multiAim.data.offset = Vector3.Lerp(beforeEnter_AimProcedural, new Vector3(48f, 0f, 0f), elapseDrawTime/drawAim);

                    if (elapseDrawTime >= drawGrab)
                        (weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon).AttatchWeaponToNoneOverrideAnimator(weaponAdvanceUser);

                    if (elapseDrawTime >= drawAim)
                    {
                        quickDrawPhase = QuickDrawPhase.Stay;

                        if (weaponAdvanceUser.weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
                            weaponAfterActionPlayer.QuickDraw(weapon, quickDrawPhase);
                    }
                }
                break;
            case QuickDrawPhase.Stay:
                {
                    WeaponAfterActionPlayer weaponAfterActionPlayer = weaponAdvanceUser.weaponAfterAction as WeaponAfterActionPlayer;    
                    weaponAfterActionPlayer.QuickDraw(weapon, quickDrawPhase);

                    weaponManuverManager.aimingWeight = 1;

                    if (weaponManuverManager.isSwitchWeaponManuver)
                    {
                        quickDrawPhase = QuickDrawPhase.HolsterSecondary;

                            weaponAfterActionPlayer.QuickDraw(weapon, quickDrawPhase);

                    }

                    weaponManuverManager.WeaponCommanding();

                    if(weaponManuverManager.isAimingManuver == false)
                        quickDrawPhase = QuickDrawPhase.HolsterPrimary;

                    if (weaponManuverManager.curWeapon.currentEventNode is IReloadNode)
                    {
                        HolsteringPrimaryWeapon();
                        weapon.Reload();
                        isComplete = true;
                    }
     
                        
                }
                break;
            case QuickDrawPhase.HolsterSecondary:
                {
                    elapseHolsterTime += Time.deltaTime;

                    float holsterSecondary = 0.34f;
                    float drawPrimary = 0.42f;

                    if (elapseHolsterTime >= holsterSecondary && weapon == weaponAdvanceUser.weaponBelt.secondaryWeapon as Weapon) 
                        weapon.AttachWeaponToSocketNoneAnimatorOverride(weaponAdvanceUser.weaponBelt.secondaryWeaponSocket);

                    multiAim.data.offset = Vector3.Lerp(new Vector3(48f, 0f, 0f),beforeEnter_AimProcedural , elapseHolsterTime / drawPrimary);

                    if (elapseHolsterTime >= drawPrimary)
                    {
                        this.secondHandWeapon.AttatchWeaponTo(weaponAdvanceUser);//++
                        isComplete = true;
                    }
                }
                break;
            case QuickDrawPhase.HolsterPrimary: 
                {
                    HolsteringPrimaryWeapon();
                    isComplete= true;
                }
                break;
        }
        weaponAdvanceUser.weaponAfterAction.AimDownSight(weapon);
    }

    private void HolsteringPrimaryWeapon()//++
    {
        if (this.secondHandWeapon == weaponAdvanceUser.weaponBelt.primaryWeapon as Weapon)
            this.secondHandWeapon.AttachWeaponToSocket(weaponAdvanceUser.weaponBelt.primaryWeaponSocket);
        else
            this.secondHandWeapon.DropWeapon();

        weapon.AttatchWeaponTo(weaponAdvanceUser);

        if (weaponAdvanceUser.weaponAfterAction is WeaponAfterActionPlayer weaponAfterActionPlayer)
            weaponAfterActionPlayer.QuickDraw(weapon, quickDrawPhase);
    }

   
}
