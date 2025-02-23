using UnityEngine;
using static IMotionImplusePushAble;

public interface IMotionImplusePushAble 
{
    public IMovementCompoent movementCompoent { get; }
    public MotionImplusePushAbleBehavior motionImplusePushAbleBehavior { get; set; }
    public enum PushMode 
    {
        InstanlyIgnoreMomentum,
        InstanlyMaintainMomentum,
    }
   public void AddForcePush(Vector3 force,PushMode pushMode);
}
public class MotionImplusePushAbleBehavior
{
   
    public void AddForecPush(IMotionImplusePushAble motionImplusePushAble,Vector3 force, PushMode pushMode)
    {
        IMovementCompoent movementCompoent = motionImplusePushAble.movementCompoent;
        switch (pushMode)
        {
            case PushMode.InstanlyIgnoreMomentum:
                {
                    movementCompoent.CancleMomentum();
                    movementCompoent.curMoveVelocity_World = force;
                }
                break;
            case PushMode.InstanlyMaintainMomentum:
                {
                    movementCompoent.curMoveVelocity_World = force;
                }
                break;
        }
    }
}
