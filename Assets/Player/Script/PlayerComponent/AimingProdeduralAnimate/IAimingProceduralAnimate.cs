using UnityEngine;
using UnityEngine.Animations.Rigging;

public interface IAimingProceduralAnimate 
{
    public MultiAimConstraint _aimConstraint { get; set; }
    public MultiRotationConstraint _rotationConstraint { get; set; }
    public AimingProceduralAnimate _aimingProceduralAnimate { get; set; }
    public Transform _aimPosRef { get; set; }
    public LeanCover _leanCover { get; set; }
    public CrosshairController _crosshairController { get;}
    public void InitializedAimingProceduralAnimate();
}
