using UnityEngine;
using UnityEngine.Animations;

public static class ParentConstraintAttachBehavior
{
    public static void Attach(ParentConstraint parentConstraint,Transform attachToTransform,Vector3 offsetVector,Quaternion offsetQuaternion)
    {
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = attachToTransform;
        source.weight = 1;
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }
        parentConstraint.AddSource(source);

        Vector3 translationOffset = offsetVector;
        Quaternion rotationOffsetQuat = Quaternion.Inverse(attachToTransform.rotation) * offsetQuaternion;
        Vector3 rotationOffset = rotationOffsetQuat.eulerAngles;

        parentConstraint.SetTranslationOffset(0, -translationOffset);
        parentConstraint.SetRotationOffset(0, rotationOffset);
        parentConstraint.constraintActive = true;
        parentConstraint.translationAtRest = Vector3.zero;
        parentConstraint.rotationAtRest = Vector3.zero;

        parentConstraint.constraintActive = true;
        parentConstraint.weight = 1;
    }
    public static void Attach(ParentConstraint parentConstraint, Transform attachToTransform)
    {
        Attach(parentConstraint, attachToTransform, Vector3.zero, Quaternion.identity);
    }
    public static void Detach(ParentConstraint parentConstraint)
    {
        if (parentConstraint.sourceCount > 0)
        {
            parentConstraint.RemoveSource(0);
        }

        parentConstraint.constraintActive = false;
        parentConstraint.weight = 0;
    }
}
