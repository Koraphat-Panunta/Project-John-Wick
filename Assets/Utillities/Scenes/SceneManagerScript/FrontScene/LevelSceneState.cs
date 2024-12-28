using UnityEngine;

public class LevelSceneState : SceneState
{
    private Canvas canvas;
    private FrontSceneManager sceneManager;
    public LevelSceneState(SceneManager sceneManager) : base(sceneManager)
    {
        this.sceneManager = sceneManager as FrontSceneManager;  
        this.canvas = this.sceneManager.levelSelectCanvas;
    }
    public override void Enter()
    {
        canvas.enabled = true;
    }

    public override void Exit()
    {
        canvas.enabled = false;
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Update()
    {
        
    }
}
