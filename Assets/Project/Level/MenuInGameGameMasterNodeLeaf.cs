using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class MenuInGameGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>,INodeLeafTransitionAble
{

    private PauseUICanvas pauseUICanvas;
    public bool isMenu { get; protected set; }
    public bool isTriggerToSetting { get; set; }
    public INodeManager nodeManager { get; set; }
    public Dictionary<INode, bool> transitionAbleNode { get; set; }
    public NodeLeafTransitionBehavior nodeLeafTransitionBehavior { get; set; }

    public MenuInGameGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,PauseUICanvas pauseUICanvas, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.pauseUICanvas = pauseUICanvas;
        gameMaster.user.userInput.PauseAction.PauseTrigger.performed += this.TriggerPause;
        pauseUICanvas.resume.onClick.AddListener(() => 
        {
            Debug.Log("OnClickResume");
            isMenu = false; 
        });
        pauseUICanvas.setting.onClick.AddListener(()=> isTriggerToSetting = true);
        pauseUICanvas.exit.onClick.AddListener(() => gameMaster.gameManager.ExitToMainMenu());
        
        this.nodeManager = gameMaster;
        this.transitionAbleNode = new Dictionary<INode, bool>();
        this.nodeLeafTransitionBehavior = new NodeLeafTransitionBehavior();
    }


    public override void Enter()
    {
        //Delay();
        Debug.Log("MenuInGameGameMasterNodeLeaf Enter");
        this.Pause();
        nodeLeafTransitionBehavior.TransitionAbleAll(this);
        base.Enter();
    }

    //private async void Delay()
    //{
    //    Time.timeScale = 0f;
    //    await Task.Delay((int)(1000f * 0.05f));
    //    Time.timeScale = 1f;
    //    isPause = false;

    //}
    public override void Exit()
    {
        Debug.Log("MenuInGameGameMasterNodeLeaf Exit");
        this.isTriggerToSetting = false;
        UnPause();
        base.Exit();
    }
    public override bool IsComplete()
    {
        return false;
    }
   
    public override void FixedUpdateNode()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public override void UpdateNode()
    {
        this.TransitioningCheck();
    }
    private void UnPause()
    {
        pauseUICanvas.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Pause()
    {
        pauseUICanvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
    private void TriggerPause(InputAction.CallbackContext context) // Call by user.PauseAction
    {
        if(isMenu == true)
            isMenu = false;
        else if(isMenu == false)
            isMenu = true;
    }

    public bool TransitioningCheck() => nodeLeafTransitionBehavior.TransitioningCheck(this);


    public void AddTransitionNode(INode node) => nodeLeafTransitionBehavior.AddTransistionNode(this, node);
    
}
