using UnityEngine;
using System.Collections.Generic;

public interface IMotionDriven 
{
    public List<GameObject> bones { get; set; }
    public GameObject hips { get; set; }
    public Animator animator { get; set; }
    public MotionControlManager motionControlManager { get; set; }
    public void MotionControlInitailized();
   
}
