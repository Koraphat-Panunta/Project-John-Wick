using UnityEngine;

[RequireComponent(typeof(IGunFuAble))]
public class GunFuDetectTarget : MonoBehaviour, ISphereCastDetectTarget<IGunFuGotAttackedAble>
{
    [SerializeField] protected IGunFuAble gunFuAble;

    [SerializeField] private Transform CastTransform;
    public Transform _castTransform { get => this.CastTransform; set => this.CastTransform = value; }

    [Range(0, 360)]
    [SerializeField] private float LimitAimAngleDegrees;
    public float _limitAimAngleDegrees { get => this.LimitAimAngleDegrees; set => this.LimitAimAngleDegrees = value; }

    [Range(0, 10)]
    [SerializeField] private float Shpere_Raduis_Detecion;
    public float _shpere_Raduis_Detecion { get => this.Shpere_Raduis_Detecion; set => this.Shpere_Raduis_Detecion = value; }

    [Range(0, 10)]
    [SerializeField] private float Shpere_Distance_Detection;
    public float _sphere_Distance_Detection { get => this.Shpere_Distance_Detection; set => this.Shpere_Distance_Detection = value; }

    private void Start()
    {
        gunFuAble = GetComponent<IGunFuAble>();
        Debug.Log("gunFuAble = " + gunFuAble);
    }

    public bool CastDetect(out IGunFuGotAttackedAble target)
    {
        target = null;
        Vector3 casrDir = CastSphere();

        if (CastDetect(out IGunFuGotAttackedAble gunFuTarget, casrDir))
        {
            target = gunFuTarget;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool CastDetect(out IGunFuGotAttackedAble target, Vector3 castDir)
    {
        target = null;

        Collider[] colliders = Physics.OverlapSphere(CastTransform.position, _shpere_Raduis_Detecion, gunFuAble._layerTarget);
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<IGunFuGotAttackedAble>(out IGunFuGotAttackedAble gunFuDamagedAble))
            {
                target = gunFuDamagedAble;
                return true; // Found a target already inside the detection sphere
            }
        }

        if (Physics.SphereCast(CastTransform.position, _shpere_Raduis_Detecion, castDir, out RaycastHit hitInfo, _sphere_Distance_Detection, gunFuAble._layerTarget))
        {
            if (hitInfo.collider.TryGetComponent<IGunFuGotAttackedAble>(out IGunFuGotAttackedAble gunFuDamagedAble))
            {
                target = gunFuDamagedAble;
                return true;
            }
            return false;
        }
        return false;
    }
    private Vector3 CastSphere()
    {
        Vector3 casrDir;

        if (Vector3.Angle(gunFuAble._gunFuUserTransform.forward, gunFuAble._gunFuAimDir) <= _limitAimAngleDegrees)
        {
            casrDir = new Vector3(gunFuAble._gunFuAimDir.x, 0, gunFuAble._gunFuAimDir.z);
        }
        else
        {
            if (Vector3.Dot(gunFuAble._gunFuUserTransform.right, gunFuAble._gunFuAimDir) < 0)
            {
                casrDir = Quaternion.Euler(0,-LimitAimAngleDegrees,0) * gunFuAble._gunFuUserTransform.forward;
            }
            else
                casrDir = Quaternion.Euler(0, LimitAimAngleDegrees, 0) * gunFuAble._gunFuUserTransform.forward;

        }

        return casrDir;
    }

    private bool CastDetect(out IGunFuGotAttackedAble target, out Vector3 hit)
    {
        target = null;
        Vector3 casrDir;

        if (Vector3.Angle(gunFuAble._gunFuUserTransform.forward, gunFuAble._gunFuAimDir) <= _limitAimAngleDegrees)
        {
            casrDir = new Vector3(gunFuAble._gunFuAimDir.x, 0, gunFuAble._gunFuAimDir.z);
        }
        else
        {
            casrDir = gunFuAble._gunFuUserTransform.forward;
        }

        if (CastDetect(out IGunFuGotAttackedAble gunFuTarget, out hit, casrDir))
        {
            target = gunFuTarget;
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CastDetect(out IGunFuGotAttackedAble target, out Vector3 hit, Vector3 castDir)
    {
        target = null;
        hit = Vector3.zero;

        if (Physics.SphereCast(CastTransform.position, _shpere_Raduis_Detecion, castDir, out RaycastHit hitInfo, _sphere_Distance_Detection, gunFuAble._layerTarget))
        {
            hit = hitInfo.point;
            if (hitInfo.collider.TryGetComponent<IGunFuGotAttackedAble>(out IGunFuGotAttackedAble gunFuDamagedAble))
            {
                target = gunFuDamagedAble;
                return true;
            }
            return false;
        }
        return false;
    }


    [SerializeField] private bool EnableDebug;
    private void OnDrawGizmos()
    {
       
        if (EnableDebug == false)
            return;

        Gizmos.color = Color.red;
        if (CastDetect(out IGunFuGotAttackedAble target))
        {
            Gizmos.DrawSphere(target._gunFuHitedAble.position, 0.75f);
        }

        Gizmos.color = Color.blue;
        Vector3 sphrerPos = this.CastTransform.position + (CastSphere()*this._sphere_Distance_Detection);
        Gizmos.DrawWireSphere(sphrerPos,  this.Shpere_Raduis_Detecion);

    }
}
