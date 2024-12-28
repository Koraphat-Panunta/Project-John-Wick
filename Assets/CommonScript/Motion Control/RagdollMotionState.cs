using System.Collections.Generic;
using UnityEngine;

public class RagdollMotionState : MotionState
{
    List<Rigidbody> bones = new List<Rigidbody>();
    GameObject hips;
    public RagdollMotionState(List<GameObject> bones,GameObject hips) 
    {
        foreach (GameObject rbBone in bones) 
        {
            this.bones.Add(rbBone.GetComponent<Rigidbody>());
        }
        DisableRagdoll();
        this.hips = hips;
    }

    public override void Enter()
    {
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
