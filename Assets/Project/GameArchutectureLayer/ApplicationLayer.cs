using UnityEngine;

[DefaultExecutionOrder(-500)]
public class ApplicationLayer : MonoBehaviour
{
    public static ApplicationLayer applicationLayer;
    private void Awake()
    {
        applicationLayer = this;
    }
}
