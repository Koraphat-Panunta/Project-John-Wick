
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private List<PlayerInfoDisplay> playerInfoDisplays = new List<PlayerInfoDisplay>();
    [SerializeField] private TextMeshProUGUI AmmoDisplay;
    [SerializeField] private RawImage Hp_barPlayer;

    [SerializeField] private Player player;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInfoDisplays.Add(new PlayerWeaponDisplay(player, this, AmmoDisplay));
        playerInfoDisplays.Add(new PlayerAttributeDisplay(player, this, Hp_barPlayer));


        if (playerInfoDisplays.Count > 0)
        {
            foreach (PlayerInfoDisplay playerInfo in playerInfoDisplays)
            {
                playerInfo.AddPlayerObserver();
            }
        }
    }
    private void OnEnable()
    {
        if (playerInfoDisplays.Count > 0)
        {
            foreach (PlayerInfoDisplay playerInfo in playerInfoDisplays)
            {
                playerInfo.AddPlayerObserver();
            }
        }
    }
    private void OnDisable()
    {
        if (playerInfoDisplays.Count <= 0)
            return;
        
        foreach (PlayerInfoDisplay playerInfo in playerInfoDisplays)
        {
            playerInfo.RemovePlayerObserver();
        }
    }
}
