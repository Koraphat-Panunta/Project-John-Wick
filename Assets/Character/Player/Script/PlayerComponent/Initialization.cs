using UnityEngine;

public class Initialization : MonoBehaviour, IInitializedAble
{

    [SerializeField] protected MonoBehaviour[] initializedComponent;

    public void Initialized()
    { 

        try
        {
            for (int i = 0; i < initializedComponent.Length; i++)
            {
                if (initializedComponent[i].TryGetComponent(out IInitializedAble initializedAble))
                    initializedAble.Initialized();
                else
                    throw new System.Exception("Player InitializedAble not found");
            }
        }
        catch
        {
            throw new System.Exception("Player Initialized been corrupt");
        }
    }
}
