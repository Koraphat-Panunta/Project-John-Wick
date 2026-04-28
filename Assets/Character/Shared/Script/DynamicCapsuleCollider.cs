using UnityEngine;

public class DynamicCapsuleCollider : MonoBehaviour
{
    [SerializeField] CapsuleCollider capsuleCollider;
    [SerializeField] public Transform[] transformPoints;

    private void FixedUpdate()
    {
        UpdateCapsuleCollider();
    }

    private void UpdateCapsuleCollider()
    {
        if (capsuleCollider == null ||
            transformPoints == null ||
            transformPoints.Length == 0)
            return;


        // Initialize bounds
        Vector3 min = transformPoints[0].position;
        Vector3 max = transformPoints[0].position;

        // World-space bounds
        foreach (var t in transformPoints)
        {
            if (t == null) continue;

            Vector3 p = t.position;
            min = Vector3.Min(min, p);
            max = Vector3.Max(max, p);
        }

        // ---- HEIGHT ----
        float height = Mathf.Max(max.y - min.y, capsuleCollider.radius * 2f);

        // ---- CENTER (LOCAL SPACE) ----
        Vector3 worldCenter = new Vector3(
            (min.x + max.x) * 0.5f,
            min.y + height * 0.5f,
            (min.z + max.z) * 0.5f
        );

        Vector3 localCenter = transform.InverseTransformPoint(worldCenter);

       

        // ---- APPLY ----
        capsuleCollider.center = new Vector3(
            capsuleCollider.center.x
            ,localCenter.y
            ,capsuleCollider.center.z);
        capsuleCollider.height = height;

    }

    private void OnValidate()
    {
        if (Application.isPlaying)
            return;

        UpdateCapsuleCollider();
    }
}
