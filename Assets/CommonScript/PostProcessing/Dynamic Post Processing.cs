using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DynamicPostProcessing : MonoBehaviour,IObserverPlayer
{
    public Player player;
    public Vignette vignette;
    public Volume volume;
    public GetShootFeedBack_PostProcessing getShootFeedBack;

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            getShootFeedBack.TriggerFeedBack();
        }
    }

    void Start()
    {
        player = FindObjectOfType<Player>();
        player.AddObserver(this);
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            this.vignette = vignette;
        }
        getShootFeedBack = new GetShootFeedBack_PostProcessing(this);
    }
    private void OnDisable()
    {
        player.RemoveObserver(this);
    }

    public void OnNotify(Player player)
    {
    }
}
