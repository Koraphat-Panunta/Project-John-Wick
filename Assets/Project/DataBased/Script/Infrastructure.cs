using UnityEngine;

public class Infrastructure : MonoBehaviour, IInitializedAble
{
    public static Infrastructure Instance;
    public void Initialized()
    {
        if (Infrastructure.Instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
    }
}
