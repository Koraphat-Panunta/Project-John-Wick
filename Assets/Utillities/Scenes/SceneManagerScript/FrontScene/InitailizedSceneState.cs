using UnityEngine;

public class InitailizedSceneState : SceneState
{
    private Canvas canvas;
    private FrontSceneManager sceneManager;
    private DataBased dataBased;
    //Save
    public InitailizedSceneState(SceneManager sceneManager) : base(sceneManager)
    {
        this.sceneManager = sceneManager as FrontSceneManager;
        this.canvas = this.sceneManager.initailizedCanvas;
    }

    public override void Enter()
    {
        this.canvas.enabled = true;
    }

    public override void Exit()
    {
        this.canvas.enabled = false;
    }

    public override void FixedUpdate()
    {
       
    }

    public override void Update()
    {
        
    }
}
