using UnityEngine;
using System.Collections.Generic;

public class GamePlayUICanvas : MonoBehaviour
{
    public List<GameplayUI> gameplayUI;
    public List<GameplayUI> GetGameplayUI()=> this.gameplayUI;

    public void EnableGameplayUI()
    {
        if(gameplayUI.Count <= 0)
            return;

        for(int i = 0; i < gameplayUI.Count; i++)
        {
            gameplayUI[i].EnableUI(); 
        }    
    }
    public void DisableGameplayUI()
    {
        if (gameplayUI.Count <= 0)
            return;

        for (int i = 0; i < gameplayUI.Count; i++)
        {
            gameplayUI[i].DisableUI();
        }
    }
}
