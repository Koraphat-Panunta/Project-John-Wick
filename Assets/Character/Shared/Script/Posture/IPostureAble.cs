using UnityEngine;

public interface IPostureAble 
{
    public void TakePostureDamaged(float postureDamage);
    public float _posture { get; set; }
    public float _maxPosture { get; set; }

}
