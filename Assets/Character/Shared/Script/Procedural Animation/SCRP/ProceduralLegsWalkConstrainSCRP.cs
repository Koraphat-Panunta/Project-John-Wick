using UnityEngine;

[CreateAssetMenu(fileName = "ProceduralLegsWalkConstrainSCRP", menuName = "ScriptableObjects/ConstrainObject/ProceduralLegsWalkConstrainSCRP")]
public class ProceduralLegsWalkConstrainSCRP : ScriptableObject
{
    [Range(0,1)]
    public float stepMaxHeight;
    [Range(0, 1)]
    public float stepMaxDistance;
    [Range(0, 10)]
    public float stepMaxVelocity;

    [Range(0, 2)]
    public float distanceBeginStep;

    [Range(0, 10)]
    public float stepVelocityFactor;
    [Range(0, 10)]
    public float stepDistanceFactor;
    [Range(0, 10)]
    public float stepHeightFactor;

}
