using System;
using UnityEngine;

public class LevelHotelGameManagerNodeLeaf : GameManagerNodeLeaf
{
    public LevelHotelGameManagerNodeLeaf(string sceneName, GameManager gameManager, Func<bool> preCondition) : base(sceneName, gameManager, preCondition)
    {
    }
    public override void Enter()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        base.Enter();
    }
    public override void UpdateNode()
    {
        
    }
}
