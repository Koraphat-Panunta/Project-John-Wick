using UnityEngine;

public interface IPainState 
{
    public bool _isPainTrigger { get; set; }
    public bool _isInPain { get; set; }
    public float _posture { get; set; }
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
    public PainPart _painPart { get; set; }
    public void BlackBoardBufferUpdate();
    public void InitializedPainState();

}
