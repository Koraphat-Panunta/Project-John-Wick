using UnityEngine;

public class FullBodyCharacterPart : MonoBehaviour, IInitializedAble
{
    [SerializeField] public HeadBodyPart headBodyPart;

    [SerializeField] public ChestBodyPart hipBodyPart;
    [SerializeField] public ChestBodyPart spline_0BodyPart;

    [SerializeField] public ArmLeftBodyPart armLeftBodyPart;
    [SerializeField] public ArmLeftBodyPart foreArmLeftBodyPart;

    [SerializeField] public ArmRightBodyPart armRightBodyPart;
    [SerializeField] public ArmRightBodyPart foreArmRightBodyPart;

    [SerializeField] public LegLeftBodyPart upperLegLeftBodyPart;
    [SerializeField] public LegLeftBodyPart lowerLegLeftBodyPart;

    [SerializeField] public LegRightBodyPart upperLegRightBodyPart;
    [SerializeField] public LegRightBodyPart lowerLegRightBodyPart;

    public void Initialized()
    {
        //Head
        this.headBodyPart.Initialized();

        //Chest
        this.hipBodyPart.Initialized();
        this.spline_0BodyPart.Initialized();

        //Legs
        this.upperLegRightBodyPart.Initialized();
        this.lowerLegRightBodyPart.Initialized();

        this.upperLegLeftBodyPart.Initialized();
        this.lowerLegLeftBodyPart.Initialized();

        //Arms
        this.armLeftBodyPart.Initialized();
        this.foreArmLeftBodyPart.Initialized();

        this.armRightBodyPart.Initialized();
        this.foreArmRightBodyPart.Initialized();

    }
}
