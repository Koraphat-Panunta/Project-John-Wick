using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Splines;

public class HumandShieldRightHandConstrainLookAtManager : MonoBehaviour
{
    [Range(0, 1)] 
    [SerializeField] private float weight;

    [SerializeField] private HumanShieldRightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject;

    [SerializeField] private MultiAimConstraint rightArm_MultiAimConstraint;
    [SerializeField] private MultiAimConstraint rightForeArm_MultiAimConstraint;
    [SerializeField] private MultiAimConstraint rightHand_MultiAimConstraint;

    private Vector3 rightArm_Offset;
    private Vector3 rightForeArm_Offset;
    private Vector3 rightHand_Offset;

    // Update is called once per frame
    public void SetWeight(float w, HumanShieldRightHandConstrainLookAtScriptableObject humanShieldRightHandConstrainLookAtScriptableObject)
    {
        SetWeight(w);
        this.humanShieldRightHandConstrainLookAtScriptableObject = humanShieldRightHandConstrainLookAtScriptableObject;
    }
    public void SetWeight(float w)
    {
        this.weight = w;
    }
    public float GetWeight() => weight;
    void Update()
    {
        if(weight <= 0)
            return;

        rightArm_MultiAimConstraint.data.offset = Vector3.MoveTowards(rightArm_Offset, humanShieldRightHandConstrainLookAtScriptableObject.offset_RightArmAim, humanShieldRightHandConstrainLookAtScriptableObject.offsetChangedRate * Time.deltaTime);
        rightForeArm_MultiAimConstraint.data.offset = Vector3.MoveTowards(rightForeArm_Offset, humanShieldRightHandConstrainLookAtScriptableObject.offset_RightForeArmAim, humanShieldRightHandConstrainLookAtScriptableObject.offsetChangedRate * Time.deltaTime);
        rightHand_MultiAimConstraint.data.offset = Vector3.MoveTowards(rightHand_Offset, humanShieldRightHandConstrainLookAtScriptableObject.offset_RightHandAim, humanShieldRightHandConstrainLookAtScriptableObject.offsetChangedRate * Time.deltaTime);

        rightArm_Offset = rightForeArm_MultiAimConstraint.data.offset;
        rightForeArm_Offset = rightForeArm_MultiAimConstraint.data.offset;
        rightHand_Offset = rightHand_MultiAimConstraint.data.offset;

        weight = Mathf.Clamp(weight, 0f, 1f);

        rightArm_MultiAimConstraint.weight = humanShieldRightHandConstrainLookAtScriptableObject.weightRightArmAim * this.weight;
        rightForeArm_MultiAimConstraint.weight = humanShieldRightHandConstrainLookAtScriptableObject.weightRightForeArmAim * this.weight;
        rightHand_MultiAimConstraint.weight = humanShieldRightHandConstrainLookAtScriptableObject.weightRightHandAim * this.weight;

       
    }
}
