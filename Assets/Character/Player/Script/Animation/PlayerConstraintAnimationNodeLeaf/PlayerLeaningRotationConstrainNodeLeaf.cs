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
        leaningRotation.SetLeaningLeftRight(Mathf.Lerp(leaningRotation.GetLeaningLeftRight() 
            , targetLeanWeight * leaningRotation.leaningLeftRightSplineMax, leaningSpeed * Time.deltaTime));

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        float targetWeight = 0;
        Vector3 castDir = weaponAdvanceUser._pointingPos - playerCastAnchorPos;
        Vector3 castFindTargetWeightBeginPos = playerCastAnchorPos;
        Vector3 castFindTargetWeightEndPos = playerCastAnchorPos;
        if (player.curShoulderSide == Player.ShoulderSide.Left)
        {

            for (int i = 0; i <= numberRaycast; i++)
            {
                float distance = checkDistance * (float)((float)i / (float)numberRaycast);
                Vector3 castPosDir = Vector3.Cross(castDir.normalized, Vector3.down);
                Vector3 castPos = playerCastAnchorPos + (castPosDir
                    * distance);

                if (PointingBlock(castPos, castDir, out RaycastHit hit))
                {
                    castFindTargetWeightBeginPos += (castDir.normalized * (Vector3.Distance(castPos, hit.point) + 0.05f));
                    castFindTargetWeightEndPos += (castDir.normalized * (Vector3.Distance(castPos, hit.point) + 0.05f));
                    //Debug.DrawLine(castPos, hit.point, Color.red, 0.1f);
                    //Debug.DrawRay(castFindTargetWeightEndPos, castPosDir,Color.green,0.1f);
                    //Debug.DrawLine(castFindTargetWeightBeginPos, castFindTargetWeightEndPos, Color.blue, 0.1f);
                    if (PointingBlock(castFindTargetWeightEndPos, (hit.point - castFindTargetWeightEndPos).normalized , out RaycastHit hit2))
                    {
                        targetWeight = 1- Mathf.Clamp01(Vector3.Distance(castFindTargetWeightBeginPos, hit2.point)/checkDistance);
                    }
                    else
                    {
                        targetWeight = 1 - ((float)i / numberRaycast);
                    }

                    if (Vector3.Distance(hit.point, weaponAdvanceUser._pointingPos) < .67f)
                        targetWeight = 0f;
                   
                    break;
                }
                else
                    castFindTargetWeightEndPos = castPos;

            }
            this.targetLeanWeight = - leaningScriptableObject.leanWeightCurve.Evaluate(targetWeight);

        }
        else if (player.curShoulderSide == Player.ShoulderSide.Right)
        {
            for (int i = 0; i <= numberRaycast; i++)
            {
                float distance = checkDistance * (float)((float)i/(float)numberRaycast);
                Vector3 castPosDir = Vector3.Cross(castDir.normalized, Vector3.up);
                Vector3 castPos = playerCastAnchorPos + (castPosDir
                    * distance);

                if (i == 0)
                    castFindTargetWeightBeginPos = castPos;

                if (PointingBlock(castPos,castDir,out RaycastHit hit))
                {

                    castFindTargetWeightBeginPos += (castDir.normalized * (Vector3.Distance(castPos, hit.point) + 0.05f));
                    castFindTargetWeightEndPos += (castDir.normalized * (Vector3.Distance(castPos, hit.point) + 0.05f));
                    //Debug.DrawLine(castPos, hit.point, Color.red, 0.1f);
                    //Debug.DrawRay(castFindTargetWeightEndPos, castPosDir, Color.green, 0.1f);
                    //Debug.DrawLine(castFindTargetWeightBeginPos, castFindTargetWeightEndPos, Color.blue, 0.1f);
                    if (PointingBlock(castFindTargetWeightEndPos, (hit.point - castFindTargetWeightEndPos).normalized, out RaycastHit hit2))
                    {
                        targetWeight = 1 - Mathf.Clamp01(Vector3.Distance(castFindTargetWeightBeginPos, hit2.point) / checkDistance);
                    }
                    else
                    {
                        targetWeight = 1 - ((float)i / numberRaycast);
                    }
                    if (Vector3.Distance(hit.point,weaponAdvanceUser._pointingPos) < .67f)
                        targetWeight = 0f;
                   
                    break;
                }
                else
                    targetWeight -= (float)(1f / (float)numberRaycast);

            }
            this.targetLeanWeight = leaningScriptableObject.leanWeightCurve.Evaluate(targetWeight);

        }

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
