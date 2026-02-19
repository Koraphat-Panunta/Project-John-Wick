using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontSceneGameManagerNodeLeaf : GameManagerNodeLeaf
{
    public FrontSceneGameManagerNodeLeaf(string sceneName,GameManager gameManager, Func<bool> preCondition) : base(sceneName,gameManager, preCondition)
    {
    }

    public override void Enter()
    {
       
        base.Enter();
    }
    public override void Exit()
    {
        base.Exit();
    }
    public override void FixedUpdateNode()
    {
       
    }

    public override void UpdateNode()
    {
        
    }
}
