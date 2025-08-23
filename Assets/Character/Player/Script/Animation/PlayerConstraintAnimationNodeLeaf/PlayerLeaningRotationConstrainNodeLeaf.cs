using System;
using UnityEngine;

public class PlayerLeaningRotationConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    private LeaningRotaionScriptableObject leaningScriptableObject;
    private LeaningRotation leaningRotation;
    private IWeaponAdvanceUser weaponAdvanceUser;
    private float checkDistance => leaningScriptableObject.checkDistance;
    private int numberRaycast => leaningScriptableObject.numberRaycast;
    private float targetLeanWeight;
    private float leaningSpeed => leaningScriptableObject.leaningSpeed;
    private Player player;
    private Vector3 playerCastAnchorPos => player.RayCastPos.position;
    public PlayerLeaningRotationConstrainNodeLeaf(Player player,LeaningRotaionScriptableObject leaningScriptableObject,LeaningRotation leaningRotation,IWeaponAdvanceUser weaponAdvanceUser,Func<bool> precondition) : base(precondition)
    {
        this.leaningScriptableObject = leaningScriptableObject;
        this.leaningRotation = leaningRotation;
        this.weaponAdvanceUser = weaponAdvanceUser;
        this.player = player;
    }

    public override void Enter()
    {
        targetLeanWeight = 0;
        base.Enter();
    }
    public override void UpdateNode()
    {
        leaningRotation.SetWeight(weaponAdvanceUser._weaponManuverManager.aimingWeight, leaningScriptableObject);
        leaningRotation.SetLeaningLeftRight(Mathf.MoveTowards(leaningRotation.GetLeaningLeftRight() 
            , targetLeanWeight * leaningRotation.leaningLeftRightSplineMax, leaningSpeed * Time.deltaTime));

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        float targetWeight = 0;
        if (player.curShoulderSide == Player.ShoulderSide.Left)
        {
            Vector3 castDir = weaponAdvanceUser._pointingPos - playerCastAnchorPos;

            targetWeight = 1;
            for (int i = 0; i < numberRaycast; i++)
            {
                float distance = checkDistance * (float)((float)i / (float)numberRaycast);
                Vector3 castPos = playerCastAnchorPos + (Vector3.Cross(castDir.normalized, Vector3.down)
                    * distance);

                if (PointingBlock(castPos, castDir, out RaycastHit hit))
                {
                    if (Vector3.Distance(hit.point, weaponAdvanceUser._pointingPos) < .67f)
                        targetWeight = 0f;
                   
                    break;
                }
                else
                    targetWeight -= (float)(1f / (float)numberRaycast);

            }
            this.targetLeanWeight = - leaningScriptableObject.leanWeightCurve.Evaluate(targetWeight);

        }
        else if (player.curShoulderSide == Player.ShoulderSide.Right)
        {
            Vector3 castDir = weaponAdvanceUser._pointingPos - playerCastAnchorPos;
            targetWeight = 1;
            for (int i = 0; i < numberRaycast; i++)
            {
                float distance = checkDistance * (float)((float)i/(float)numberRaycast);
                Vector3 castPos = playerCastAnchorPos + (Vector3.Cross(castDir.normalized, Vector3.up)
                    * distance);
               
                if (PointingBlock(castPos,castDir,out RaycastHit hit))
                {
                    if(Vector3.Distance(hit.point,weaponAdvanceUser._pointingPos) < .67f)
                        targetWeight = 0f;
                   
                    break;
                }
                else
                    targetWeight -= (float)(1f / (float)numberRaycast);

            }
            this.targetLeanWeight = leaningScriptableObject.leanWeightCurve.Evaluate(targetWeight);

        }
        Debug.Log("targetLeanWeight = " + this.targetLeanWeight);

        base.FixedUpdateNode();
    }
    public override void Exit()
    {
        base.Exit();
    }

    private bool PointingBlock(Vector3 startCastPos,Vector3 castDir,out RaycastHit hit)
    {


        if(Physics.Raycast(startCastPos,castDir.normalized, out hit, leaningScriptableObject.distanceCheck, leaningScriptableObject.castingCheckLayer.value))
        {
            return true;
        }
        //if(Physics.Raycast(startCastPos,castDir.normalized,out RaycastHit hit, leaningScriptableObject.distanceCheck, leaningScriptableObject.castingCheckLayer.value))
        //{

           
        //}

        return false;
    }
  
   
}
