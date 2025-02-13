using UnityEngine;

public interface ISphereCastDetectTarget<Target>
{
    public Transform _castTransform { get; set; }
    public float _limitAimAngleDegrees { get; set; }
    public float _shpere_Raduis_Detecion { get; set; }
    public float _sphere_Distance_Detection { get; set; }
    public bool CastDetect(out Target target,Vector3 casrDir);
}
