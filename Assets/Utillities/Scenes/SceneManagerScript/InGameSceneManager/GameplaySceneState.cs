
using System.Collections.Generic;
using UnityEngine;

public class GameplaySceneState : SceneState
{
    private Canvas canvas;
    private InGameSceneManager sceneManager;
    public GameplaySceneState(SceneManager sceneManager) : base(sceneManager)
    {
        this.sceneManager = sceneManager as InGameSceneManager;
        this.canvas = this.sceneManager.gamePlayCanvas;
    }
    public override void Enter()
    {
        List<Enemy> enemies = sceneManager.enemies;
        Player player = sceneManager.player;
    
        canvas.enabled = true;
        foreach (Enemy enemy in enemies)
            enemy.enabled = true;

        player.enabled = true;
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
