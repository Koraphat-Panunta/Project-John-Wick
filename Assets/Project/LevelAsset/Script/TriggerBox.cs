using System;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    public enum TriggerEvent
    {
        Enter,
        Stay,
        Exit
    }
    public Action<Collision, TriggerEvent> onTriggerBoxEvent { get; protected set; }
    public void AddTriggerBoxEvent(Action<Collision,TriggerEvent> actionEvent)
    {
        this.onTriggerBoxEvent += actionEvent;
    }
    public void RemoveTriggerBoxEvent(Action<Collision, TriggerEvent> actionEvent)
    {
        this.onTriggerBoxEvent -= actionEvent;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (onTriggerBoxEvent != null)
            onTriggerBoxEvent.Invoke(collision, TriggerEvent.Enter);
    }
    private void OnCollisionStay(Collision collision)
    {
        if (onTriggerBoxEvent != null)
            onTriggerBoxEvent.Invoke(collision, TriggerEvent.Stay);
    }
    private void OnCollisionExit(Collision collision)
    {
        if (onTriggerBoxEvent != null)
            onTriggerBoxEvent.Invoke(collision, TriggerEvent.Exit);
    }
    private void OnValidate()
    {
        if (this.boxCollider == null)
            TryGetComponent<BoxCollider>(out this.boxCollider);
    }

    private void OnDrawGizmos()
    {
        if(this.boxCollider == null)
            return;

        Gizmos.color = Color.yellow * 0.7f;

        // Save matrix state
        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw cube at collider center, with collider size
        Gizmos.DrawCube(this.boxCollider.center, this.boxCollider.size);

        // Reset matrix (optional, good practice if you draw multiple gizmos)
        Gizmos.matrix = Matrix4x4.identity;
    }
}
