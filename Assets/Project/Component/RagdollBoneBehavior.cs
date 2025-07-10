using UnityEngine;

public static class RagdollBoneBehavior 
{
    public static void AlignPositionToHips(Transform root, Transform hipsBone, Transform enemyTransform, BoneTransform hipsAnimStartTransform)
    {
        Vector3 originalHipsPosition = hipsBone.position;
        Vector3 originalPos = enemyTransform.position;
        //Vector3 hipOffset = enemyTransform.position - root.position;
        enemyTransform.position = hipsBone.position;

        Vector3 positionOffset = hipsAnimStartTransform.Position;
        positionOffset.y = 0;
        positionOffset = enemyTransform.rotation * positionOffset;

        enemyTransform.position -= positionOffset;
        enemyTransform.position = new Vector3(enemyTransform.position.x, originalPos.y, enemyTransform.position.z);
       
        hipsBone.position = originalHipsPosition;
    }

    public static void AlignRotationToHips(Transform hipsBone, Transform enemyTransform)
    {
        Vector3 originalHipsPosition = hipsBone.position;
        Quaternion originalHipsRotation = hipsBone.rotation;

        Vector3 desiredDirection = hipsBone.up;
        if (Vector3.Dot(hipsBone.forward, Vector3.up) > 0)
        {
            desiredDirection *= -1;
        }

        desiredDirection.y = 0;
        desiredDirection.Normalize();

        Quaternion fromToRotation = Quaternion.FromToRotation(enemyTransform.forward, desiredDirection);
        enemyTransform.rotation *= fromToRotation;

        hipsBone.position = originalHipsPosition;
        hipsBone.rotation = originalHipsRotation;
    }

    public static void PopulateBoneTransforms(Transform[] bones, BoneTransform[] boneTransforms)
    {
        for (int i = 0; i < bones.Length; i++)
        {
            boneTransforms[i].Position = bones[i].localPosition;
            boneTransforms[i].Rotation = bones[i].localRotation;
        }
    }

    public static void PopulateAnimationStartBoneTransforms(
        AnimationClip clip,
        GameObject enemyGO,
        Transform[] bones,
        BoneTransform[] boneTransforms,
        Transform enemyTransform)
    {
        Vector3 positionBeforeSampling = enemyTransform.position;
        Quaternion rotationBeforeSampling = enemyTransform.rotation;

        clip.SampleAnimation(enemyGO, 0);
        PopulateBoneTransforms(bones, boneTransforms);

        enemyTransform.position = positionBeforeSampling;
        enemyTransform.rotation = rotationBeforeSampling;
    }
    public static void LerpBoneTransforms(
        Transform[] bones,
        BoneTransform[] fromTransforms,
        BoneTransform[] toTransforms,
        float t)
    {
        t= Mathf.Clamp01(t);

        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localPosition = Vector3.Lerp(
                fromTransforms[i].Position,
                toTransforms[i].Position,
                t);

            bones[i].localRotation = Quaternion.Lerp(
                fromTransforms[i].Rotation,
                toTransforms[i].Rotation,
                t);
        }
    }
   
}
