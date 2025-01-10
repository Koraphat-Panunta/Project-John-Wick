using UnityEngine;

public class GunFuCommand 
{
    public float timeHeldDown { get; private set; }
    public GunFuCommand(float heldDownTime)
    {
        this.timeHeldDown = heldDownTime;
    }
}
