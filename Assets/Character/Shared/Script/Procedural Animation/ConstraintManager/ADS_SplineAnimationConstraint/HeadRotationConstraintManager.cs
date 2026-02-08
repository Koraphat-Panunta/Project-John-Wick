using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HeadRotationConstraintManager : MonoBehaviour, IConstraintManager
{
    [SerializeField] private float weight;

    [SerializeField] protected MultiRotationConstraint multiRotationConstraint;
    [SerializeField] protected Transform headAnchor;
    [SerializeField] protected Transform headRotationRef;

    [Range(0, 100)]
    public float rotateSpeed;

    [Range(0, 360)]
    public float edgeEulerRotate;

    [Range(0, 360)]
    public float limitEulerRotateHorizontal;

    [Range(0, 360)]
    public float limitEulerRotateVertical;

    public float targetEulerRotateHorizontal;
    public float eulerRotateHorizontal;

    public float targetEulerRotateVertical;
    public float eulerRotateVertical;

    public void SetWeight(float w)
    {
        this.weight = Mathf.Clamp01(w);
        this.multiRotationConstraint.weight = weight;

    }
    public float GetWeight() => this.weight;
    private void Update()
    {
        this.UpdateTargetRotate();
    }

    public void SetLookDirection(Vector3 dir)
    {
        this.targetDir = dir;
    }

    public void SetLookPos(Vector3 pos)
    {
        this.targetDir = (pos - this.multiRotationConstraint.data.constrainedObject.position).normalized;
    }

    Vector3 targetDir;
    protected void UpdateTargetRotate()
    {

        this.targetEulerRotateHorizontal = Quaternion.FromToRotation(new Vector3(this.headAnchor.forward.x,this.targetDir.normalized.y,this.headAnchor.forward.z),this.targetDir.normalized).eulerAngles.y;
        this.targetEulerRotateVertical = Vector3.SignedAngle(this.headAnchor.forward, new Vector3(this.headAnchor.forward.x, this.targetDir.normalized.y, this.headAnchor.forward.z).normalized, this.headAnchor.right);


        if (this.eulerRotateHorizontal > 0)
        {
            if (this.targetEulerRotateHorizontal > edgeEulerRotate)
            {
                this.targetEulerRotateHorizontal -= 360;
            }
        }
        else // this.eulerRotateHorizontal <= 0
        {
            this.targetEulerRotateHorizontal -= 360;
            if (this.targetEulerRotateHorizontal <= -edgeEulerRotate)
            {
                this.targetEulerRotateHorizontal += 360;
            }
        }

        if (this.targetEulerRotateVertical > 180)
        {
            this.targetEulerRotateVertical -= 360;
        }



        this.eulerRotateHorizontal = Mathf.Lerp(this.eulerRotateHorizontal, Mathf.Clamp(this.targetEulerRotateHorizontal, -this.limitEulerRotateHorizontal, this.limitEulerRotateHorizontal), Time.deltaTime * rotateSpeed);
        this.eulerRotateVertical = Mathf.Lerp(this.eulerRotateVertical, Mathf.Clamp(this.targetEulerRotateVertical, -this.limitEulerRotateVertical, this.limitEulerRotateVertical), Time.deltaTime * this.rotateSpeed);

        Vector3 dir = Quaternion.LookRotation(headAnchor.forward, Vector3.up) * Quaternion.Euler(this.eulerRotateVertical, this.eulerRotateHorizontal, 0) * Vector3.forward;

        Quaternion rotate = Quaternion.LookRotation(dir.normalized, this.headAnchor.up);
        this.headRotationRef.rotation = rotate;

        Debug.DrawRay(this.multiRotationConstraint.data.constrainedObject.position, this.multiRotationConstraint.data.constrainedObject.forward,Color.blue);
    }


}
