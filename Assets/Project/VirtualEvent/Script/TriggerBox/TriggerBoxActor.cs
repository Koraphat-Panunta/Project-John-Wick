using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;


[RequireComponent (typeof(BoxCollider))]
public class TriggerBoxActor : Actor
{
    public enum TriggerBoxEvent
    {
        Enter,
        Stay,
        Exit
    }

    [SerializeField] private BoxCollider boxCollider;

    

    private void OnTriggerEnter(Collider other)
    {
        this.NotifyObserver(TriggerBoxEvent.Enter);
    }
    private void OnTriggerStay(Collider other)
    {
        this.NotifyObserver(TriggerBoxEvent.Stay);
    }
    private void OnTriggerExit(Collider other)
    {
        this.NotifyObserver(TriggerBoxEvent.Exit);
    }
    private void OnValidate()
    {
        if (this.boxCollider == null)
            TryGetComponent<BoxCollider>(out this.boxCollider);
    }
    protected override void OnDrawGizmos()
    {
        if(this.boxCollider == null)
            return;

        Gizmos.color = color;

        // Save matrix state
        Gizmos.matrix = transform.localToWorldMatrix;

        // Draw cube at collider center, with collider size
        Gizmos.DrawCube(this.boxCollider.center, this.boxCollider.size);

        // Reset matrix (optional, good practice if you draw multiple gizmos)
        Gizmos.matrix = Matrix4x4.identity;

        if (this.isEnableGizmos == false)
            return;

        Vector3 cameraPos;
        if (Application.isPlaying)
        {
            cameraPos = Camera.main.transform.position;
        }
        else
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, this.transform.position) < base.drawNameDistance)
        {
            Handles.Label(transform.position + (Vector3.up * this.boxCollider.size.y), this.name);
        }
        Gizmos.DrawRay(transform.position, Vector3.up * this.boxCollider.size.y);

        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, this.boxCollider.size);

    }
}


