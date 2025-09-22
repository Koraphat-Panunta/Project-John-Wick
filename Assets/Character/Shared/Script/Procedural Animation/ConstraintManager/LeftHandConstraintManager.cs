using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeftHandConstraintManager : MonoBehaviour,IConstraintManager
{
    [SerializeField] private TwoBoneIKConstraint twoBoneIKConstraint;
    [SerializeField] private Transform referenceTransform;

    [SerializeField] private Transform leftHandHint;
    [SerializeField] private Transform leftHandTarget;

    [SerializeField] public Vector3 hintOffset;

    [SerializeField] public Vector3 targetHand;
    [SerializeField] public Quaternion targetHandRotation;

    public float GetWeight() => twoBoneIKConstraint.weight;
    public void SetWeight(float w) => twoBoneIKConstraint.weight = w;   


    // Update is called once per frame
    void Update()
    {
        if(twoBoneIKConstraint.weight <= 0)
            return;

        leftHandHint.position = referenceTransform.position 
            + (referenceTransform.forward * hintOffset.z)
            + (referenceTransform.right * hintOffset.x)
            + (referenceTransform.up * hintOffset.y);

        this.leftHandTarget.position = targetHand;
        this.leftHandTarget.rotation = targetHandRotation;
    }
}
