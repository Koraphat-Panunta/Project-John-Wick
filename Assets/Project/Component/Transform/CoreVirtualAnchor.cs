using System.Collections.Generic;
using UnityEngine;

public class CoreVirtualAnchor 
{
    private Vector3 anchorPosition;
    private Quaternion anchorRotation;

    public struct TransformVirtual
    {
        public Vector3 localPosition;
        public Quaternion localRotation;
    }
    public Dictionary<Transform, TransformVirtual> childTransform;

    public CoreVirtualAnchor(Vector3 anchorPosition, Quaternion anchorRotation)
    {
        this.anchorPosition = anchorPosition;
        this.anchorRotation = anchorRotation;
        childTransform = new Dictionary<Transform, TransformVirtual>();
    }

    public void UpdateCoreVirtualAnchorTransform()
    {
        if (childTransform.Count > 0)
        {

            var keys = new List<Transform>(childTransform.Keys);

            for (int i = 0; i < keys.Count; i++)
            {
                Transform child = keys[i];

                Vector3 childPosLocal = Quaternion.Inverse(anchorRotation) * (child.position - anchorPosition);
                Quaternion childRotLocal = Quaternion.Inverse(anchorRotation) * child.rotation;

                this.childTransform[child] = new TransformVirtual
                {
                    localPosition = childPosLocal,
                    localRotation = childRotLocal
                };
            }
        }
    }

    public Vector3 GetAnchorPosition() => anchorPosition;
    public Quaternion GetAnchorRotation() => anchorRotation;
    public void SetAnchorPosition(Vector3 anchorPosition)
    {
        Vector3 deltaPosition = anchorPosition - this.anchorPosition;
        foreach (Transform childTransform in childTransform.Keys)
        {
            childTransform.position += deltaPosition;
        }
        this.anchorPosition = anchorPosition;
    }
    public void SetAnchorRotation(Quaternion newAnchorRotation)
    {
        // Compute delta from current anchor rotation → new rotation
        Quaternion deltaRotation = newAnchorRotation * Quaternion.Inverse(this.anchorRotation);

        // Rotate each child around the anchor
        foreach (Transform child in childTransform.Keys)
        {
            // Update position relative to anchor
            Vector3 offset = child.position - anchorPosition;
            child.position = anchorPosition + deltaRotation * offset;

            // Update child world rotation
            child.rotation = deltaRotation * child.rotation;
        }

        // Update anchor rotation state
        this.anchorRotation = newAnchorRotation;
    }
    public void AddChildTransform(Transform childTransform)
    {
        Vector3 childPosLocal = Quaternion.Inverse(anchorRotation) * (childTransform.position - anchorPosition);
        Quaternion childRotLocal = Quaternion.Inverse(anchorRotation) * childTransform.rotation;

        TransformVirtual childVirtualTransform = new TransformVirtual
        {
            localPosition = childPosLocal,
            localRotation = childRotLocal
        };
        this.childTransform.Add(childTransform, childVirtualTransform);
    }
    public void RemoveChildTransform(Transform childTransform)
    {
        if (this.childTransform.TryGetValue(childTransform, out TransformVirtual transformVirtual))
        {
            this.childTransform.Remove(childTransform);
        }
    }
    public void Clear()
    {
        this.childTransform.Clear();
    }
}
