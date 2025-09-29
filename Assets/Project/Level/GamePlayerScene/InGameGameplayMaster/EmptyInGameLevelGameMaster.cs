using System;
using UnityEngine;

public class EmptyInGameLevelGameMaster : InGameLevelGameMaster
{
    
    private EmptyInGameLevel__Gameplay_GameMasterNodeLeaf emptyInGameLevel_Gameplay_Nodeleaf { get; set; }
    [SerializeField] private Camera cameraMain;


    public override void Initialized()
    {
        base.Initialized();
    }


    protected void LateUpdate()
    {
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(() => true);
        menuInGameGameMasterNodeLeaf = new MenuInGameGameMasterNodeLeaf(this, this.pauseCanvasUI, () => menuInGameGameMasterNodeLeaf.isMenu);
        emptyInGameLevel_Gameplay_Nodeleaf = new EmptyInGameLevel__Gameplay_GameMasterNodeLeaf(this, () => true);

        startNodeSelector.AddtoChildNode(menuInGameGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(emptyInGameLevel_Gameplay_Nodeleaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }
    private class EmptyInGameLevel__Gameplay_GameMasterNodeLeaf : InGameLevelGamplayGameMasterNodeLeaf<EmptyInGameLevelGameMaster>
    {
        public EmptyInGameLevel__Gameplay_GameMasterNodeLeaf(EmptyInGameLevelGameMaster gameMaster, Func<bool> preCondition) : base(gameMaster, preCondition)
        {

        }
        public override void Enter()
        {

            base.Enter();
        }
        public override void FixedUpdateNode()
        {
            base.FixedUpdateNode();
        }
        public override void UpdateNode()
        {

            base.UpdateNode();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void RestartCheckPoint()
        {
            throw new NotImplementedException();
        }
    }



}
