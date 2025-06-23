using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrontSceneGameMaster : GameMaster
{
    public Canvas splashScene;
    public Canvas menuScene;
    public Canvas settingScene;

    public MainMenuUICanvas mainMenuUICanvas;

    public TextMeshProUGUI titleGame;
    public Image fadeImage;

    public SplashSceneFrontSceneMasterNodeLeaf splashSceneFrontSceneMasterNodeLeaf;
    public MenuSceneFrontSceneMasterNodeLeaf menuSceneFrontSceneMasterNodeLeaf;

    protected override void Start()
    {
        base.Start();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector(this, () => true);

        splashSceneFrontSceneMasterNodeLeaf = new SplashSceneFrontSceneMasterNodeLeaf(this, () => splashSceneFrontSceneMasterNodeLeaf.isComplete == false);
        menuSceneFrontSceneMasterNodeLeaf = new MenuSceneFrontSceneMasterNodeLeaf(this, this.mainMenuUICanvas , ()=> true);

        startNodeSelector.AddtoChildNode(splashSceneFrontSceneMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(menuSceneFrontSceneMasterNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);

        nodeManagerBehavior.SearchingNewNode(this);
    }

    public override void UpdateNode() => nodeManagerBehavior.UpdateNode(this);
    public override void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);

}