using UnityEngine;

public abstract class SceneManager : MonoBehaviour
{
    public SceneState curScene;
    protected virtual void Start()
    {
        
    }
    protected void Update()
    {
        
    }
    protected void FixedUpdate()
    {
        
    }
    public void ChangeScene(SceneState nextScene)
    {
        if (curScene != null)
            curScene.Exit();
        curScene = nextScene;
        curScene.Enter();
    }
    
}
