using UnityEngine;

public interface IDodgeAble 
{

    public float _dodgeImpluseForce { get; }

    public float _dodgeInAirStopForce { get; }

    public float _dodgeOnGroundStopForce { get; }
}
