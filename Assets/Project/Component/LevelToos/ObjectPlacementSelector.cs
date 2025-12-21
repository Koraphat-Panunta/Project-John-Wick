using UnityEngine;

public class ObjectPlacementSelector : MonoBehaviour
{
    [SerializeField] private ObjectPlacement[] objectPlacements;
    [SerializeField] private int index = 0;

    [SerializeField] private GameObject curentObjPlace;
   
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

        curentObjPlace.transform.position = transform.position
            + (transform.forward * this.Current.positionOffset.z)
            + (transform.up * this.Current.positionOffset.y)
            + (transform.right * this.Current.positionOffset.x);

        curentObjPlace.transform.rotation = transform.rotation * Quaternion.Euler(this.Current.rotationEulerOffset);
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

}

[System.Serializable]
public struct ObjectPlacement
{
    public GameObject gameObject;
    public Vector3 positionOffset;
    public Vector3 rotationEulerOffset;
}
