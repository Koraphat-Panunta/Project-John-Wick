using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private List<PlayerInfoDisplay> playerInfoDisplays = new List<PlayerInfoDisplay>();
    [SerializeField] private TextMeshProUGUI AmmoDisplay;
    [SerializeField] private RawImage Hp_barPlayer;
    // Start is called before the first frame update
    void Start()
    {
        playerInfoDisplays.Add(new PlayerWeaponDisplay(FindAnyObjectByType<Player>().GetComponent<Player>(), this,AmmoDisplay));
        playerInfoDisplays.Add(new PlayerAttributeDisplay(FindAnyObjectByType<Player>().GetComponent<Player>(), this, Hp_barPlayer));
        if (playerInfoDisplays.Count > 0)
        {
            foreach (PlayerInfoDisplay player in playerInfoDisplays)
            {
                player.AddPlayerObserver();
            }
        }
    }
    private void OnEnable()
    {
        if (playerInfoDisplays.Count > 0)
        {
            foreach (PlayerInfoDisplay player in playerInfoDisplays)
            {
                player.AddPlayerObserver();
            }
        }
    }
    private void OnDisable()
    {
        foreach (PlayerInfoDisplay player in playerInfoDisplays)
        {
            player.RemovePlayerObserver();
        }
    }
}
