using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(IGunFuAble))]
public class GunFuDetectTarget : MonoBehaviour
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

    [SerializeField, TextArea]
    private string gunFuDetectTargetDebug;
    private void Start()
    {
        gunFuAble = GetComponent<IGunFuAble>();
        gunFuDetectTargetDebug += "gunFuAble = "+gunFuAble+"\n";
    }
    public bool CastDetectExecuteAbleTarget(out IGunFuGotAttackedAble gunFuGotExecuteAble)
    {
        gunFuGotExecuteAble = null;
        Vector3 castDir = CastDir();
        Ray ray = new Ray(_castTransform.position, castDir);
        RaycastHit[] collider = Physics.SphereCastAll(ray, _shpere_Raduis_Detecion, _sphere_Distance_Detection, 0 + gunFuAble._layerTarget);
        foreach (RaycastHit hit in collider)
        {
            if (hit.collider.gameObject.TryGetComponent<IGunFuGotAttackedAble>(out IGunFuGotAttackedAble gunFuGotAttackedAble) == false)
                continue;

            if (gunFuGotAttackedAble == gunFuAble)
            {
                gunFuDetectTargetDebug += "cast to self \n";
                continue;
            }

            if (gunFuGotAttackedAble._isDead)
                continue;

            if (gunFuGotAttackedAble._isGotExecutedAble == false)
                continue;


            Ray ray1 = new Ray(_castTransform.position, (hit.collider.gameObject.transform.position - _castTransform.position).normalized);
            if (Physics.Raycast(ray1, out RaycastHit hitInfo, 100, 0 + gunFuAble._layerTarget))
            {
                if (hitInfo.collider.gameObject.GetInstanceID() == hit.collider.gameObject.GetInstanceID())
                {
                    gunFuGotExecuteAble = gunFuGotAttackedAble;
                    return true;
                }
            }
        }
        return false;

    } // Called form gunFuAble
    public bool CastDetect(out IGunFuGotAttackedAble target)
    {
        target = null;
        Vector3 casrDir = CastDir();

        if (CastDetect(out IGunFuGotAttackedAble gunFuTarget, casrDir))
        {
            target = gunFuTarget;
            return true;
        }
        else
        {
            return false;
        }
    } // Called form player
    private Vector3 curPositionVolume;
    private float curRaduis;
    public bool CastDetectTargetInVolume(out List<IGunFuGotAttackedAble> target,Vector3 positionVolume,float raduis)
    {

        target = new List<IGunFuGotAttackedAble>();
        Collider[] colliders = Physics.OverlapSphere(positionVolume, raduis, gunFuAble._layerTarget.value);

        gunFuDetectTargetDebug += "layerTarget = " + gunFuAble._layerTarget.value + "\n";

        curPositionVolume = positionVolume;
        curRaduis = raduis;

        foreach (Collider item in colliders)
        {
            gunFuDetectTargetDebug += "in collider = " + item +"0 \n";

            if (item.TryGetComponent<IGunFuGotAttackedAble>(out IGunFuGotAttackedAble gunFuGotAttackedAble) == false)
                continue;

            gunFuDetectTargetDebug += "in collider = " + item + "1 \n";

            if (gunFuGotAttackedAble._isDead
                || gunFuGotAttackedAble._isGotAttackedAble == false
                || gunFuGotAttackedAble == gunFuAble)
                continue;

            gunFuDetectTargetDebug += "in collider = " + item + "2 \n";

            target.Add(gunFuGotAttackedAble);
        }

        if(target.Count >0)
            return true;
        return false;
    }// Called form gunFuAble
    private bool CastDetect(out IGunFuGotAttackedAble target, Vector3 castDir)
    {
        target = null;
        Ray ray = new Ray(_castTransform.position,castDir);
        RaycastHit[] collider = Physics.SphereCastAll(ray, _shpere_Raduis_Detecion, _sphere_Distance_Detection, 0 + gunFuAble._layerTarget);
        foreach(RaycastHit hit in collider)
        {
            if(hit.collider.gameObject.TryGetComponent<IGunFuGotAttackedAble>(out IGunFuGotAttackedAble gunFuGotAttackedAble) == false)
                continue;

            if(gunFuGotAttackedAble._isDead 
                || gunFuGotAttackedAble._isGotAttackedAble == false
                || gunFuGotAttackedAble == gunFuAble)
                continue ;

            //if(gunFuGotAttackedAble.curNodeLeaf is FallDown_EnemyState_NodeLeaf
            //    || gunFuGotAttackedAble.curNodeLeaf is HumandThrow_GotInteract_NodeLeaf
            //    || gunFuGotAttackedAble.curNodeLeaf is GotKnockDown_GunFuGotHitNodeLeaf)
            //    continue ;


            Ray ray1 = new Ray(_castTransform.position, (hit.collider.gameObject.transform.position - _castTransform.position).normalized);
            if (Physics.Raycast(ray1,out RaycastHit hitInfo,100, 0 + gunFuAble._layerTarget))
            {
                if(hitInfo.collider.gameObject.GetInstanceID() == hit.collider.gameObject.GetInstanceID())
                {
                    target = gunFuGotAttackedAble; 
                    return true;
                }
            }
        }
        return false;

        
    } 
    private Vector3 CastDir()
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
    private void OnValidate()
    {
        gunFuAble = GetComponent<IGunFuAble>();
    }

    [SerializeField] private bool EnableDebug;
    private void OnDrawGizmos()
    {
       
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(curPositionVolume, curRaduis);

        if (EnableDebug == false)
            return;

        Gizmos.color = Color.red;
        Vector3 sphrerPos = this.CastTransform.position + (CastDir() * this._sphere_Distance_Detection);
        if (CastDetect(out IGunFuGotAttackedAble target))
        {
            Gizmos.DrawSphere(target._gunFuAttackedAble.position, 0.75f);
            sphrerPos = this.CastTransform.position + (CastDir() * Vector3.Distance(_castTransform.position,target._gunFuAttackedAble.position));

        }

        Gizmos.color = Color.blue;
      
        
        Gizmos.DrawLine(_castTransform.position, sphrerPos);
        Gizmos.DrawWireSphere(sphrerPos,  this.Shpere_Raduis_Detecion);


    }
}
