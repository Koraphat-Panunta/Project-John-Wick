using UnityEngine;

public class Initialization : MonoBehaviour
{

    [SerializeField] protected MonoBehaviour[] initializedComponent;
    [SerializeField] public int initializedOrder;

    private int componentIndex = 0;

    public void Initialized()
    {
       
        try
        {
            for (int i = 0; i < initializedComponent.Length; i++)
            {
                componentIndex = i;
                Debug.Log("initializedObj = " + gameObject + " Initialized been corrupt comoinent index = " + componentIndex);
                if (initializedComponent[i] is IInitializedAble initializedAble)
                    initializedAble.Initialized();
                else
                    throw new System.Exception("InitializedAble not found index" + componentIndex);
            }
        }
        catch
        {
            throw new System.Exception("initializedOrder = "+initializedOrder + " Initialized been corrupt comoinent index = "+componentIndex);
        }
    }
}
