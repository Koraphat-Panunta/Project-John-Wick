using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MotionControlManager 
{
    public RagdollMotionState ragdollMotionState;
    public AnimationDrivenMotionState animationDrivenMotionState;
    public CodeDrivenMotionState codeDrivenMotionState;

    private ResetingBoneMotionState resetingBoneMotionState;
    public MotionState curMotionState;

    public List<BoneTransform> animationBoneTransform;
    public List<BoneTransform> ragdollBoneTransform;
    private List<Transform> myBones = new List<Transform>();
    private Animator myAnimator;
     public MotionControlManager(List<GameObject> bones,GameObject hips,Animator animator) 
    {
        ragdollBoneTransform = new List<BoneTransform>();
        animationBoneTransform = new List<BoneTransform>();

        for (int i = 0; i < bones.Count; i++)
        {
            myBones.Add(bones[i].GetComponent<Transform>());

            ragdollBoneTransform.Add(new BoneTransform());
            animationBoneTransform.Add(new BoneTransform());
        }

        myAnimator = animator;

        ragdollMotionState = new RagdollMotionState(bones,hips,myAnimator);
        animationDrivenMotionState = new AnimationDrivenMotionState(animator);
        codeDrivenMotionState = new CodeDrivenMotionState(animator);
        resetingBoneMotionState = new ResetingBoneMotionState(myBones);
        ChangeMotionState(animationDrivenMotionState);
    }
    public void Update()
    {
        curMotionState.Update();
    }
    public void FixedUpdate()
    {
        curMotionState.FixedUpdate();
    }
    public void ChangeMotionState(MotionState nextMotionState)
    {
        

        if (curMotionState != null)
            curMotionState.Exit();
        curMotionState = nextMotionState;
        curMotionState.Enter();
    }

    public void ChangeMotionState(MotionState nextMotionState,AnimationClip animationClip)
    {
        if (curMotionState == ragdollMotionState
             && nextMotionState == animationDrivenMotionState){

            AlignRotationToHips();
            AlignPositionToHips();

            PopulateBoneTransform(ragdollBoneTransform);
            PopulateAnimationStartBoneTransforms(animationClip);

            curMotionState = resetingBoneMotionState;
            curMotionState.Enter();

            return;
        }
        ChangeMotionState(nextMotionState);
    }

    private void PopulateBoneTransform(List<BoneTransform> bones)
    {
        for (int i = 0;i < myBones.Count; i++)
        {
            bones[i].Position = myBones[i].localPosition;
            bones[i].Rotation = myBones[i].localRotation;
        }
    }

    private void PopulateAnimationStartBoneTransforms(AnimationClip clip)
    {
        Transform transform = myAnimator.transform;
        GameObject gameObject = transform.gameObject;

        Vector3 positionBeforeSampling = transform.position;
        Quaternion rotationBeforeSampling = transform.rotation;

        clip.SampleAnimation(gameObject, 0);
        PopulateBoneTransform(animationBoneTransform);

        transform.position = positionBeforeSampling;
        transform.rotation = rotationBeforeSampling;
    }

    private void AlignPositionToHips()
    {
        Transform _hipsBone = ragdollMotionState.hips.transform;
        Transform transform = myAnimator.gameObject.transform;

        Vector3 originalHipsPosition = _hipsBone.position;
        transform.position = _hipsBone.position;

        Vector3 positionOffset = animationBoneTransform[2].Position;
        positionOffset.y = 0;
        positionOffset = transform.rotation * positionOffset;
        transform.position -= positionOffset;

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo))
        {
            transform.position = new Vector3(transform.position.x, hitInfo.point.y, transform.position.z);
        }

        _hipsBone.position = originalHipsPosition;
    }

    private void AlignRotationToHips()
    {
        Transform _hipsBone = ragdollMotionState.hips.transform;
        Transform transform = myAnimator.gameObject.transform;

        Vector3 originalHipsPosition = _hipsBone.position;
        Quaternion originalHipsRotation = _hipsBone.rotation;

        Vector3 desiredDirection = _hipsBone.up * -1;
        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(transform.forward, desiredDirection);
        transform.rotation *= fromToRotation;

        _hipsBone.position = originalHipsPosition;
        _hipsBone.rotation = originalHipsRotation;
    }
}
