using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FrontSceneGameManagerNodeLeaf : GameManagerNodeLeaf
{
    public FrontSceneGameManagerNodeLeaf(string sceneName,GameManager gameManager, Func<bool> preCondition) : base(sceneName,gameManager, preCondition)
    {
    }

    public override void Enter()
    {
        try
        {
            if (gameManager.soundTrackManager.GetCurSoundTrack() != gameManager.soundTrackManager.openingTrack)
                gameManager.soundTrackManager.PlaySoundTrack(gameManager.soundTrackManager.openingTrack);
        }
        catch { }
        UnityEngine.SceneManagement.SceneManager.LoadScene(base.sceneName);
        base.Enter();
    }
    public override void Exit()
    {
        gameManager.soundTrackManager.StopSoundTrack(3);
        base.Exit();
    }
    public override void FixedUpdateNode()
    {
       
    }

    public override void UpdateNode()
    {
        
    }
}
