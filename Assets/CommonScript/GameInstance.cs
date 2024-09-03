using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    // Start is called before the first frame update
    static public double GameTime;
    static public double FixedGameTime;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameTime += 1;
    }
    void FixedUpdate()
    {
        FixedGameTime += 1;
    }
}
