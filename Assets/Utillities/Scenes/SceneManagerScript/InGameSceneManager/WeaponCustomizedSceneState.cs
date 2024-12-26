using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class WeaponCustomizedSceneState : SceneState
{
    private Canvas canvas;
    private InGameSceneManager sceneManager;
    public WeaponCustomizedSceneState(SceneManager sceneManager) : base(sceneManager)
    {
        this.sceneManager = sceneManager as InGameSceneManager;
        this.canvas = this.sceneManager.weaponCustomizedCanvas;
    }
    public override void Enter()
    {
        canvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public override void Exit()
    {
        canvas.enabled = false;
        Cursor.lockState = CursorLockMode.Locked;

    }
    public override void Update()
    {
        
    }
    public override void FixedUpdate()
    {
        
    }

    
}
