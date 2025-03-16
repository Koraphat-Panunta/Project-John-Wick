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
        float distance = Random.Range(0f, this.raduis); // Random distance within radius

        float xOffset = Mathf.Cos(angle) * distance;
        float zOffset = Mathf.Sin(angle) * distance;

        return new Vector3(zonePosition.x + xOffset, zonePosition.y, zonePosition.z + zOffset);
    }
}
