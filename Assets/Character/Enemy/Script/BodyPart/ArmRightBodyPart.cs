using UnityEngine;

public class ArmRightBodyPart : BodyPart
{
    public float _posture { get; set; }
    public float _maxPosture { get; set; }
    public float postureRecoverySpeed = 6;

 
    private void FixedUpdate()
    {
        if (this._posture < this._maxPosture)
        {
            this._posture = Mathf.Clamp(this._posture + (Time.fixedDeltaTime * this.postureRecoverySpeed), 0, this._maxPosture);
        }
    }
}
