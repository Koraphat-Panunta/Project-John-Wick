using UnityEngine;

public abstract class SceneManager : MonoBehaviour
{
    public SceneState curScene { get; protected set; }
    protected abstract void Start();
    protected virtual void Update()
    {
        if(curScene != null)
            curScene.Update();
    }
    protected virtual void FixedUpdate()
    {
        if(curScene != null)
            curScene.FixedUpdate();
    }
    public void ChangeScene(SceneState nextScene)
    {
        if (curScene != null)
            curScene.Exit();
        curScene = nextScene;
        curScene.Enter();
    }
    
}
