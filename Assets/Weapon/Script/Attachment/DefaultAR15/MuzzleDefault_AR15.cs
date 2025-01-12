using UnityEngine;
using UnityEngine.Animations;

public class MuzzleDefault_AR15 : Muzzle
{
    [SerializeField] private float RecoilController;
    [SerializeField] private float AimDownSightSpeed;
    [SerializeField] private Transform Anchor;
    [SerializeField] private Transform BulletSpawnerPos;
    public override Transform anchor { 
        get => Anchor;
        set => Anchor = value; 
    }

    protected override float recoilController { 
        get => RecoilController; 
        set => RecoilController = value; }
    protected override float aimDownSightSpeed { 
        get => AimDownSightSpeed;
        set => AimDownSightSpeed = value; }
    protected override Transform bulletSpawnPos { 
        get => BulletSpawnerPos;
        set => BulletSpawnerPos = value;}

}
