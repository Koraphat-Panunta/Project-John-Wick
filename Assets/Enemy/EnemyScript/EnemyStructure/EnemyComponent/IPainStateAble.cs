using UnityEngine;

public interface IPainStateAble 
{
    public bool _isPainTrigger { get; set; }
    public bool _isInPain { get; set; }
    public float _posture { get; set; }
    public float _postureLight { get; set; }
    public float _postureMedium { get; set; }
    public float _postureHeavy { get; set; }
    public enum PainPart
    {

        None,
        BodyFornt,
        BodyBack,
        ArmLeft,
        ArmRight,
        LegLeft, 
        LegRight,
        Head
    }
    public PainStateDurationScriptableObject _painDurScrp { get; }
    public PainPart _painPart { get; set; }
    public void BlackBoardBufferUpdate();
    public void InitializedPainState();

}
