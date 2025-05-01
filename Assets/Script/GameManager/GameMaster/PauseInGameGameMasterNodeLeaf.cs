using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseInGameGameMasterNodeLeaf : GameMasterNodeLeaf<InGameLevelGameMaster>
{
    private PauseUICanvas pauseUICanvas;
    public bool isPause { get; protected set; }
    public PauseInGameGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,PauseUICanvas pauseUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.pauseUICanvas = pauseUICanvas;
        gameMaster.user.userInput.PauseAction.PauseTrigger.performed += this.TriggerPause;
    }

    public override void Enter()
    {
        isPause = false;
        this.Pause();
    }

    public override void Exit()
    {
        isPause = false;
        UnPause();
    }
    public override bool IsComplete()
    {
        return false;
    }
    public override bool IsReset()
    {
        return isPause;
    }
    public override void FixedUpdateNode()
    {

    }
    public override void UpdateNode()
    {

    }
    private void UnPause()
    {
        Time.timeScale = 1;
        pauseUICanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Pause()
    {
        Time.timeScale = 0;
        pauseUICanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    private void TriggerPause(InputAction.CallbackContext context) // Call by user.PauseAction
    {
        if(isPause == true)
            isPause = false;
        else if(isPause == false)
            isPause = true;
    }
}
