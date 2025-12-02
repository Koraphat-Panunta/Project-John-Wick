using UnityEngine;

public class ArmRightBodyPart : BodyPart
{
    public float _posture { get; set; }
    public float _maxPosture { get; set; }
    public float postureRecoverySpeed = 6;

    public override void TakeDamage(IDamageVisitor damageVisitor)
    {
        base.TakeDamage(damageVisitor);
    }

    public override void TakeDamageBullet(IDamageVisitor damageVisitor, Vector3 hitPart, Vector3 hitDir, float hitforce)
    {
        this.TakeDamage(damageVisitor);
        base.TakeDamageBullet(damageVisitor, hitPart, hitDir, hitforce);
    }

    private void FixedUpdate()
    {
        if (this._posture < this._maxPosture)
        {
            this._posture = Mathf.Clamp(this._posture + (Time.fixedDeltaTime * this.postureRecoverySpeed), 0, this._maxPosture);
        }
    }
}
