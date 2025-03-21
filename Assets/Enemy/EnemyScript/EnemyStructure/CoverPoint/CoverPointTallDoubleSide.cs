using UnityEngine;

public class CoverPointTallDoubleSide : CoverPoint
{
    [SerializeField] public Transform peekPosL;
    [SerializeField] public Transform peekPosR;

    [Range(0f, 100f)]
    public float _fovDistance;
    public override float fovDistance { get => _fovDistance; set => _fovDistance = value; }
    [Range(0f, 360f)]
    public float _fovAngleDegrees;
    public override float fovAngleDegrees { get => _fovAngleDegrees; set => _fovAngleDegrees = value; }

    public override bool CheckingTargetInCoverView(ICoverUseable coverUser, LayerMask targetMask, Transform peekPos,out GameObject target)
    {
        target = null;
        fieldOfView = new FieldOfView(fovDistance, fovAngleDegrees, peekPos);

        if (fieldOfView.FindSingleObjectInView(targetMask, out GameObject targetObj))
        {
            target = targetObj;
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

        if (Physics.Raycast(coverPos.position, Vector3.down, out RaycastHit hitCoverPos))
            this.coverUser.coverPos = hitCoverPos.point;
        else
            this.coverUser.coverPos = coverPos.position;

        if (Physics.Raycast(peekPosL.position, Vector3.down, out RaycastHit hitPeekPos))
            this.coverUser.peekPos = hitPeekPos.point;
        else
            this.coverUser.peekPos = peekPosL.position;

        this.coverUser.coverPoint = this;
    }
    public void TakeThisCover(ICoverUseable coverUser,Transform peekPos)
    {
        this.coverUser = coverUser;

        if (Physics.Raycast(coverPos.position, Vector3.down, out RaycastHit hitCoverPos))
            this.coverUser.coverPos = hitCoverPos.point;
        else
            this.coverUser.coverPos = coverPos.position;

        if (Physics.Raycast(peekPos.position, Vector3.down, out RaycastHit hitPeekPos))
            this.coverUser.peekPos = hitPeekPos.point;
        else
            this.coverUser.peekPos = peekPos.position;

        this.coverUser.coverPoint = this;
    }

    protected override void Start()
    {
        base.Start();
    }
    private void OnDrawGizmos()
    {
        if (coverPos == null || peekPosL == null || peekPosR == null)
            return;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(coverPos.position, 0.25f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(peekPosL.position, 0.25f);
        Gizmos.DrawSphere(peekPosR.position, 0.25f);

        Gizmos.color = Color.green;
        Vector3 desPos1 = peekPosL.position + (Quaternion.Euler(0, fovAngleDegrees / 2, 0) * peekPosL.forward) * fovDistance;
        Vector3 desPos2 = peekPosL.position + (Quaternion.Euler(0, -fovAngleDegrees / 2, 0) * peekPosL.forward) * fovDistance;
        Gizmos.DrawLine(peekPosL.position, desPos1);
        Gizmos.DrawLine(peekPosL.position, desPos2);
        desPos1 = peekPosR.position + (Quaternion.Euler(0, fovAngleDegrees / 2, 0) * peekPosR.forward) * fovDistance;
        desPos2 = peekPosR.position + (Quaternion.Euler(0, -fovAngleDegrees / 2, 0) * peekPosR.forward) * fovDistance;
        Gizmos.DrawLine(peekPosR.position, desPos1);
        Gizmos.DrawLine(peekPosR.position, desPos2);
    }
}
