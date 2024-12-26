using UnityEngine;

public class FrontSceneManager : SceneManager
{
    public Canvas menuCanvas;
    public Canvas levelSelectCanvas;

    public MenuSceneState menuSceneState;
    public LevelSceneState levelSceneState;
  
    protected override void Start()
    {
        menuCanvas.enabled = false;
        levelSelectCanvas.enabled = false;

        menuSceneState = new MenuSceneState(this);
        levelSceneState = new LevelSceneState(this);

        ChangeScene(menuSceneState);
        //curScene = menuSceneState;
        //curScene.Enter();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    protected override void Update()
    {
        base.Update();
    }
    public void ChangeScene(string frontScene)
    {
        switch (frontScene)
        {
            case "Menu":ChangeScene(menuSceneState); 
                break;
            case "LevelSelect":ChangeScene(levelSceneState);
                break;
        }
    }
}
