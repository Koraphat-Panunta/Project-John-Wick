using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine.Video;
using TMPro;

public class PrologueLevelGameMaster : InGameLevelGameMaster
{
    [SerializeField] private OpeningUICanvas openingUICanvas;
    [SerializeField] private GameOverUICanvas gameOverUICanvas;
    [SerializeField] private MissionCompleteUICanvas missionCompleteUICanvas;

    public InGameLevelOpeningGameMasterNodeLeaf levelOpeningGameMasterNodeLeaf { get; protected set; }
    public InGameLevelCompleteGameMasterNodeLeaf levelMisstionCompleteGameMasterNodeLeaf { get; protected set; }
    public InGameLevelGameOverGameMasterNodeLeaf levelGameOverGameMasterNodeLeaf { get; protected set; }
    public InGameLevelDelayOpeningLoad delayOpeningGameMasterNodeLeaf { get ; protected set ; }
    protected InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster> prologueInGameLevelGameplayGameMasterNodeLeaf;

    public override void Initialized()
    {

        base.Initialized();
    }
    
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    

    public override void InitailizedNode()
    {
        startNodeSelector = new NodeSelector(()=>true,"PrologueStartNodeSelector");


        delayOpeningGameMasterNodeLeaf = new InGameLevelDelayOpeningLoad(this, () => base.isCompleteLoad == false);
        levelOpeningGameMasterNodeLeaf = new InGameLevelOpeningGameMasterNodeLeaf(this, openingUICanvas , () => levelOpeningGameMasterNodeLeaf.isComplete == false);
        levelGameOverGameMasterNodeLeaf = new InGameLevelGameOverGameMasterNodeLeaf(this, gameOverUICanvas, () => player.isDead);
        pausingSelector = new NodeSelector(() => this.menuInGameGameMasterNodeLeaf.isMenu);
        menuInGameGameMasterNodeLeaf = new MenuInGameGameMasterNodeLeaf(this, pauseCanvasUI, () => true);
        optionMenuSettingInGameGameMasterNode = new OptionMenuSettingInGameGameMasterNodeLeaf(this, optionCanvasUI, () => menuInGameGameMasterNodeLeaf.isTriggerToSetting);

        levelMisstionCompleteGameMasterNodeLeaf = new InGameLevelCompleteGameMasterNodeLeaf(this, missionCompleteUICanvas, () => base.isLevelComplete);
        prologueInGameLevelGameplayGameMasterNodeLeaf = new InGameLevelGamplayGameMasterNodeLeaf<PrologueLevelGameMaster>(this,()=> true);

        startNodeSelector.AddtoChildNode(delayOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelOpeningGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(levelGameOverGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(pausingSelector);
        startNodeSelector.AddtoChildNode(levelMisstionCompleteGameMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(prologueInGameLevelGameplayGameMasterNodeLeaf);

        pausingSelector.AddtoChildNode(optionMenuSettingInGameGameMasterNode);
        pausingSelector.AddtoChildNode(menuInGameGameMasterNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);

    }
}

