using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSceneManager : SceneManager
{
    public Canvas gamePlayCanvas;
    public Canvas weaponCustomizedCanvas;

    public WeaponCustomizedSceneState weaponCustomizedSceneState;
    public GameplaySceneState gameplaySceneState;

    public List<Enemy> enemies = new List<Enemy>();
    public Player player;
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Start()
    {
        gamePlayCanvas.enabled = false;
        weaponCustomizedCanvas.enabled = false;

        weaponCustomizedSceneState = new WeaponCustomizedSceneState(this);
        gameplaySceneState = new GameplaySceneState(this);
        ChangeScene(weaponCustomizedSceneState);

        //enemies = FindObjectsOfType<Enemy>().ToList<Enemy>();
        //foreach (Enemy enemy in enemies)
        //    enemy.enabled = false;

        //player = FindAnyObjectByType<Player>();
        //player.enabled = false;

    }

    protected override void Update()
    {
        base.Update();
    }
    public void StartGamePlayScene()
    {
        ChangeScene(gameplaySceneState);
    }
}
