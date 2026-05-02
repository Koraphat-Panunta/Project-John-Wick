using UnityEngine;

public class PhysicUpdateManager : MonoBehaviour,IInitializedAble
{
    public void Initialized()
    {
        Physics.simulationMode = SimulationMode.Script;
    }

    readonly float physicsStep = 0.02f;
    float physicsAccumulator = 0f;
    // Update is called once per frame

    private void Update()
    {
        this.physicsAccumulator += Time.deltaTime;
        this.UpdatePhysic();
    }
   
   
    private void UpdatePhysic()
    {
        float timeStep = this.physicsStep * Mathf.Clamp(Time.timeScale,.25f, Time.timeScale); 

        if (this.physicsAccumulator < (timeStep))
            return;

        this.physicsAccumulator = 0;


        Physics.Simulate(timeStep);

    }
 
}
