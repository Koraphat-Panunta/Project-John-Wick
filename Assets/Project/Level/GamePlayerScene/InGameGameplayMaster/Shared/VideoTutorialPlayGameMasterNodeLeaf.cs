using System;
using UnityEngine;
using UnityEngine.Video;

public class VideoTutorialPlayGameMasterNodeLeaf : InGameLevelGameMasterNodeLeaf<InGameLevelGameMaster>
{
    public VideoPlayer videoPlayer { get; protected set; }
    public bool isPlaying;
    public GameObject videoPlayerObject;
    public VideoTutorialPlayGameMasterNodeLeaf(InGameLevelGameMaster gameMaster,VideoPlayer videoPlayer,GameObject videoPlayerObject, Func<bool> preCondition) : base(gameMaster, preCondition)
    {
        this.videoPlayer = videoPlayer;
        this.videoPlayerObject = videoPlayerObject;
    }

    public override void Enter()
    {
        this.videoPlayer.Play();
        this.videoPlayerObject.SetActive(true);
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
        this.videoPlayer.Stop();
        this.videoPlayerObject.SetActive(false);
        base.Exit();
    }

    public override bool IsComplete()
    {
        return false;
    }
    public void SetVideoPlayer(VideoPlayer videoPlayer)
    {
        this.videoPlayer = videoPlayer;
    }
}
