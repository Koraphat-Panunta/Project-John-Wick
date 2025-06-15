using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public static class WeaponAnimatorOverrider 
{
    public static void OverrideAnimator(Animator animator,AnimatorOverrideController animatorOverrideController)
    {
        //AnimatorStateInfo[] animatorStates = new AnimatorStateInfo[animator.layerCount];
        //float[] animatorStateNormalized = new float[animator.layerCount];

        //for (int i = 0; i < animatorStates.Length; i++)
        //{
        //    animatorStates[i] = animator.GetCurrentAnimatorStateInfo(i);
        //    animatorStateNormalized[i] = animator.GetCurrentAnimatorStateInfo(i).normalizedTime;
        //}

        AnimatorStateInfo animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        float animatorStateNormalizedTime = animatorStateInfo.normalizedTime;

        animator.runtimeAnimatorController = animatorOverrideController;

        animator.Play(animatorStateInfo.fullPathHash, 0, animatorStateNormalizedTime);
        //for (int i = 0; i < animatorStates.Length; i++)
        //{
        //    animator.Play(animatorStates[i].fullPathHash, i, animatorStateNormalized[i]);
        //}
    }
    //public static void OverrideAnimator(Animator animator,RigBuilder rigBuilder, AnimatorOverrideController animatorOverrideController)
    //{
    //    OverrideAnimator(animator, animatorOverrideController);
    //    rigBuilder.Build();
    //}
}
