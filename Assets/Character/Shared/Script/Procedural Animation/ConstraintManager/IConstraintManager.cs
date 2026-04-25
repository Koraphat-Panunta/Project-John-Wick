using UnityEngine;

public interface IConstraintManager
{
    public void AssignBone(HumanoidBone humanoidBone);
    public void SetWeight(float w);
    public float GetWeight();
}
