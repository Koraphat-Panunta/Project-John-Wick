using System.Collections.Generic;
using UnityEngine;

public class MotionControlManager 
{
    public RagdollMotionState ragdollMotionState;
    public AnimationDrivenMotionState animationDrivenMotionState;
    public MotionState curMotionState;
     public MotionControlManager(List<GameObject> bones,GameObject hips,Animator animator) 
    {
        ragdollMotionState = new RagdollMotionState(bones,hips);
        animationDrivenMotionState = new AnimationDrivenMotionState(animator);
        ChangeMotionState(animationDrivenMotionState);
    }
    public void ChangeMotionState(MotionState nextMotionState)
    {
        if(curMotionState != null)
            curMotionState.Exit();

        if (curMotionState == ragdollMotionState
            && nextMotionState == animationDrivenMotionState) {
            return;
        }

        curMotionState = nextMotionState;
        curMotionState.Enter();
    }
}
