using UnityEngine;

public class Initialization : MonoBehaviour
{

    [SerializeField] protected MonoBehaviour[] initializedComponent;
    [SerializeField] public int initializedOrder;

    private int componentIndex = 0;

    public void Initialized()
    {
        for (int i = 0; i < initializedComponent.Length; i++)
        {
            componentIndex = i;
            if (initializedComponent[i] is IInitializedAble initializedAble)
                initializedAble.Initialized();
            else
            {
                throw new System.Exception("InitializedAble not found index" + componentIndex);
            }
        }
    }
    private void OnValidate()
    {
        for (int i = 0; i < initializedComponent.Length; i++)
        {
            componentIndex = i;
            if (initializedComponent[i] is IInitializedAble == false)
            {
                throw new System.Exception("InitializedAble not found index" + initializedComponent[i]);
            }
        }
    }
}
