using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] GameObject head;
    [SerializeField] GameObject spline;
    [SerializeField] GameObject hip;
    [SerializeField] GameObject right_upperLeg;
    [SerializeField] GameObject right_lowerLeg;
    //[SerializeField] GameObject right_foot;
    [SerializeField] GameObject left_upperLeg;
    [SerializeField] GameObject left_lowerLeg;
    //[SerializeField] GameObject left_foot;
    [SerializeField] GameObject right_upperArm;
    [SerializeField] GameObject right_lowerArm;
    [SerializeField] GameObject left_upperArm;
    [SerializeField] GameObject left_lowerArm;

    [SerializeField] Transform rootPos;
    private Rigidbody[] ragdoll = new Rigidbody[11];

    private float deltaRoot_Hips;
    public bool enableRagdoll;

    private Vector3 rootPosRagdoll = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        deltaRoot_Hips = hip.transform.position.y - rootPos.transform.position.y;
        AddRigidbody();
        
        foreach (Rigidbody rb in ragdoll)
        {
            rb.isKinematic = true;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enableRagdoll == true)
        {
            rootPosRagdoll = hip.transform.position;
        }
    }
    //private void OnValidate()
    //{
    //    if(enableRagdoll == true)
    //    {
    //        animator.enabled = false;
    //        foreach (Rigidbody rb in ragdoll)
    //        {
    //            rb.isKinematic = false;
    //        }
    //    }
    //    else
    //    {
    //        rootPos.position = rootPosRagdoll;
    //        animator.enabled=true;
    //        foreach (Rigidbody rb in ragdoll)
    //        {
    //            rb.isKinematic = true;
    //        }
    //    }
    //}
    public void EnableRagdoll()
    {
       
            animator.enabled = false;
            foreach (Rigidbody rb in ragdoll)
            {
                rb.isKinematic = false;
            }
        
    }
    private void AddRigidbody()
    {
        ragdoll[0] = head.GetComponent<Rigidbody>();
        ragdoll[1] = spline.GetComponent<Rigidbody>();
        ragdoll[2] = hip.GetComponent<Rigidbody>();
        ragdoll[3] = right_upperLeg.GetComponent<Rigidbody>();
        ragdoll[4] = right_lowerLeg.GetComponent<Rigidbody>();
        //ragdoll[5] = right_foot.GetComponent<Rigidbody>();
        ragdoll[5] = left_upperLeg.GetComponent<Rigidbody>();
        ragdoll[6] = left_lowerLeg.GetComponent<Rigidbody>();
        //ragdoll[8] = left_foot.GetComponent<Rigidbody>();
        ragdoll[7] = right_upperArm.GetComponent<Rigidbody>();
        ragdoll[8] = right_lowerArm.GetComponent<Rigidbody>();
        ragdoll[9] = left_upperArm.GetComponent<Rigidbody>();
        ragdoll[10] = left_lowerArm.GetComponent<Rigidbody>();
    }
}
