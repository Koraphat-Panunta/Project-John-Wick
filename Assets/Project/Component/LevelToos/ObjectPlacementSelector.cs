using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPlacementSelector : MonoBehaviour
{
    [SerializeField] private ObjectPlacement[] objectPlacements;
    [SerializeField] private int index = 0;

    [SerializeField] private GameObject curentObjPlace;

    [SerializeField] protected Transform placeTransform;
    private void OnValidate()
    {
        
        if(curentObjPlace == null
            || curentObjPlace != this.Current.gameObject)
        {
            UpdateObj();
        }

    }

    public ObjectPlacement Current =>
        objectPlacements != null && objectPlacements.Length > 0
        ? objectPlacements[Mathf.Clamp(index, 0, objectPlacements.Length - 1)]
        : default;

    private void UpdateObj()
    {

        if(curentObjPlace 
            == this.Current.gameObject)
            return;

        if(curentObjPlace != null)
        curentObjPlace.gameObject.SetActive(false);
        curentObjPlace = this.Current.gameObject;
        curentObjPlace.gameObject.SetActive(true);

        curentObjPlace.transform.position = placeTransform.position
            + (placeTransform.forward * this.Current.positionOffset.z)
            + (placeTransform.up * this.Current.positionOffset.y)
            + (placeTransform.right * this.Current.positionOffset.x);

        curentObjPlace.transform.rotation = placeTransform.rotation * Quaternion.Euler(this.Current.rotationEulerOffset);
    }

    public void NextObject()
    {
        if (objectPlacements == null || objectPlacements.Length == 0) return;
        index = (index + 1) % objectPlacements.Length;

        UpdateObj();
    }

    public void BackObject()
    {
        if (objectPlacements == null || objectPlacements.Length == 0) return;
        index--;
        if (index < 0) index = objectPlacements.Length - 1;

        UpdateObj();
    }

   public void ClearUnSelected()
    {
        if(objectPlacements.Length <= 0)
            return;

        for (int i = 0; i < objectPlacements.Length; i++)
        {
            if (this.objectPlacements[i].gameObject != curentObjPlace)
                DestroyImmediate(this.objectPlacements[i].gameObject);
        }
    }
    private void OnDrawGizmos()
    {


        Vector3 cameraPos;
        if (Application.isPlaying)
        {
            cameraPos = Camera.main.transform.position;
        }
        else
        {
            cameraPos = SceneView.lastActiveSceneView.camera.transform.position;
        }

        if (Vector3.Distance(cameraPos, placeTransform.position) < 4.5f)
        {
            Handles.Label(placeTransform.position + (Vector3.up * .2f), "Select Prop");
        }
        if (Vector3.Distance(cameraPos, placeTransform.position) < 7.25f)
        {
            Gizmos.color = Color.white * .75f;
            Gizmos.DrawSphere(placeTransform.position, .1f);
        }

    }

}

[System.Serializable]
public struct ObjectPlacement
{
    public GameObject gameObject;
    public Vector3 positionOffset;
    public Vector3 rotationEulerOffset;
}
