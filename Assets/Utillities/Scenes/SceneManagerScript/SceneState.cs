using UnityEngine;

public abstract class SceneState 
{

    public SceneState(SceneManager sceneManager)
    {
        
    }
    public abstract void Enter();
    public abstract void Exit();

    public abstract void Update();

    public abstract void FixedUpdate();
   
}
