using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation :MonoBehaviour,IObserverPlayer
{
    public Animator animator;
    public enum parameterName
    {
        ForBack_Ward,
        Side_LR,
        IsSprinting,
        Reloading,
        TacticalReload
    }
    public enum layerName
    {
        BaseLayer,
        Aiming,
        Reloading

    }
    public Dictionary<parameterName, string> animationParameter = new Dictionary<parameterName, string>();
    public Dictionary<layerName,string> animationLayer = new Dictionary<layerName,string>();
    void Start()
    {
        animationParameter.Add(parameterName.ForBack_Ward, "ForBack_Ward");
        animationParameter.Add(parameterName.Side_LR, "Side_LR");
        animationParameter.Add(parameterName.IsSprinting, "IsSprinting");
        animationParameter.Add(parameterName.Reloading, "Reloading");
        animationParameter.Add(parameterName.TacticalReload, "TacticalReload");

        animationLayer.Add(layerName.BaseLayer, layerName.BaseLayer.ToString());
        animationLayer.Add(layerName.Aiming, layerName.Aiming.ToString());
        animationLayer.Add(layerName.Reloading, layerName.Reloading.ToString());

    }
    private void OnEnable()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            player.AddObserver(this);
        }
    }
    private void OnDisable()
    {
        if (TryGetComponent<Player>(out Player player))
        {
            player.RemoveObserver(this);
        }
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void AnimateMove(PlayerMovement playerMovement)
    {
        float var = (playerMovement.curVelocity_Local.z) / playerMovement.move_MaxSpeed;
        animator.SetFloat(animationParameter[parameterName.ForBack_Ward], var);
        var = (playerMovement.curVelocity_Local.x) / playerMovement.move_MaxSpeed;
        animator.SetFloat(animationParameter[parameterName.Side_LR], var);
    }
    public void AnimateSprint(PlayerMovement playerMovement,bool isSprinting)
    {
        if (isSprinting == true)
        {
            animator.SetBool(animationParameter[parameterName.IsSprinting], true);
        }
        else
        {
            animator.SetBool(animationParameter[parameterName.IsSprinting], false);
        }
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.Move)
        {
            AnimateMove(player.playerMovement);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Idle)
        {
            AnimateMove(player.playerMovement);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Sprint)
        {
            AnimateSprint(player.playerMovement,true);
        }
        else
        {
            AnimateSprint(player.playerMovement, false);
        }

        //Animate Weapon Animation
        if (playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            animator.SetLayerWeight(1,player.playerWeaponCommand.CurrentWeapon.weapon_StanceManager.AimingWeight);
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            animator.SetLayerWeight(1, player.playerWeaponCommand.CurrentWeapon.weapon_StanceManager.AimingWeight);
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            //Animate Firing
        }

        //Animator Override
        if(playerAction == SubjectPlayer.PlayerAction.GetWeapon)
        {
            this.animator.runtimeAnimatorController = player.weaponSocket.weaponSingleton.GetOverride_Player_Controller();
        }

        if(playerAction == SubjectPlayer.PlayerAction.Reloading)
        {
            Reload.ReloadType reloadType = player.weaponSocket.CurWeapon.weapon_stateManager.reloadState.reloadType;
            if (reloadType == Reload.ReloadType.TacticalReload)
            {
                this.animator.SetTrigger(animationParameter[parameterName.TacticalReload]);
                StartCoroutine(ReloadTiming());
            }
            if(reloadType == Reload.ReloadType.ReloadMagOut)
            {
                this.animator.SetTrigger(animationParameter[parameterName.Reloading]);
                StartCoroutine(ReloadTiming());
            }
        }
       
    }
    IEnumerator ReloadTiming()
    {
        this.animator.SetLayerWeight(2, 1);
        yield return new WaitForSeconds(2.6f);
        this.animator.SetLayerWeight(2, 0);
       
    }
}
