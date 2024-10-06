using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeanCover 
{
    private MultiRotationConstraint multiRotationConstraint;
    private CrosshairController crosshairController;
    private LayerMask layerMask;
    public enum LeanDir
    {
        Left,
        Right,
        None
    }
    private LeanDir leandir = LeanDir.None;
    private float leanWeight = 0.5f;
    private float leanSpeed = 5;
    public LeanCover(MultiRotationConstraint multiRotationConstraint,CrosshairController crosshairController)
    {
       this.multiRotationConstraint = multiRotationConstraint;
        this.crosshairController = crosshairController;
        leanWeight = 0.5f;
        layerMask = LayerMask.GetMask("Default");
    }
    public void LeaningUpdate(Transform shootPoint)
    {
        leaningCheck(shootPoint);
        var source = multiRotationConstraint.data.sourceObjects;
        source.SetWeight(0, leanWeight);
        source.SetWeight(1, 1 - leanWeight);
        multiRotationConstraint.data.sourceObjects = source;
    }
    private void leaningCheck(Transform shootpoint)
    {
        Vector3 CrosshairScreenPos = Camera.main.WorldToScreenPoint(crosshairController.TargetAim.transform.position);
        //Debug.Log("CrosshairScreenPos :" + CrosshairScreenPos);
        Vector3 ImpactpointScreenPos = Vector2.zero;
        if(Physics.SphereCast(shootpoint.position,0.45f,(crosshairController.TargetAim.transform.position-shootpoint.position).normalized,out RaycastHit hitInfo, 1000,layerMask))
        {
            Debug.DrawLine(shootpoint.position, hitInfo.point);
            //Debug.Log("Hit point =" + hitInfo.point);
            ImpactpointScreenPos = Camera.main.WorldToScreenPoint(hitInfo.point);
            if (Vector3.Distance(hitInfo.point, shootpoint.position)<3)
            {
                if (ImpactpointScreenPos.x > CrosshairScreenPos.x + 5f)
                {
                    //Debug.Log("LeanLeft");
                    leandir = LeanDir.Left;
                    leanWeight = Mathf.Lerp(leanWeight, 1, Time.deltaTime * leanSpeed);
                }
                else if (ImpactpointScreenPos.x < CrosshairScreenPos.x - 5f)
                {
                    //Debug.Log("LeanRight");
                    leandir = LeanDir.Right;
                    leanWeight = Mathf.Lerp(leanWeight, 0, Time.deltaTime * leanSpeed);
                }
                else
                {
                    //Debug.Log("LeanNone");
                    leandir = LeanDir.None;
                    leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
                }
            }
            else
            {
                //Debug.Log("LeanNone");
                leandir = LeanDir.None;
                leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
            }
        }
        else
        {
            Debug.DrawLine(shootpoint.position, hitInfo.point);
            //Debug.Log("Hit point =" + hitInfo.point);
            ImpactpointScreenPos = Camera.main.WorldToScreenPoint(hitInfo.point);
        }
       
        //Debug.Log("ImpactpointScreenPos :" + ImpactpointScreenPos);
        
    }
    public void LeanRecovery() 
    {
        //Debug.Log("LeanNone");
        leandir = LeanDir.None;
        leanWeight = Mathf.Lerp(leanWeight, 0.5f, Time.deltaTime * leanSpeed);
        var source = multiRotationConstraint.data.sourceObjects;
        source.SetWeight(0, leanWeight);
        source.SetWeight(1, 1 - leanWeight);
        multiRotationConstraint.data.sourceObjects = source;
    }

}
