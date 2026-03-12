using System;
using UnityEngine;

public class ProneLegsConstrainNodeLeaf : AnimationConstrainNodeLeaf
{
    protected LegsConstrainManager legsConstrainManager { get; set; }

    public float angle; //0-360
    protected LegsBlendingConstrainScriptableObject legsBlendIKConstrainScriptableObject { get; set; }

    public Vector3 target_LeftLeg_Position { get; protected set; }
    public Quaternion target_LeftLeg_Rotation { get; protected set; }
    public Vector3 hint_LeftLeg_Position { get; protected set; }

    public Vector3 target_RightLeg_Position { get; protected set; }
    public Quaternion target_RightLeg_Rotation { get; protected set; }
    public Vector3 hint_RightLeg_Position { get; protected set; }


    protected float weight;

    protected Transform refPos;
    protected Transform refDir;

    protected Transform leftFootTransform => this.legsConstrainManager.GetLeftLeg_Target_Transform();
    protected Transform rightFootTransform => this.legsConstrainManager.GetRightLeg_Target_Transform();

    public float transitionSpeed;

    public ProneLegsConstrainNodeLeaf(
        LegsConstrainManager legsConstrainManager
        ,Transform refPos
        ,Transform refDir
        ,LegsBlendingConstrainScriptableObject legsIKConstrainScriptableObject
        ,Func<bool> precondition) : base(precondition)
    {
        this.legsConstrainManager = legsConstrainManager;
        this.legsBlendIKConstrainScriptableObject = legsIKConstrainScriptableObject;

        this.refPos = refPos;
        this.refDir = refDir;
    }

    public override void Enter()
    {
        this.weight = 0f;
        this.CalculateProperty();
        base.Enter();
    }

    public override void UpdateNode()
    {
        this.weight = Mathf.Clamp(this.weight + Time.deltaTime,0,1);
        this.CalculateProperty();
        this.UpdateLegsConstrainManager();

        base.UpdateNode();
    }
    public override void FixedUpdateNode()
    {
        base.FixedUpdateNode();
    }
    protected void CalculateProperty()
    {
        LegsIKConstrainScriptableObject scrp =
       this.legsBlendIKConstrainScriptableObject.GetBlendData(this.angle);

        Vector3 refForward = this.refDir.forward;
        Vector3 refRight = this.refDir.right;
        Vector3 refUp = this.refDir.up;

        Quaternion refRot = Quaternion.LookRotation(this.refDir.forward,this.refDir.up);

        // LEFT LEG POSITION
        this.target_LeftLeg_Position =
            this.refPos.position
            + (refForward * scrp.leftLegPositionOffset.z)
            + (refUp * scrp.leftLegPositionOffset.y)
            + (refRight * scrp.leftLegPositionOffset.x);

        // LEFT LEG ROTATION
        this.target_LeftLeg_Rotation =
            refRot * Quaternion.Euler(scrp.leftLegRotationEulerOffset);

        // LEFT LEG HINT (anchor = left foot)
        this.hint_LeftLeg_Position =
            this.leftFootTransform.position
            + (this.leftFootTransform.forward * scrp.leftLegHintPositionOffset.z)
            + (this.leftFootTransform.up * scrp.leftLegHintPositionOffset.y)
            + (this.leftFootTransform.right * scrp.leftLegHintPositionOffset.x);


        // RIGHT LEG POSITION
        this.target_RightLeg_Position =
            this.refPos.position
            + (refForward * scrp.rightLegPositionOffset.z)
            + (refUp * scrp.rightLegPositionOffset.y)
            + (refRight * scrp.rightLegPositionOffset.x);

        // RIGHT LEG ROTATION
        this.target_RightLeg_Rotation =
            refRot * Quaternion.Euler(scrp.rightLegRotationEulerOffset);

        // RIGHT LEG HINT (anchor = right foot)
        this.hint_RightLeg_Position =
            this.rightFootTransform.position
            + (this.rightFootTransform.forward * scrp.rightLegHintPositionOffset.z)
            + (this.rightFootTransform.up * scrp.rightLegHintPositionOffset.y)
            + (this.rightFootTransform.right * scrp.rightLegHintPositionOffset.x);
    }

