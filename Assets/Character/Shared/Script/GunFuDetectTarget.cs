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
  
   
    public bool CastDetectExecuteAbleTarget(out IGotGunFuAttackedAble gunFuGotExecuteAble)
    {
        gunFuGotExecuteAble = null;
        Vector3 castDir = CastDir();
        Ray ray = new Ray(_castTransform.position, castDir);
        RaycastHit[] collider = Physics.SphereCastAll(ray, _shpere_Raduis_Detecion, _sphere_Distance_Detection, 0 + gunFuAble._layerTarget);
        foreach (RaycastHit hit in collider)
        {
            if (hit.collider.gameObject.TryGetComponent<IGotGunFuAttackedAble>(out IGotGunFuAttackedAble gunFuGotAttackedAble) == false)
                continue;

            if (gunFuGotAttackedAble == gunFuAble)
            {
                gunFuDetectTargetDebug += "cast to self \n";
                continue;
            }

            if (gunFuGotAttackedAble._character.isDead)
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
    public bool CastDetect(out IGotGunFuAttackedAble target)
    {
        target = null;
        Vector3 casrDir = CastDir();

        if (CastDetect(out IGotGunFuAttackedAble gunFuTarget, casrDir))
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
    public bool CastDetectTargetInVolume(out List<IGotGunFuAttackedAble> target,Vector3 positionVolume,float raduis)
    {

        target = new List<IGotGunFuAttackedAble>();
        Collider[] colliders = Physics.OverlapSphere(positionVolume, raduis, gunFuAble._layerTarget.value);

        gunFuDetectTargetDebug += "layerTarget = " + gunFuAble._layerTarget.value + "\n";

        curPositionVolume = positionVolume;
        curRaduis = raduis;
        if(colliders == null)
            return false;

        if(colliders.Length <=0)
            return false;

        foreach (Collider item in colliders)
        {
            gunFuDetectTargetDebug += "in collider = " + item +"0 \n";

            if (item.TryGetComponent<IGotGunFuAttackedAble>(out IGotGunFuAttackedAble gunFuGotAttackedAble) == false)
                continue;

            gunFuDetectTargetDebug += "in collider = " + item + "1 \n";

            if (gunFuGotAttackedAble._character.isDead
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
    private bool CastDetect(out IGotGunFuAttackedAble target, Vector3 castDir)
    {
        target = null;
        Ray ray = new Ray(_castTransform.position,castDir);
        RaycastHit[] collider = Physics.SphereCastAll(ray, _shpere_Raduis_Detecion, _sphere_Distance_Detection, 0 + gunFuAble._layerTarget);
        foreach(RaycastHit hit in collider)
        {
            if(hit.collider.gameObject.TryGetComponent<IGotGunFuAttackedAble>(out IGotGunFuAttackedAble gunFuGotAttackedAble) == false)
                continue;

            if(gunFuGotAttackedAble._character.isDead 
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

        if (Vector3.Angle(gunFuAble._character.transform.forward, gunFuAble._gunFuAimDir) <= _limitAimAngleDegrees)
        {
            casrDir = new Vector3(gunFuAble._gunFuAimDir.x, 0, gunFuAble._gunFuAimDir.z);
        }
        else
        {
            if (Vector3.Dot(gunFuAble._character.transform.right, gunFuAble._gunFuAimDir) < 0)
            {
                casrDir = Quaternion.Euler(0,-LimitAimAngleDegrees,0) * gunFuAble._character.transform.forward;
            }
            else
                casrDir = Quaternion.Euler(0, LimitAimAngleDegrees, 0) * gunFuAble._character.transform.forward;

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

        if (EnableDebug == false)
            return;

        Gizmos.color = Color.red;
        Vector3 sphrerPos = this.CastTransform.position + (CastDir() * this._sphere_Distance_Detection);
        Gizmos.color = Color.blue;
      
        Gizmos.DrawLine(_castTransform.position, sphrerPos);
        Gizmos.DrawWireSphere(sphrerPos,  this.Shpere_Raduis_Detecion);


    }
}
