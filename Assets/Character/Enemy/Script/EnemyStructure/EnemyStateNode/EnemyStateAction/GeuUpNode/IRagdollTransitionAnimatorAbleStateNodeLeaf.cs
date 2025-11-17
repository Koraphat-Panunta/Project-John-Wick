using UnityEngine;

public interface IRagdollTransitionAnimatorAbleStateNodeLeaf 
{
    public enum RagdollTransitionAnimatorState
    {
        ResetingBone,
        PlayAnimation
    }
    public RagdollTransitionAnimatorState curRagdollAnimatorState { get; set; }
}
