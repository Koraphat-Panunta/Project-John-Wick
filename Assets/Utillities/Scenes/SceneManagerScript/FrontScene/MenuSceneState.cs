using UnityEngine;

public class MenuSceneState : SceneState
{
    private FrontSceneManager sceneManager;
    private Canvas canvas;
    public MenuSceneState(SceneManager sceneManager) : base(sceneManager)
    {
        this.sceneManager = sceneManager as FrontSceneManager;
        this.canvas = this.sceneManager.menuCanvas;
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
