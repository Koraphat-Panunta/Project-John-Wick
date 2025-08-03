using UnityEngine;

public class ZoneDefine 
{
    public Vector3 zonePosition { get; private set; }
    public float raduis { get; private set; }
    public ZoneDefine(Vector3 pos,float raduis)
    {
        this.zonePosition = pos;
        this.raduis = raduis;
    }
   public void SetZone(Vector3 position)
   {
        this.zonePosition = position;
   }
    public void SetZone(Vector3 position,float r)
    {
        this.raduis = r;
        SetZone(position);
    }
    public Vector3 GetRandomPositionInZone()
    {
        float angle = Random.Range(0f, 2f * Mathf.PI); // Random angle in radians
        float distance = Random.Range(0f, this.raduis); // Random distance within distance

        float xOffset = Mathf.Cos(angle) * distance;
        float zOffset = Mathf.Sin(angle) * distance;

        return new Vector3(zonePosition.x + xOffset, zonePosition.y, zonePosition.z + zOffset);
    }
    public Vector3 GetRandomPositionInZoneForwardZoneToPosition(Vector3 pos,float angle)
    {
        float halfAngle = angle / 2f; // Half of the given angle for the forward-facing area
        float randomAngleOffset = Random.Range(-halfAngle, halfAngle); // Random angle within the sector

        Vector3 forwardDir = (pos - zonePosition).normalized; // Direction from pos to the zone center
        forwardDir = Quaternion.Euler(0, randomAngleOffset, 0) * forwardDir; // Rotate direction within the given angle

        float distance = Random.Range(0f, this.raduis); // Random distance within the distance

        Vector3 randomPos = zonePosition + forwardDir * distance; // Move along the rotated direction
        randomPos.y = zonePosition.y; // Keep Y position the same

        return randomPos;
    }
    public bool IsPositionInTheZone(Vector3 pos)
    {
        if(Vector3.Distance(pos,this.zonePosition) > raduis)
            return false;
        return true;
    }
}
