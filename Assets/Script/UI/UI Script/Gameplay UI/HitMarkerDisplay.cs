using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class HitMarkerDisplay : MonoBehaviour,IObserverPlayer,IGameplayUI
{
    [SerializeField] RawImage X_markker;
    [SerializeField] Player player;

    float markerWeight;
    enum ColorMarker
    {
        Red,
        White
    }
    private ColorMarker curColorMarker;    
    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if (playerAction == SubjectPlayer.PlayerAction.OppenentStagger)
        {
            X_markker.color = Color.white;
            curColorMarker = ColorMarker.White;
            markerWeight = 1;

        }

        if (playerAction == SubjectPlayer.PlayerAction.OpponentKilled)
        {
            X_markker.color = Color.red;
            curColorMarker = ColorMarker.Red;
            markerWeight = 1;

        }
    }

    public void OnNotify(Player player)
    {

    }

    private void Awake()
    {
        this.player.AddObserver(this);
    }
    // UpdateNode is called once per frame
    void Update()
    {
        Color color = X_markker.color;
        color.a = markerWeight;
        X_markker.color = color;

        markerWeight -= Time.deltaTime * 2;
    }
    private void OnValidate()
    {
        this.player = FindAnyObjectByType<Player>();
    }

    public void EnableUI()
    {
       this.X_markker.enabled = true;
    }

    public void DisableUI()
    {
        this.X_markker.enabled = false;
    }
}
