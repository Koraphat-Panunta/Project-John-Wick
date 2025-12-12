using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

[RequireComponent (typeof(BoxCollider))]
public class TriggerBox : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;

    [SerializeField] private MonoBehaviour beenCalledEvent;

    [SerializeField] private UnityEvent onTriggerEventEnterOnce;
    [SerializeField] private UnityEvent onTriggerEventEnter;
    [SerializeField] private UnityEvent onStayEvent;
    [SerializeField] private UnityEvent onTriggerEventExitOnce;
    [SerializeField] private UnityEvent onTriggerEventExit;

    public Action<Collider,TriggerBox> onTriggerBoxEnterEvent { get; protected set; }
    public void AddTriggerBoxEvent(Action<Collider, TriggerBox> actionEvent)
    {
        this.onTriggerBoxEnterEvent += actionEvent;
    }
    public void RemoveTriggerBoxEvent(Action<Collider, TriggerBox> actionEvent)
    {
        this.onTriggerBoxEnterEvent -= actionEvent;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (onTriggerBoxEnterEvent != null)
            onTriggerBoxEnterEvent.Invoke(other,this);

        if(onTriggerEventEnterOnce != null)
        {
            onTriggerEventEnterOnce.Invoke();
            onTriggerEventEnterOnce = null;
        }

        if(onTriggerEventEnter != null)
            onTriggerEventEnter.Invoke();
    }
    private void OnTriggerStay(Collider other)
    {
        if (onStayEvent != null)
            this.onStayEvent.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (onTriggerEventExitOnce != null)
        {
            onTriggerEventExitOnce.Invoke();
            onTriggerEventExitOnce = null;
        }

        if (onTriggerEventExit != null)
            onTriggerEventExit.Invoke();
    }
    private void OnValidate()
    {
        if (this.boxCollider == null)
            TryGetComponent<BoxCollider>(out this.boxCollider);
    }
    OnDrawGizmosTriggerEvent onDrawGizmosTriggerEvent = new OnDrawGizmosTriggerEvent();
    private void OnDrawGizmos()
    {
        if(this.boxCollider == null)
            return;



        Gizmos.color = Color.yellow * 0.35f;

        // Save matrix state
        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw cube at collider center, with collider size
        Gizmos.DrawCube(this.boxCollider.center, this.boxCollider.size);

        // Reset matrix (optional, good practice if you draw multiple gizmos)
        Gizmos.matrix = Matrix4x4.identity;



        onDrawGizmosTriggerEvent.DrawGizmosEvent(
            this.transform.position
            , onTriggerEventEnterOnce
            ,nameof(onTriggerEventEnterOnce) 
            ,Color.green);

        onDrawGizmosTriggerEvent.DrawGizmosEvent(
            this.transform.position
            , onTriggerEventEnter
            ,nameof(onTriggerEventEnter)
            , Color.green);
        onDrawGizmosTriggerEvent.DrawGizmosEvent(
            this.transform.position
            , onStayEvent
            ,nameof(onStayEvent)
            , Color.white);
        onDrawGizmosTriggerEvent.DrawGizmosEvent(
            this.transform.position
            , onTriggerEventExitOnce
            ,nameof(onTriggerEventExitOnce)
            , Color.red);
        onDrawGizmosTriggerEvent.DrawGizmosEvent(
            this.transform.position
            , onTriggerEventExit
            ,nameof(onTriggerEventExit)
            , Color.red);
    }
}