    protected void UpdateLegsConstrainManager()
    {

        //IK_Update
        Vector3 castDownWardLeftLegPos = new Vector3(this.target_LeftLeg_Position.x, this.refPos.position.y, this.target_LeftLeg_Position.z);
        Vector3 castDownWardRightLegPos = new Vector3(this.target_RightLeg_Position.x, this.refPos.position.y, this.target_RightLeg_Position.z);

        Vector3 leftLegPos = this.target_LeftLeg_Position;
        Vector3 rightLegPos = this.target_RightLeg_Position;

        Vector3 hintLeftPos = this.hint_LeftLeg_Position;
        Vector3 hintRightPos = this.hint_RightLeg_Position;

        RaycastHit hit;

        float biasIKDistance = .1f;

        // ---------- LEFT FOOT IK ----------
        Vector3 leftDir = (this.target_LeftLeg_Position - castDownWardLeftLegPos);
        float leftDist = leftDir.magnitude + biasIKDistance;
        Vector3 refToGround = leftLegPos - this.refPos.position;

        if (Physics.Raycast(
                castDownWardLeftLegPos,
                leftDir.normalized,
                out hit,
                leftDist,
                LayerMask.GetMask("Default"),
                QueryTriggerInteraction.Ignore))
        {
            leftLegPos = hit.point + (leftDir.normalized*-1 * biasIKDistance);
            refToGround = leftLegPos - this.refPos.position;

        }

        if (Physics.Raycast(
                    this.refPos.position,
                    refToGround.normalized,
                    out hit,
                    refToGround.magnitude + biasIKDistance,
                    LayerMask.GetMask("Default"),
                    QueryTriggerInteraction.Ignore))
        {
            leftLegPos = hit.point + (refToGround.normalized * -1 * biasIKDistance);
        }
        //Vector3 hintCastPos = new Vector3(hintLeftPos.x, this.refPos.position.y, hintLeftPos.z);
        //Vector3 hintCastDir = hintLeftPos = hintCastPos;

        //if (Physics.Raycast(
        //                hintCastPos,
        //                hintCastDir.normalized,
        //                out hit,
        //                hintCastDir.magnitude,
        //                LayerMask.GetMask("Default"),
        //                QueryTriggerInteraction.Ignore))
        //{
        //    hintLeftPos = hit.point;
        //}



        // ---------- RIGHT FOOT IK ----------
        Vector3 rightDir = (this.target_RightLeg_Position - castDownWardRightLegPos);
        float rightDist = rightDir.magnitude + biasIKDistance;
        refToGround = rightLegPos - this.refPos.position;
        if (Physics.Raycast(
                castDownWardRightLegPos,
                rightDir.normalized,
                out hit,
                rightDist,
                LayerMask.GetMask("Default"),
                QueryTriggerInteraction.Ignore))
        {
            rightLegPos = hit.point + (rightDir.normalized *-1 * biasIKDistance);
            refToGround = rightLegPos - this.refPos.position;
        }

        if (Physics.Raycast(
                      this.refPos.position,
                      refToGround.normalized,
                      out hit,
                      refToGround.magnitude + biasIKDistance,
                      LayerMask.GetMask("Default"),
                      QueryTriggerInteraction.Ignore))
        {
            rightLegPos = hit.point + (refToGround.normalized * -1 * biasIKDistance);
        }
        //hintCastPos = new Vector3(hintRightPos.x, this.refPos.position.y, hintRightPos.z);
        //hintCastDir = hintRightPos = hintCastPos;

        //if (Physics.Raycast(
        //                hintCastPos,
        //                hintCastDir.normalized,
        //                out hit,
        //                hintCastDir.magnitude,
        //                LayerMask.GetMask("Default"),
        //                QueryTriggerInteraction.Ignore))
        //{
        //    hintRightPos = hit.point;
        //}



        // LEFT LEG TARGET
        this.legsConstrainManager.SetLeftLeg_Target_Foot(
            Vector3.Lerp(
                this.legsConstrainManager.leftLegTransformValue.position,
                leftLegPos,
                this.weight * Time.deltaTime * this.transitionSpeed ),
            Quaternion.Lerp(
                Quaternion.Euler(this.legsConstrainManager.leftLegTransformValue.rotationEuler),
                this.target_LeftLeg_Rotation,
                this.weight * Time.deltaTime * this.transitionSpeed)
        );

        // LEFT LEG HINT
        this.legsConstrainManager.SetLeftLeg_Hint_FootPos(
            Vector3.Lerp(
                this.legsConstrainManager.hintLeftLegPos,
                hintLeftPos,
                this.weight)
        );


        // RIGHT LEG TARGET
        this.legsConstrainManager.SetRightLeg_Target_Foot(
            Vector3.Lerp(
                this.legsConstrainManager.rightLegTransformValue.position,
                rightLegPos,
                this.weight * Time.deltaTime * this.transitionSpeed),
            Quaternion.Lerp(
                Quaternion.Euler(this.legsConstrainManager.rightLegTransformValue.rotationEuler),
                this.target_RightLeg_Rotation,
                this.weight * Time.deltaTime * this.transitionSpeed)
        );

        // RIGHT LEG HINT
        this.legsConstrainManager.SetRightLeg_Hint_FootPos(
            Vector3.Lerp(
                this.legsConstrainManager.hintRightLegPos,
                hintRightPos,
                this.weight )
        );
    }

    public void SetAngle(float angle) => this.angle = angle;
    public void SetSCRP(LegsBlendingConstrainScriptableObject legsBlendingConstrainScriptableObject)
        => this.legsBlendIKConstrainScriptableObject = legsBlendingConstrainScriptableObject;

    public void SetTransitionSpeed(float v) => this.transitionSpeed = v;
    public void SetWeight(float w) => this.weight = w;
}
