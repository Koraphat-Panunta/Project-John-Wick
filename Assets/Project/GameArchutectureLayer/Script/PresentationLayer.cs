using UnityEngine;

[DefaultExecutionOrder(-500)]
public class PresentationLayer : MonoBehaviour
{
    public static PresentationLayer presentationLayer;
    private void Awake()
    {
        presentationLayer = this;
    }
}
