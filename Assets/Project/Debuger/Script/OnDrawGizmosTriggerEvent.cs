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

    int numberOfSphere = 4;

    public bool isDrawEnable;

    public void DrawGizmosEvent(
        Transform triggerEvent,
        UnityEvent unityEvent,
        Color color
       )
    {
        if(isDrawEnable == false)
            return;

        if (unityEvent == null) return;

        int count = unityEvent.GetPersistentEventCount();


        Gizmos.color = color;

        for (int i = 0; i < count; i++)
        {
            Object target = unityEvent.GetPersistentTarget(i);

            if (target is MonoBehaviour mb
                && target != null)
            {
                Vector3 targetPos = mb.transform.position;

                // Draw line
                //Gizmos.color = color;
                //Gizmos.DrawCube(targetPos,Vector3.one * .25f);
                // Draw labels
                Vector3 unityEventNamePos = targetPos + ((triggerEvent.position - targetPos).normalized * 1.5f) + Vector3.up * 0.3f;
                Handles.Label(unityEventNamePos, unityEvent.GetPersistentMethodName(i));

                Gizmos.DrawRay(unityEventNamePos,Vector3.down * .3f);
                DrawSphere(triggerEvent.position, targetPos, color);

                // Draw moving sphere along the line
            }
        }
    }

   
    public void DrawSphere(Vector3 triggerPosition,Vector3 targetPos,Color color)
    {
        if (isDrawEnable == false)
            return;

        t += Time.deltaTime * speed;
        if (t > 1f) t = 0f;

        for (int n = 0; n < numberOfSphere; n++)
        {
            float l = ((float)n * (1f / (float)numberOfSphere)) + (t * (1f / (float)numberOfSphere));
            Vector3 spherePos = Vector3.Lerp(triggerPosition, targetPos, l);
            Gizmos.color = color * .75f;
            Gizmos.DrawSphere(spherePos, sphereSize);
            Gizmos.DrawLine(triggerPosition, targetPos);
        }
    }
}
