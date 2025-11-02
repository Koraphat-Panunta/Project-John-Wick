using UnityEngine;

[DefaultExecutionOrder(-500)]
public class EntitiesLayer : MonoBehaviour
{
    public static EntitiesLayer entitiesLayer;
    private void Awake()
    {
        entitiesLayer = this;
    }
}
