using UnityEngine;

public class BalancePointComponent 
{
    public Transform root { get; protected set; }
    protected Vector3 rootOffset;
    public Vector3 centerPosition 
        => root.position
        + (root.forward * rootOffset.z)
        + (root.up * rootOffset.y)
        + (root.right * rootOffset.x);

    public Vector3 balancePointLookAt { get; protected set; }

    private Vector3 f_calculateBalancePoint;

    private Vector3 max_Distance_BalancePoint;

    private Vector3 frequency_BalancePoint;

    public BalancePointComponent(
        Transform root
        ,Vector3 rootOffset
        ,Vector3 max_Distance_BalancePoint
        ,Vector3 frequency_BalancePoint) 
    {
        this.root = root;
        this.rootOffset = rootOffset;
        this.max_Distance_BalancePoint = max_Distance_BalancePoint;
        this.frequency_BalancePoint = frequency_BalancePoint;
    }

    public void SetUpdatePropertiesBalancePoint
        (
        Vector3 rootOffset
        ) 
    {
        this.rootOffset = rootOffset;
    }
    public virtual void UpdateBalancePoint()
    {

        // SinWave
        if (f_calculateBalancePoint.y <= 2)
            this.f_calculateBalancePoint.y += (Time.deltaTime * frequency_BalancePoint.y);
        else
            this.f_calculateBalancePoint.y = 0;

        if (f_calculateBalancePoint.x <= 2)
            this.f_calculateBalancePoint.x += (Time.deltaTime * frequency_BalancePoint.x);
        else
            this.f_calculateBalancePoint.x = 0;

        if(this.f_calculateBalancePoint.z <= 0)
            this.f_calculateBalancePoint.z += (Time.deltaTime * frequency_BalancePoint.z);
        else
            this.f_calculateBalancePoint.z = 0;
        //
        
        // Calculate balance point
        Vector3 balancePointDistance = Vector3.zero;

        balancePointDistance.y = Mathf.Sin(this.f_calculateBalancePoint.y * Mathf.PI);
        balancePointDistance.x = Mathf.Sin(this.f_calculateBalancePoint.x * Mathf.PI);
        balancePointDistance.z = Mathf.Sin(this.f_calculateBalancePoint.z * Mathf.PI);

       
        this.balancePointLookAt = this.centerPosition
            + (root.up * balancePointDistance.y * this.max_Distance_BalancePoint.y)
            + (root.right * balancePointDistance.x * this.max_Distance_BalancePoint.x)
            + (root.forward * balancePointDistance.z * this.max_Distance_BalancePoint.z);
        
        Debug.DrawLine(this.centerPosition + (this.root.up * this.max_Distance_BalancePoint.y)
            , this.centerPosition + (this.root.up * this.max_Distance_BalancePoint.y * -1)
            , Color.green);

        Debug.DrawLine(this.centerPosition + (this.root.right * this.max_Distance_BalancePoint.x)
           , this.centerPosition + (this.root.right * this.max_Distance_BalancePoint.x * -1)
           , Color.red);

        Debug.DrawLine(this.centerPosition + (this.root.forward * this.max_Distance_BalancePoint.z)
           , this.centerPosition + (this.root.forward * this.max_Distance_BalancePoint.z * -1)
           , Color.blue);

        Debug.DrawLine(this.centerPosition, this.balancePointLookAt, Color.white);
    }
}
