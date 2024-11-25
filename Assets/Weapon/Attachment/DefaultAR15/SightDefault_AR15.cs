using UnityEngine;

public class SightDefault_AR15 : Sight
{
    [SerializeField] private Transform Anchor;
    [SerializeField] private float MinPrecision;
    [SerializeField] private float MaxPrecision;
    [SerializeField] private float Accuracy;
    [SerializeField] private float AimDownSightSpeed;
    public override Transform anchor { get => Anchor; set => Anchor = value; }
    protected override float min_Precision { get => MinPrecision; set => MinPrecision = value; }
    protected override float max_Precision { get => MaxPrecision; set => MaxPrecision = value; }
    protected override float accuracy { get => Accuracy; set => Accuracy = value; }
    protected override float aimDownSightSpeed { get => AimDownSightSpeed; set => AimDownSightSpeed = value;}

}
