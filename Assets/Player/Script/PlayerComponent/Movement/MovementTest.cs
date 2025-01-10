using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MovementTest",menuName = "MoveTest")]
public class MovementTest : ScriptableObject
{
    public float move_MaxSpeed = 2.8f;
    public float move_Acceleration = 0.4f;
    public float sprint_MaxSpeed = 5.6f;
    public float sprint_Acceleration = 0.6f;
}
