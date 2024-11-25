using UnityEngine;
using UnityEngine.Animations;

public class GripDefault_AR15 : Grip
{
    [SerializeField] private Transform Anchor;
    [SerializeField] private float RecoilCameraKickBackController;
    [SerializeField] private float AimDownSightSpeed;
    [SerializeField] private float MovementSpeed;
    [SerializeField] private float Accuracy;
    public override Transform anchor { get => Anchor; set => Anchor = value; }
    protected override float recoilCameraKickBackController { get => RecoilCameraKickBackController; set => RecoilCameraKickBackController = value; }
    protected override float aimDownSightSpeed { get => AimDownSightSpeed; set => AimDownSightSpeed = value; }
    protected override float movementSpeed { get => MovementSpeed; set => MovementSpeed = value; }
    protected override float accuracy { get => Accuracy; set => Accuracy = value; }

    public override void Attach(Weapon weapon)
    {
        base.Attach(weapon);
    }
}
