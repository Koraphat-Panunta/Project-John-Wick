using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class BodySetup : MonoBehaviour
{
    [SerializeField] protected Enemy enemy;

    [SerializeField] protected HumanoidBone humanoidBone;

    [SerializeField] protected BodyPartDamageRecivedSCRP head_BodyPartDamageRecivedSCRP;
    [SerializeField] protected BodyPartDamageRecivedSCRP body_BodyPartDamageRecivedSCRP;
    [SerializeField] protected BodyPartDamageRecivedSCRP arms_BodyPartDamageRecivedSCRP;
    [SerializeField] protected BodyPartDamageRecivedSCRP legs_BodyPartDamageRecivedSCRP;

    [Range(0,100)]
    [SerializeField] private float mass;

    private float headMassRatio = .08f;
    private float bodyMassRatio = .25f;
    private float armMassRatio = .05f;
    private float legMassRatio = .16f;


    [Range(0,.01f)]
    [SerializeField] private float headRaduis;
    [Range(0,.01f)]
    [SerializeField] private float headHeight;
    [SerializeField] private Vector3 headOffset;

    [SerializeField] private Vector3 bodySize;
    [SerializeField] private Vector3 bodyCenter;
    [SerializeField] private Vector3 hipCenter;
    [SerializeField] private Vector3 hipSize;
    [Range(0, .01f)]
    [SerializeField] private float legRaduis;
    [Range(0, .01f)]
    [SerializeField] private float armRaduis;
    [Range(0, .01f)]
    [SerializeField] private float armLenght;
    [Range(-.01f,.01f)]
    [SerializeField] private float armOffset;

    [SerializeField] EnemyAnimationManager enemyAnimationManager;
    [SerializeField] EnemyConstrainAnimationNodeManager enemyConstrainAnimationNodeManager;

    public void SetBodyOwner()
    {
        Transform[] bones = this.GetBoneRagdollPart();
        for (int i = 0; i < bones.Length; i++) 
        {
            this.SetBoneBodyOwner(bones[i]);
        }

        if (this.enemyAnimationManager != null)
            this.enemyAnimationManager.enemy = this.enemy;

        if (this.enemyConstrainAnimationNodeManager != null)
            this.enemyConstrainAnimationNodeManager.enemy = this.enemy;

    }
    private void SetBoneBodyOwner(Transform bone)
    {
        if (bone.TryGetComponent<BodyPart>(out BodyPart bodyPart))
        {
            bodyPart.SetCharacterBodyOwner(this.enemy);
        }
        else
        {
            Debug.LogError("this bone " + nameof(bone) + " is not body part");
        }
        
    }
    public void SetupBodyPart()
    {
        if(this.humanoidBone == null)
        {
            Debug.LogError("None assing humanoidBone");
            return;
        }

        this.SetUpBodyPart<HeadBodyPart>(this.humanoidBone._headBone, this.head_BodyPartDamageRecivedSCRP);

        this.SetUpBodyPart<ChestBodyPart>(this.humanoidBone.hips, this.body_BodyPartDamageRecivedSCRP);
        this.SetUpBodyPart<ChestBodyPart>(this.humanoidBone._spine_0_Bone, this.body_BodyPartDamageRecivedSCRP);

        this.SetUpBodyPart<ArmLeftBodyPart>(this.humanoidBone._leftForeArmBone, this.arms_BodyPartDamageRecivedSCRP);
        this.SetUpBodyPart<ArmLeftBodyPart>(this.humanoidBone._leftArmBone, this.arms_BodyPartDamageRecivedSCRP);

        this.SetUpBodyPart<ArmRightBodyPart>(this.humanoidBone._rightArmBone, this.arms_BodyPartDamageRecivedSCRP);
        this.SetUpBodyPart<ArmRightBodyPart>(this.humanoidBone._rightForeArmBone, this.arms_BodyPartDamageRecivedSCRP);

        this.SetUpBodyPart<LegLeftBodyPart>(this.humanoidBone._leftUpperLegBone, this.legs_BodyPartDamageRecivedSCRP);
        this.SetUpBodyPart<LegLeftBodyPart>(this.humanoidBone._leftLowerLegBone, this.legs_BodyPartDamageRecivedSCRP);

        this.SetUpBodyPart<LegRightBodyPart>(this.humanoidBone._rightUpperLegBone, this.legs_BodyPartDamageRecivedSCRP);
        this.SetUpBodyPart<LegRightBodyPart>(this.humanoidBone._rightLowerLegBone, this.legs_BodyPartDamageRecivedSCRP);

    }

    private void SetUpBodyPart<T>(Transform bone,BodyPartDamageRecivedSCRP bodyPartDamageRecivedSCRP) where T : BodyPart
    {
        if (bone == null)
            Debug.LogError("None assing humanoidBone " + nameof(bone));

        if (bone.TryGetComponent<T>(out T bodyPart) == false) 
        {
            bone.gameObject.AddComponent<T>().SetBodyPartDamageRecivedSCRP(bodyPartDamageRecivedSCRP);
        }
        else
        {
            bodyPart.SetBodyPartDamageRecivedSCRP(bodyPartDamageRecivedSCRP);
        }


    }

    public Transform[] GetBoneRagdollPart()
    {
        Transform[] bone = new Transform[11];

        bone[0] = this.humanoidBone.hips;
        bone[1] = this.humanoidBone._spine_0_Bone;
        
        bone[2] = this.humanoidBone._headBone;

        bone[3] = this.humanoidBone._leftArmBone;
        bone[4] = this.humanoidBone._leftForeArmBone;

        bone[5] = this.humanoidBone._rightArmBone;
        bone[6] = this.humanoidBone._rightForeArmBone;

        bone[7] = this.humanoidBone._leftUpperLegBone;
        bone[8] = this.humanoidBone._leftLowerLegBone;

        bone[9] = this.humanoidBone._rightUpperLegBone;
        bone[10] = this.humanoidBone._rightLowerLegBone;

        return bone;
    }
  
    private void ReValue(Transform bone, float massRatio ,float raduis)
    {
        bone.GetComponent<Rigidbody>().mass = massRatio * this.mass;
      
        if(bone.TryGetComponent<CapsuleCollider>(out CapsuleCollider capsuleCollider))
        {
            capsuleCollider.radius = raduis;
        }
    }
    private void ReValue(Transform bone, float massRatio, Vector3 size)
    {
        bone.GetComponent<Rigidbody>().mass = massRatio * this.mass;
        bone.GetComponent<BoxCollider>().size = size * .01f;
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying)
        {
            if(this.enemyAnimationManager == null)
            {
                if(TryGetComponent<EnemyAnimationManager>(out EnemyAnimationManager enemyAnimationManager))
                {
                    this.enemyAnimationManager = enemyAnimationManager;
                }
            }
            if(this.enemyConstrainAnimationNodeManager == null)
            {
                this.enemyConstrainAnimationNodeManager = this.gameObject.GetComponentInChildren<EnemyConstrainAnimationNodeManager>();
            }
            this.ReValue(this.humanoidBone._headBone, this.headMassRatio, this.headRaduis);

            if(this.humanoidBone._headBone.TryGetComponent<CapsuleCollider>(out CapsuleCollider headCollider))
            {
                headCollider.center = this.headOffset * .01f;
                headCollider.height = this.headHeight;
            }


            this.ReValue(this.humanoidBone._spine_0_Bone, this.bodyMassRatio, this.bodySize);
            if (this.humanoidBone._spine_0_Bone.TryGetComponent<BoxCollider>(out BoxCollider splineCollider))
            {
                splineCollider.center = this.bodyCenter * .01f;
            }
            this.ReValue(this.humanoidBone.hips, this.bodyMassRatio, this.hipSize);
            if (this.humanoidBone.hips.TryGetComponent<BoxCollider>(out BoxCollider hipCollider))
            {
                hipCollider.center = this.hipCenter * .01f;
            }

            this.ReValue(this.humanoidBone._leftArmBone, this.armMassRatio, this.armRaduis);
            this.ReValue(this.humanoidBone._leftForeArmBone, this.armMassRatio, this.armRaduis);
            this.ReValue(this.humanoidBone._rightArmBone, this.armMassRatio, this.armRaduis);
            this.ReValue(this.humanoidBone._rightForeArmBone, this.armMassRatio, this.armRaduis);

            if (this.humanoidBone._rightForeArmBone.TryGetComponent<CapsuleCollider>(out CapsuleCollider rightForeArm))
            {
                rightForeArm.height = this.armLenght;
                rightForeArm.center = new Vector3(0, this.armOffset, 0);
            }
            if (this.humanoidBone._leftForeArmBone.TryGetComponent<CapsuleCollider>(out CapsuleCollider leftForeArm))
            {
                leftForeArm.height = this.armLenght;
                leftForeArm.center = new Vector3(0, this.armOffset, 0);
            }

            this.ReValue(this.humanoidBone._leftLowerLegBone, this.legMassRatio, this.legRaduis);
            this.ReValue(this.humanoidBone._leftUpperLegBone, this.legMassRatio, this.legRaduis);
            this.ReValue(this.humanoidBone._rightLowerLegBone, this.legMassRatio, this.legRaduis);
            this.ReValue(this.humanoidBone._rightUpperLegBone, this.legMassRatio, this.legRaduis);
        }
    }
#endif

}
