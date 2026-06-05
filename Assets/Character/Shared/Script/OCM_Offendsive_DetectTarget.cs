using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(I_OCM_Attack_Able))]
public class OCM_Offendsive_DetectTarget : MonoBehaviour,IInitializedAble
{
    [SerializeField] protected I_OCM_Attack_Able gunFuAble;

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

    public LayerMask _layerTarget;

    public void Initialized()
    {
        this.gunFuAble = GetComponent<I_OCM_Attack_Able>();
    }
    public bool CastDetectExecuteAbleTarget(out I_Got_OCM_Attacked_Able gunFuGotExecuteAble)
    {
        gunFuGotExecuteAble = null;
        Vector3 castDir = CastDir();

        if (!CastFinding.FindLiveObjectInViewByComponent<I_Got_OCM_Attacked_Able>(
            _castTransform.position,
            castDir,
            _sphere_Distance_Detection,
            _shpere_Raduis_Detecion,
            _layerTarget,
            out I_Got_OCM_Attacked_Able found,
            QueryTriggerInteraction.Collide))
            return false;

        if (found.gotGunFuAttackedAble == gunFuAble || found._isGotExecutedAble == false)
            return false;

        gunFuGotExecuteAble = found.gotGunFuAttackedAble;
        return true;
    } // Called form gunFuAble
    public bool CastDetect(out I_Got_OCM_Attacked_Able target)
    {
        target = null;
        Vector3 casrDir = CastDir();

        if (CastDetect(out I_Got_OCM_Attacked_Able gunFuTarget, casrDir))
        {
            target = gunFuTarget;
            return true;
        }
        else
        {
            return false;
        }
    } // Called form player
    public bool CastDetectTargetInVolume(out List<I_Got_OCM_Attacked_Able> target, Vector3 positionVolume, float raduis, LayerMask targetMask)
    {
        target = new List<I_Got_OCM_Attacked_Able>();

        if (!CastFinding.FindAllLiveObjectsInConeByComponent<I_Got_OCM_Attacked_Able>(
            positionVolume,
            Vector3.forward,
            raduis,
            180f,
            targetMask,
            out List<I_Got_OCM_Attacked_Able> found,
            triggerInteraction: QueryTriggerInteraction.Collide))
            return false;

        foreach (I_Got_OCM_Attacked_Able item in found)
        {
            if (item._isGotAttackedAble == false || item.gotGunFuAttackedAble == gunFuAble)
                continue;
            if (!target.Contains(item.gotGunFuAttackedAble))
                target.Add(item.gotGunFuAttackedAble);
        }

        return target.Count > 0;
    }// Called form gunFuAble
    public bool CastDetectTargetInVolume(out List<I_Got_OCM_Attacked_Able> target, Vector3 positionVolume, float raduis)
    {
        return CastDetectTargetInVolume(out target,positionVolume,raduis,this._layerTarget);
    }// Called form gunFuAble
    private bool CastDetect(out I_Got_OCM_Attacked_Able target, Vector3 castDir)
    {
        target = null;

        if (!CastFinding.FindLiveObjectInViewByComponent<I_Got_OCM_Attacked_Able>(
            _castTransform.position,
            castDir,
            _sphere_Distance_Detection,
            _shpere_Raduis_Detecion,
            _layerTarget,
            out I_Got_OCM_Attacked_Able found,
            QueryTriggerInteraction.Collide))
            return false;

        if (found.gotGunFuAttackedAble == gunFuAble || found._isGotAttackedAble == false)
            return false;

        target = found.gotGunFuAttackedAble;
        return true;
    }
    private Vector3 CastDir()
    {
        Vector3 casrDir;

        if (Vector3.Angle(gunFuAble._character.transform.forward, gunFuAble._attackAimDir) <= _limitAimAngleDegrees)
        {
            casrDir = new Vector3(gunFuAble._attackAimDir.x, 0, gunFuAble._attackAimDir.z);
        }
        else
        {
            if (Vector3.Dot(gunFuAble._character.transform.right, gunFuAble._attackAimDir) < 0)
            {
                casrDir = Quaternion.Euler(0,-LimitAimAngleDegrees,0) * gunFuAble._character.transform.forward;
            }
            else
                casrDir = Quaternion.Euler(0, LimitAimAngleDegrees, 0) * gunFuAble._character.transform.forward;

        }


        return casrDir;
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
