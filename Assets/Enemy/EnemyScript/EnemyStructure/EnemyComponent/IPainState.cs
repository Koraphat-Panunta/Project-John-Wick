using UnityEngine;

public interface IPainState 
{
    public bool _isPainTrigger { get; set; }
    public bool _isInPain { get; set; }
    public float _pressure { get; set; }
    public enum PainPart
    {

        None,
        BodyFornt,
        BodyBack,
        ArmLeft,
        ArmRight,
        LegLeft, 
        LegRight,

    }
    public PainPart _painPart { get; set; }
    public void BlackBoardBufferUpdate();
    public void InitializedPainState();

}
