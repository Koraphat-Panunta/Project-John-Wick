using System;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoTutorialPlayGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{
    public bool isPlaying;
    private VideoTutorialUI videoTutorialUI;
    public VideoTutorialPlayGameMasterNodeLeaf(
        InGameLevelGameMaster gameMaster
        , Func<bool> preCondition
        , VideoTutorialUI videoTutorialUI) 
        : base(gameMaster, preCondition)
    {
        this.videoTutorialUI = videoTutorialUI;
    }

    public override void Enter()
    {
        this.videoTutorialUI.gameObject.SetActive(true);
        base.Enter();
    }

    public override void FixedUpdateNode()
    {
       
    }
    public override void UpdateNode()
    {
        
    }

    public override void Exit()
    {
        this.videoTutorialUI.gameObject.SetActive(false);
        base.Exit();
    }

    public override bool IsComplete()
    {
        return false;
    }
    public void SetTextTutorial(TextMeshProUGUI tutorialText)
    {
        this.videoTutorialUI.SetTutorialMassage(tutorialText);
    }
    public void SetVideoPlayer(VideoPlayer videoPlayer)
    {
        this.videoTutorialUI.SetVideoPlayer(videoPlayer);
    }
}
