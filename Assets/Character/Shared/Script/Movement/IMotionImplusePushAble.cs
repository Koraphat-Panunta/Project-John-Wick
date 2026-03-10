using System.Collections;
using UnityEngine;
using static IMotionImplusePushAble;

public interface IMotionImplusePushAble 
{
    public MovementCompoent movementCompoent { get; }
    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get; set; }
    public enum PushMode 
    {
        IgnoreMomentum,
        MaintainMomentum,
    }
   
    public void AddForcePushInstantly(Vector3 force,PushMode pushMode);
    public void AddForcePushVelocityChange(Vector3 force, PushMode pushMode,float velocityChangeDuration);
}
public class MotionImplusePushAbleBehavior
{
   
    public void AddInstantVelocity(IMotionImplusePushAble motionImplusePushAble,Vector3 v, PushMode pushMode)
    {
        MovementCompoent movementCompoent = motionImplusePushAble.movementCompoent;
        switch (pushMode)
        {
            case PushMode.IgnoreMomentum:
                {
                    movementCompoent.CancleMomentum();
                    movementCompoent.curMoveVelocity_World = v;
                }
                break;
            case PushMode.MaintainMomentum:
                {
                    movementCompoent.curMoveVelocity_World += v;
                }
                break;
        }
    }
    public void AddChangeVelocity(IMotionImplusePushAble motionImplusePushAble, Vector3 v, PushMode pushMode,float velocityChangeDuration)
    {
        MovementCompoent movementCompoent = motionImplusePushAble.movementCompoent;
        switch (pushMode)
        {
            case PushMode.IgnoreMomentum:
                {
                    movementCompoent.CancleMomentum();
                    movementCompoent.userMovement.StartCoroutine(VelotityChangeIgnoreMomentum(velocityChangeDuration, v,movementCompoent));
                }
                break;
            case PushMode.MaintainMomentum:
                {
                    movementCompoent.userMovement.StartCoroutine(VelotityChangeMaintainMomentum(velocityChangeDuration, v, movementCompoent));
                }
                break;
        }
    }



    protected IEnumerator VelotityChangeIgnoreMomentum(float velocityChangeDuration, Vector3 v,MovementCompoent movementCompoent)
    {
        float time = 0;
        Vector3 enterVelocity = movementCompoent.curMoveVelocity_World;

        while (time < velocityChangeDuration)
        {
            time += Time.deltaTime;
            movementCompoent.curMoveVelocity_World = Vector3.Lerp(enterVelocity, v, time / velocityChangeDuration);

            yield return null;
        }
    }

    protected IEnumerator VelotityChangeMaintainMomentum(float velocityChangeDuration, Vector3 v, MovementCompoent movementCompoent)
    {
        float time = 0;

        while (time < velocityChangeDuration)
        {
            time += Time.deltaTime;
            movementCompoent.curMoveVelocity_World += Vector3.Lerp(Vector3.zero, v, time / velocityChangeDuration);

            yield return null;
        }
    }
}


