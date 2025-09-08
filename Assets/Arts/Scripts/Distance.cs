using UnityEngine;

public class Distance : MonoBehaviour
{
    public Transform target;
    public Renderer shadows;
    void Start()
    {

    }



    void Update()
    {
        if (target != null && shadows != null)
        {
            Vector3 pos = target.position;
            shadows.material.SetVector("_TargetPosition", pos);
        }
    }
}
