using UnityEditor;
using UnityEngine;

public abstract class VirtualVariable : MonoBehaviour
{
    [SerializeField] protected string variableName;
    [SerializeField] public Color color;
    [SerializeField] protected bool isEnableDrawGizmos;
    
    protected virtual void OnValidate()
    {
        this.variableName = gameObject.name;
    }
    protected OnDrawGizmosTriggerEvent onDrawGizmosTriggerEvent = new OnDrawGizmosTriggerEvent();

    [SerializeField] Mesh gizmosMesh;
    public float drawNameDistance = 10;
    protected virtual void OnDrawGizmos()
    {
        if(isEnableDrawGizmos == false)
            return;

        Gizmos.color = color;
        Gizmos.DrawMesh(this.gizmosMesh, this.transform.position, this.transform.rotation * Quaternion.Euler(45,0,0), new Vector3(.1f,.15f,.15f));
        Gizmos.color = Color.black * .65f;
        Gizmos.DrawWireMesh(this.gizmosMesh, this.transform.position, this.transform.rotation * Quaternion.Euler(45, 0, 0), new Vector3(.1f, .15f, .15f));

        Vector3 cameraPos;
        if (Application.isPlaying)
        {
            cameraPos = Camera.main.transform.position;
        }
        else
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }
        if (Vector3.Distance(cameraPos, this.transform.position) < drawNameDistance)
        {
            Handles.Label(transform.position + (Vector3.up * .15f), this.name);
            Gizmos.color = Color.black;
            Gizmos.DrawRay(transform.position , (Vector3.up * .15f));
        }
    }
}
