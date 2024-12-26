using UnityEngine;

public abstract class SceneState 
{
    public Canvas canvas;
    public SceneManager sceneManager;
    public SceneState(SceneManager sceneManager)
    {
        this.sceneManager = sceneManager;
    }
    public abstract void Enter();
    public abstract void Exit();

    public abstract void Update();

    public abstract void FixedUpdate();
   
}
