using UnityEngine;
using UnityEngine.Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OnDrawGizmosTriggerEvent
{
    float t = 0;

    float sphereSize = 0.06f;
    float speed = .25f;

    int numberOfSphere = 7;

    public void DrawGizmosEvent(
        Vector3 triggerPosition,
        UnityEvent unityEvent,
        string triggerEventName,
        Color color
       )
    {
        if (unityEvent == null) return;

        // animate 0 → 1 → loop
        t += Time.deltaTime * speed;
        if (t > 1f) t = 0f;

        int count = unityEvent.GetPersistentEventCount();
        Gizmos.color = color;

        for (int i = 0; i < count; i++)
        {
            Object target = unityEvent.GetPersistentTarget(i);

            if (target is MonoBehaviour mb)
            {
                Vector3 targetPos = mb.transform.position;

                // Draw line
                Gizmos.DrawLine(triggerPosition, targetPos);

#if UNITY_EDITOR
                // Draw labels
                Handles.Label(Vector3.Lerp(triggerPosition, targetPos, .4f) + Vector3.up * 0.2f, triggerEventName);
                Handles.Label(targetPos + Vector3.up * 0.2f, unityEvent.GetPersistentMethodName(i));
                for (int n = 0; n < numberOfSphere; n++)
                {
                    float l = ((float)n * (1f / (float)numberOfSphere)) + (t * (1f / (float)numberOfSphere));


                    Vector3 spherePos = Vector3.Lerp(triggerPosition, targetPos, l);
                    Gizmos.DrawSphere(spherePos, sphereSize);
                }
#endif

                // Draw moving sphere along the line


            }
        }
    }
}
