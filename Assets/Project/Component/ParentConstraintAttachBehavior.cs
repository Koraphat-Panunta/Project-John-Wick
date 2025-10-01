using UnityEngine;
using UnityEngine.Animations;

public static class ParentConstraintAttachBehavior
{
    public static void Attach(
        ParentConstraint parentConstraint,
        Transform attachToTransform,
        Vector3 offsetVector,
        Quaternion offsetQuaternion)
    {
        // Ensure source
        ConstraintSource source = new ConstraintSource
        {
            sourceTransform = attachToTransform,
            weight = 1
        };
        if (parentConstraint.sourceCount > 0)
            parentConstraint.RemoveSource(0);
        parentConstraint.AddSource(source);

        // Apply rotation to the offset vector (anchor rotation logic)
        Vector3 rotatedOffset = offsetQuaternion * offsetVector;

        // Use rotated offset for translation
        parentConstraint.SetTranslationOffset(0, -rotatedOffset);

        // Keep rotation offset as usual
        parentConstraint.SetRotationOffset(0, offsetQuaternion.eulerAngles);

        // Reset rest
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
            parentConstraint.RemoveSource(0);

        parentConstraint.constraintActive = false;
        parentConstraint.weight = 0;
    }
}
