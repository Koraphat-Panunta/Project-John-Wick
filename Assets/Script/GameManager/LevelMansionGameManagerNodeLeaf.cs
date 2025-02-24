using System;
using UnityEngine;

public class LevelMansionGameManagerNodeLeaf : GameManagerNodeLeaf
{
    public LevelMansionGameManagerNodeLeaf(string sceneName, GameManager gameManager, Func<bool> preCondition) : base(sceneName, gameManager, preCondition)
    {
    }
    public override void Enter()
    {
        gameManager.gamePlaySoundTrack = gameManager.soundTrackManager.theMansionTrack;
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        base.Enter();
    }

    public override void UpdateNode()
    {
        
    }
}
