using UnityEngine;

public class CoverPointTallSingleSide : CoverPoint
{
    [SerializeField] public Transform peekPos;

    [Range(0f,100f)]
    public float _fovDistance;
    public override float fovDistance { get => _fovDistance ; set => _fovDistance = value ; }
    [Range(0f,360f)]
    public float _fovAngleDegrees;
    public override float fovAngleDegrees { get => _fovAngleDegrees ; set => _fovAngleDegrees = value ; }

    public override bool CheckingTargetInCoverView(ICoverUseable coverUser, LayerMask targetMask, Transform peekPos)
    {
        fieldOfView = new FieldOfView(fovDistance,fovAngleDegrees,peekPos);

        if(fieldOfView.FindSingleObjectInView(targetMask,out GameObject targetObj))
        {
            return true;
        }
        return false;
    }

    public override void OffThisCover()
    {
        this.coverUser = null;
    }

    public override void TakeThisCover(ICoverUseable coverUser)
    {
        this.coverUser = coverUser;
        this.coverUser.coverPos = base.coverPos.position;
        this.coverUser.peekPos = this.peekPos.position;
    }
    
    protected override void Start()
    {
        base.Start();
    }
    private void OnDrawGizmos()
    {
        if(coverPos == null || peekPos == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(coverPos.position,0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(peekPos.position, 0.25f);

        Gizmos.color = Color.green;
        Vector3 desPos1 = peekPos.position + (Quaternion.Euler(0, fovAngleDegrees / 2, 0) * peekPos.forward) * fovDistance;
        Vector3 desPos2 = peekPos.position + (Quaternion.Euler(0, -fovAngleDegrees / 2, 0) * peekPos.forward) * fovDistance;
        Gizmos.DrawLine(peekPos.position,desPos1);
        Gizmos.DrawLine(peekPos.position, desPos2);
    }
}
