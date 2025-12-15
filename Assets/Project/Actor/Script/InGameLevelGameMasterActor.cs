using UnityEngine;

public class InGameLevelGameMasterActor : Actor
{
    [SerializeField] protected InGameLevelGameMaster inGameLevelGameMaster;

    public void CompleteLevel()
    {
        this.inGameLevelGameMaster.CompleteLevel();
    }

    public void DisableGameplay()
    {
        this.inGameLevelGameMaster.isActiveFreeState = true;
    }

    public void EnebaleGameplay()
    {
        this.inGameLevelGameMaster.isActiveFreeState = false;
    }
    private void OnValidate()
    {
        if(inGameLevelGameMaster == null)
        {
            this.inGameLevelGameMaster = FindAnyObjectByType<InGameLevelGameMaster>();
        }
    }
}
