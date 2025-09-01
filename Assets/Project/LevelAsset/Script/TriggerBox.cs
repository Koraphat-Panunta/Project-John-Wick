using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent (typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
   
    public Action<Collider> onTriggerBoxEnterEvent { get; protected set; }
    public void AddTriggerBoxEvent(Action<Collider> actionEvent)
    {
        this.onTriggerBoxEnterEvent += actionEvent;
    }
    public void RemoveTriggerBoxEvent(Action<Collider> actionEvent)
    {
        this.onTriggerBoxEnterEvent -= actionEvent;
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter");
        if (onTriggerBoxEnterEvent != null)
            onTriggerBoxEnterEvent.Invoke(other);
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
