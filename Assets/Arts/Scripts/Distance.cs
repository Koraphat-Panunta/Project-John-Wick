using UnityEngine;

public class Distance : MonoBehaviour
{
    public Renderer light;

    public Camera cam;
    void Start()
    {
        cam = Camera.main;
        light = this.gameObject.GetComponent<Renderer>();
    }



    void Update()
    {
        if (cam != null && cam != null)
        {
            Vector3 pos = cam.transform.position;
            light.material.SetVector("_TargetPosition", pos);
        }
    }
}
