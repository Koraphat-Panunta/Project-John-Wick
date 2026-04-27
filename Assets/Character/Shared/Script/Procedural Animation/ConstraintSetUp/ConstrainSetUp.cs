using UnityEditor;
using UnityEngine;

public class ConstrainSetUp : MonoBehaviour
{
    public MonoBehaviour[] constrainManager;
    public HumanoidBone humanoidBone;



    public void SetUpConstrainBone()
    {
        if(this.constrainManager == null
            || this.constrainManager.Length <= 0)
            return;

        for(int i = 0; i < this.constrainManager.Length; i++)
        {
            if (this.constrainManager[i] is IConstraintManager constraint)
            {
                constraint.AssignBone(this.humanoidBone);
            }
            else
                continue;
        }
    }
}


