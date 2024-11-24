using UnityEngine;

public class DefaultAR_Muzzle : Muzzle, IAR_Muzzle
{
    [SerializeField] private Transform Anchor;
    [SerializeField] private float RecoilController;
    [SerializeField] private float AimDownSightSpeed;
    [SerializeField] private Vector2 Bulletspawner;
    public override Transform anchor { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    protected override float recoilController { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    protected override float aimDownSightSpeed { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    protected override Vector3 bulletSpawnerPos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

}
