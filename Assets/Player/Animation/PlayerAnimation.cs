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
        if(player.curShoulderSide == Player.ShoulderSide.Left)
        {
            
            animator.SetFloat("SholderSide", Mathf.Clamp(animator.GetFloat("SholderSide") - 2*Time.deltaTime,-1,1));
        }
        else if(player.curShoulderSide == Player.ShoulderSide.Right)
        {
            animator.SetFloat("SholderSide", Mathf.Clamp(animator.GetFloat("SholderSide") + 2 * Time.deltaTime, -1, 1));
        }
        if(playerAction == SubjectPlayer.PlayerAction.Move)
        {
            AnimateMove(player.playerMovement);
            if (player.playerStateManager.move == player.playerStateManager.moveInCover)
            {
                animator.SetBool("IsTakeCover", true);
            }
            else
            {
                animator.SetBool("IsTakeCover", false);
            }

        }
        if(playerAction == SubjectPlayer.PlayerAction.Idle)
        {
            AnimateMove(player.playerMovement);
            if (player.playerStateManager.idle == player.playerStateManager.idleInCover)
            {
                animator.SetBool("IsTakeCover", true);
            }
            else
            {
                animator.SetBool("IsTakeCover", false);
            }
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
            animator.SetLayerWeight(2,player.currentWeapon.aimingWeight);
            animator.SetLayerWeight(1,1- player.currentWeapon.aimingWeight);
            animator.SetFloat("AimingWeigth", player.currentWeapon.aimingWeight);
        }
        if(playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            if (player.currentWeapon != null)
            {
                animator.SetLayerWeight(2, player.currentWeapon.aimingWeight);
                animator.SetLayerWeight(1, 1 - player.currentWeapon.aimingWeight);
                animator.SetFloat("AimingWeigth", player.currentWeapon.aimingWeight);
            }
            else
            {
                animator.SetLayerWeight(2,0);
                animator.SetLayerWeight(1, 1 );
                animator.SetFloat("AimingWeigth",0);
            }
        }
        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            //Animate Firing
        }

        //Animator Override
        if(playerAction == SubjectPlayer.PlayerAction.PickUpWeapon)
        {
            this.animator.runtimeAnimatorController = player.currentWeapon._weaponOverrideControllerPlayer;
        }

        if(playerAction == SubjectPlayer.PlayerAction.Reloading)
        {
            ReloadType reloadType = (player.currentWeapon.bulletStore[BulletStackType.Chamber]>0)?ReloadType.MAGAZINE_TACTICAL_RELOAD:ReloadType.MAGAZINE_RELOAD;
            if (reloadType == ReloadType.MAGAZINE_TACTICAL_RELOAD)
            {
                this.animator.SetTrigger(animationParameter[parameterName.TacticalReload]);
                StartCoroutine(ReloadTiming(player.currentWeapon.reloadSpeed));
            }
            if(reloadType == ReloadType.MAGAZINE_RELOAD)
            {
                this.animator.SetTrigger(animationParameter[parameterName.Reloading]);
                StartCoroutine(ReloadTiming(player.currentWeapon.reloadSpeed));
            }
        }
  
    }
    IEnumerator ReloadTiming(float reloadTime)
    {
        //Debug.Log("ReloadSpeed" + reloadTime);
        while (this.animator.GetLayerWeight(3) < 1)
        {
            this.animator.SetLayerWeight(3, this.animator.GetLayerWeight(3)+Time.deltaTime*8);
            yield return null;
        }
        this.animator.SetLayerWeight(3, 1);
        yield return new WaitForSeconds(reloadTime);
        while (this.animator.GetLayerWeight(3) > 0)
        {
            this.animator.SetLayerWeight(3, this.animator.GetLayerWeight(3) - Time.deltaTime * 8);
            yield return null;
        }
        this.animator.SetLayerWeight(3, 0);
       
    }
}
