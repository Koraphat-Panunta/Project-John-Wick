using UnityEngine;

[DefaultExecutionOrder(-500)]
public class Local_InfrastructureLayer : MonoBehaviour
{
    public static Local_InfrastructureLayer localInfrastructureLayer;
    private void Awake()
    {
        localInfrastructureLayer = this;
    }
}
