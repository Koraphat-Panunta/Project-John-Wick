using UnityEngine;

public partial class Player : IDodgeAble
{
    [Range(0, 100)]
    [SerializeField] private float dodgeImpluseForce;
    [Range(0, 100)]
    [SerializeField] private float dodgeInAirStopForce;
    [Range(0, 100)]
    [SerializeField] private float dodgeOnGroundStopForce;

    public float _dodgeImpluseForce => this.dodgeImpluseForce;

    public float _dodgeInAirStopForce => this.dodgeInAirStopForce;

    public float _dodgeOnGroundStopForce => this.dodgeOnGroundStopForce;
}
