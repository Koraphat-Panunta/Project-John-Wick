
using UnityEngine;


public class EnemyGetShootDirection 
{
    private Enemy enemy;
    private Weapon weapon;
    public float trackingTargetRate { get; protected set; }
    public float trackingTargetAccelerate = .05f;
    public float trackingTargetDecelerate = 1;


    public EnemyGetShootDirection(Enemy enemy)
    {
        this.enemy = enemy;
    }
    public Vector3 GetShootingPos()
    {
        float accuracy = Random.Range(0, 0.1f);
        Vector3 dirTarget;

        if(enemy._currentWeapon == null)
            dirTarget = (this.GetPointingPos() - (enemy.rayCastPos.position + new Vector3(0, 0.2f, 0))).normalized;
        else
            dirTarget = (this.GetPointingPos() - (enemy._currentWeapon.bulletSpawnerPos.position)).normalized;

        dirTarget = new Vector3(dirTarget.x + Random.Range(-accuracy, accuracy), dirTarget.y + Random.Range(-accuracy, accuracy), dirTarget.z);
        Ray ray = new Ray(enemy._currentWeapon.bulletSpawnerPos.position, dirTarget);

        return ray.GetPoint(100);
        //return new Vector3(dir.x + Random.Range(-accuracy, accuracy), dir.y + Random.Range(-accuracy, accuracy), dir.z);
        
    }
    private Vector3 pointingPos; 
    private Vector3 forwardDir => enemy.transform.forward; 
    private float maxHorizontalRotateDegrees = 30;
    private float maxVerticalRotateDegrees = 60;
    public bool outOfHorizontalLimit { get; private set; }
    public void SetPointingPos(Vector3 poitnPos)
    {

        // Normalize input
        Vector3 dirToPoint = (poitnPos - enemy.transform.position).normalized;

        // Basis: forward, right, up
        Vector3 fwd = forwardDir.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, fwd).normalized;
        Vector3 up = Vector3.Cross(fwd, right).normalized;

        // Project onto local basis (dot products give angles)
        float horizontalAngle = Mathf.Atan2(Vector3.Dot(dirToPoint, right), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg;
        float verticalAngle = (Mathf.Atan2(Vector3.Dot(dirToPoint, up), Vector3.Dot(dirToPoint, fwd)) * Mathf.Rad2Deg)*-1;

         outOfHorizontalLimit = Mathf.Abs(horizontalAngle) > maxHorizontalRotateDegrees;

        // Clamp angles
        horizontalAngle = Mathf.Clamp(horizontalAngle, -maxHorizontalRotateDegrees, maxHorizontalRotateDegrees);
        verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalRotateDegrees, maxVerticalRotateDegrees);

        // Rebuild direction from clamped angles
        Quaternion rot = Quaternion.AngleAxis(horizontalAngle, Vector3.up) *
                         Quaternion.AngleAxis(verticalAngle, right);
        Vector3 clampedDir = rot * fwd;

        // Final pointing position (you can scale as needed)
        pointingPos = Vector3.Lerp(pointingPos,enemy.transform.position + (clampedDir.normalized * Mathf.Clamp((poitnPos - enemy.transform.position).magnitude,1, 10)),this.trackingTargetRate);
        enemy.pointingTransform.position = pointingPos;
        Debug.DrawLine(enemy.transform.position, pointingPos, Color.blue);
    }
    public Vector3 GetPointingPos()
    { 
        return pointingPos;
    }
    public void SetTrackingRate(float trackingRate)
    {
        this.trackingTargetRate = Mathf.Clamp01(trackingRate);
    }
   

}
