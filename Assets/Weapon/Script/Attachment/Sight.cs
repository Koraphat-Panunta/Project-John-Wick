using UnityEngine;
using UnityEngine.Animations;

public class Sight : WeaponAttachment
{
    [SerializeField] private Transform Anchor;
    public override Transform anchor { get => Anchor ; set { return; } }
    [SerializeField] protected float min_Precision;
    [SerializeField] protected float max_Precision;
    [SerializeField] protected float accuracy;
    [SerializeField] protected float aimDownSightSpeed;
    public override AttachmentSlot myAttachmentSlot { get;protected set ; }
    public override void Attach(Weapon weapon)
    {
        myAttachmentSlot = AttachmentSlot.SCOPE;
        weapon.min_Precision += this.min_Precision;
        weapon.max_Precision += this.max_Precision;
        weapon.Accuracy += this.accuracy;
        weapon.aimDownSight_speed += this.aimDownSightSpeed;

        base.Attach(weapon);
    }
    protected override void Update()
    {
        base.Update();
    }

}

   
