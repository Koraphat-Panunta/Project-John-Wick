using System.Collections.Generic;
using UnityEngine;

public class RagdollMotionState : MotionState
{
    public List<Rigidbody> bones = new List<Rigidbody>();
    public GameObject hips;
    private Rigidbody hipsRigid;
    private Animator animator;
    public RagdollMotionState(List<GameObject> bones,GameObject hips,Animator animator) 
    {
        this.animator = animator;
        foreach (GameObject rbBone in bones) 
        {
            this.bones.Add(rbBone.GetComponent<Rigidbody>());
        }
        DisableRagdoll();
        this.hips = hips;
        this.hipsRigid = this.hips.GetComponent<Rigidbody>();
    }

    public override void Enter()
    {
        this.animator.applyRootMotion = false;
        this.animator.enabled = false;
        EnableRagdoll();
        base.Enter();
    }

    public override void Exit()
    {
        DisableRagdoll();

        base.Exit();
    }

    private void EnableRagdoll()
    {
        foreach (Rigidbody rb in bones)
        {
            rb.isKinematic = false;
        }
    }

    private void DisableRagdoll()
    {
        foreach (Rigidbody rb in bones)
        {
            rb.isKinematic = true;
        }
    }
   
}
