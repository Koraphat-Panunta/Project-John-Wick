using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ProceduralBoneController : MonoBehaviour
{
    [SerializeField] MultiAimConstraint Spine;
    [SerializeField] MultiAimConstraint Head;
    [SerializeField] MultiAimConstraint RightArm;
    [SerializeField] MultiAimConstraint RightHand;
    // Start is called before the first frame update
    void Start()
    {
        Spine.weight = 0;
        Head.weight = 0;
        RightArm.weight = 0;
        RightHand.weight = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
