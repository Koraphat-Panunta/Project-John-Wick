using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeanCover:IObserverPlayer
{
    private MultiRotationConstraint multiRotationConstraint;
    private CrosshairController crosshairController;
    private LayerMask layerMask;
    private Transform shootPoint;
    private Player player;
    public enum LeanDir
    {
        Left,
        Right,
        None
    }
    private LeanDir leandir = LeanDir.None;
    private float leanWeight = 0.5f;
    private float leanSpeed = 5;
    public LeanCover(MultiRotationConstraint multiRotationConstraint,CrosshairController crosshairController,Player player)
    {
       this.multiRotationConstraint = multiRotationConstraint;
        this.crosshairController = crosshairController;
        leanWeight = 0.5f;
        layerMask = LayerMask.GetMask("Default");
        this.player = player;
        shootPoint = player.RayCastPos;
        player.AddObserver(this);
    }

    float leanRecoverTimer = .5f;
    float elaspeLeanRecover;
    public void LeaningUpdate(Transform shootPoint)
    {

        leaningCheck(shootPoint);

        if(leandir == LeanDir.None)
        {
            leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
        }
        else if(leandir == LeanDir.Right)
        {
            if(player.curShoulderSide == Player.ShoulderSide.Right)
                leanWeight = Mathf.Lerp(leanWeight, 0, Time.deltaTime * leanSpeed);
            else
                leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
        }
        else if(leandir == LeanDir.Left)
        {
            if (player.curShoulderSide == Player.ShoulderSide.Left)
                leanWeight = Mathf.Lerp(leanWeight, 1, Time.deltaTime * leanSpeed);
            else
                leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
        }

        var source = multiRotationConstraint.data.sourceObjects;
        source.SetWeight(0, leanWeight);
        source.SetWeight(1, 1 - leanWeight);
        multiRotationConstraint.data.sourceObjects = source;
    }
    private void leaningCheck(Transform shootpoint)
    {
       

        Vector3 CrosshairScreenPos = Camera.main.WorldToScreenPoint(crosshairController.TargetAim.transform.position);
        Vector3 ImpactpointScreenPos = Vector2.zero;
        if (Physics.Raycast(shootpoint.position, (crosshairController.TargetAim.transform.position - shootpoint.position).normalized, out RaycastHit hit, 1000, layerMask))
        {
            if (Vector3.Distance(hit.point, crosshairController.TargetAim.position) < 3f)
            {
                elaspeLeanRecover = Mathf.Clamp(elaspeLeanRecover - Time.deltaTime, 0, leanRecoverTimer);
                if(elaspeLeanRecover <=0)
                    leandir = LeanDir.None;
                return;
            }
        }


        Physics.BoxCast(shootpoint.position, new Vector3(0.45f, 0.0001f, 0.45f), (crosshairController.TargetAim.transform.position - shootpoint.position).normalized,out RaycastHit hitInfo, shootpoint.transform.rotation, 1000, layerMask);
        Debug.DrawLine(shootpoint.position, crosshairController.TargetAim.transform.position, Color.green);

        ImpactpointScreenPos = Camera.main.WorldToScreenPoint(hitInfo.point);

        Debug.DrawLine(shootpoint.position, hitInfo.point, Color.red);

       
        if (Vector3.Distance(hitInfo.point, shootpoint.position) > 5)
        {
            elaspeLeanRecover = Mathf.Clamp(elaspeLeanRecover - Time.deltaTime, 0, leanRecoverTimer);
            if (elaspeLeanRecover <= 0)
                leandir = LeanDir.None;
            return;
        }

        if(ImpactpointScreenPos.x < CrosshairScreenPos.x)
        {
            leandir = LeanDir.Right;
            elaspeLeanRecover = leanRecoverTimer;
            return;
        }

        if (ImpactpointScreenPos.x > CrosshairScreenPos.x)
        {
            leandir = LeanDir.Left;
            elaspeLeanRecover = leanRecoverTimer;
            return;
        }



    }
    public void LeanRecovery() 
    {
        //Debug.Log("LeanNone");

        elaspeLeanRecover = 0;
        leandir = LeanDir.None;
        leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
        var source = multiRotationConstraint.data.sourceObjects;
        source.SetWeight(0, leanWeight);
        source.SetWeight(1, 1 - leanWeight);
        multiRotationConstraint.data.sourceObjects = source;
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.LowReady
            || playerAction == SubjectPlayer.PlayerAction.Sprint)
            LeanRecovery();
        if (playerAction == SubjectPlayer.PlayerAction.Aim)
        {
            //if(player.currentWeapon != null)
            //    shootPoint = player.currentWeapon.bulletSpawnerPos;
            //else shootPoint = player.RayCastPos;
            LeaningUpdate(shootPoint);
        }
    }

    public void OnNotify(Player player)
    {
    }
}
