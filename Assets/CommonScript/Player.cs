using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Reload;

public class Player : Character
{
    private RotateObjectToward rotateObjectToward;
    private Animator animator;
    public CameraKickBack cameraKickBack;
    public CameraZoom cameraZoom;
    private void Start()
    {
        rotateObjectToward = new RotateObjectToward();
        animator = GetComponent<Animator>();
    }
    public override void Aiming(Weapon weapon)
    {
        rotateObjectToward.RotateTowards(Camera.main.transform.forward,gameObject,6);
        animator.SetLayerWeight(1,weapon.weapon_StanceManager.AimingWeight);
        cameraZoom.ZoomIn(weapon);
    }

    public override void Firing(Weapon weapon)
    {
        cameraKickBack.Performed(weapon);
    }

    public override void LowReadying(Weapon weapon)
    {
        animator.SetLayerWeight(1, weapon.weapon_StanceManager.AimingWeight);
        cameraZoom.ZoomOut(weapon);
    }

    public override void Reloading(Weapon weapon, Reload.ReloadType reloadType)
    {
        if (reloadType == ReloadType.TacticalReload)
        {
            animator.SetTrigger("TacticalReload");
            animator.SetLayerWeight(2, 1);
        }
        else if(reloadType == ReloadType.ReloadMagOut)
        {
            animator.SetTrigger("Reloading");
            animator.SetLayerWeight(2, 1);
        }
        else if(reloadType == ReloadType.ReloadFinished)
        {
            animator.SetLayerWeight(2, 0);
        }
    }
}
