using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrontSceneGameMaster : GameMaster
{
    public Canvas splashScene;
    public Canvas menuScene;
    public Canvas settingScene;

    public TextMeshProUGUI titleGame;
    public Image fadeImage;

    public SplashSceneFrontSceneMasterNodeLeaf splashSceneFrontSceneMasterNodeLeaf;
    public MenuSceneFrontSceneMasterNodeLeaf menuSceneFrontSceneMasterNodeLeaf;

    public enum FrontSceneState
    {
        splash,
        menu,
        setting,
    }
    public FrontSceneState frontSceneState;

    protected override void Start()
    {
        this.frontSceneState = FrontSceneState.splash;
        base.Start();
    }

    public override void InitailizedNode()
    {
        startNodeSelector = new GameMasterNodeSelector(this, () => true);

        splashSceneFrontSceneMasterNodeLeaf = new SplashSceneFrontSceneMasterNodeLeaf(this, () => this.frontSceneState == FrontSceneState.splash);
        menuSceneFrontSceneMasterNodeLeaf = new MenuSceneFrontSceneMasterNodeLeaf(this, ()=> this.frontSceneState == FrontSceneState.menu);

        startNodeSelector.AddtoChildNode(splashSceneFrontSceneMasterNodeLeaf);
        startNodeSelector.AddtoChildNode(menuSceneFrontSceneMasterNodeLeaf);

        startNodeSelector.FindingNode(out INodeLeaf nodeLeaf);

        curNodeLeaf = nodeLeaf;
        curNodeLeaf.Enter();
    }

    public override void UpdateNode() => nodeManagerBehavior.UpdateNode(this);
    public override void FixedUpdateNode() => nodeManagerBehavior.FixedUpdateNode(this);

}