using UnityEngine;

public class PhysicUpdateManager : MonoBehaviour,IInitializedAble
{
    public void Initialized()
    {
        Physics.simulationMode = SimulationMode.Script;
    }

    const float physicsStep = 0.02f;
    float physicsAccumulator = 0f;
    // Update is called once per frame
    private void FixedUpdate()
    {

        Debug.Log("Physic deltaTime = " + Time.deltaTime);
        Debug.Log("Physic fixDeltaTime = " + Time.fixedDeltaTime);
        Debug.Log("Physic timeScale = " + Time.timeScale);
        Debug.Log("Physic physicsStep * Time.timeScale = " + physicsStep * Time.timeScale);


        Physics.Simulate(physicsStep * Time.timeScale);
    }
   
 
}
