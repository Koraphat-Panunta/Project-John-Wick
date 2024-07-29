using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RightHandController : MonoBehaviour
{
    TwoBoneIKConstraint RightHand;
    void Start()
    {
        RightHand = GetComponent<TwoBoneIKConstraint>();
    }

   
}
