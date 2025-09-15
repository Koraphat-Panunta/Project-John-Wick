using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class GameInitializer : MonoBehaviour
{
    [SerializeField] protected List<Initialization> gameObjInitialzied;
    [SerializeField] protected List<int> initializedOrder;

    private void Awake()
    {
        if(gameObjInitialzied.Count > 0)
        for(int i = 0;i < gameObjInitialzied.Count; i++)
        {
            gameObjInitialzied[i].Initialized();
        }
    }
    private void OnValidate()
    {
        // Find all Initialization components in the scene
        Initialization[] initializations = FindObjectsByType<Initialization>(FindObjectsInactive.Include,FindObjectsSortMode.None);

        // Sort them by initializedOrder (ascending)
        gameObjInitialzied = initializations
            .OrderBy(init => init.initializedOrder).ToList();

        initializedOrder = new List<int>();
        initializedOrder.Clear();
        for(int i = 0; i < gameObjInitialzied.Count; i++)
        {
            initializedOrder.Add(gameObjInitialzied[i].initializedOrder);
        }
           
    }

}
