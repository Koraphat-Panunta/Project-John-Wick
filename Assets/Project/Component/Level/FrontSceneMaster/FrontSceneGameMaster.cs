using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrontSceneGameMaster : GameMaster
{

    public SplashUICanvas splashCanvas;
    public MainMenuUICanvas mainMenuUICanvas;
    public OptionUICanvas optionUICanvas;

    public TextMeshProUGUI titleGame;
    public Image fadeImage;

    public SplashSceneFrontSceneMasterNodeLeaf splashSceneFrontSceneMasterNodeLeaf;
    public MenuSceneFrontSceneMasterNodeLeaf menuSceneFrontSceneMasterNodeLeaf;
    public OptionMenuSettingInGameGameMasterNodeLeaf optionMenuSettingInGameMasterNodeLeaf;

 
    public override void InitailizedNode()
    {
        this.startNodeSelector = new NodeSelector(() => true);

        this.splashSceneFrontSceneMasterNodeLeaf = new SplashSceneFrontSceneMasterNodeLeaf(this
            , this.splashCanvas
            , () => splashSceneFrontSceneMasterNodeLeaf.isComplete == false
            );

        this.optionMenuSettingInGameMasterNodeLeaf = new OptionMenuSettingInGameGameMasterNodeLeaf(this
            ,this.optionUICanvas
            ,()=> this.optionMenuSettingInGameMasterNodeLeaf.isTriggerEnter 
            || this.menuSceneFrontSceneMasterNodeLeaf.isTriggerOption
            );

        this.menuSceneFrontSceneMasterNodeLeaf = new MenuSceneFrontSceneMasterNodeLeaf(this, this.mainMenuUICanvas 
            , ()=> true
            );

        this.startNodeSelector.AddtoChildNode(splashSceneFrontSceneMasterNodeLeaf);
        this.startNodeSelector.AddtoChildNode(this.optionMenuSettingInGameMasterNodeLeaf);
        this.startNodeSelector.AddtoChildNode(menuSceneFrontSceneMasterNodeLeaf);

        _nodeManagerBehavior.SearchingNewNode(this);
    }

    public override void UpdateNode() => _nodeManagerBehavior.UpdateNodeAndCheckFindingNode(this);
    public override void FixedUpdateNode() => _nodeManagerBehavior.FixedUpdateNode(this);

}