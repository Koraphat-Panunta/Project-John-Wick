using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class HitMarkerDisplay : MonoBehaviour,IObserverPlayer,IObserverPlayerSpawner
{
    [SerializeField] RawImage X_markker;
    [SerializeField] Player player;

    [SerializeField] PlayerSpawner playerSpawner;

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
        playerSpawner = FindAnyObjectByType<PlayerSpawner>();
        this.playerSpawner.AddObserverPlayerSpawner(this);
    }
    private void Start()
    {
       
    }

    // UpdateNode is called once per frame
    void Update()
    {
        Color color = X_markker.color;
        color.a = markerWeight;
        X_markker.color = color;

        markerWeight -= Time.deltaTime * 2;
    }

    public void GetNotify(Player player)
    {
        this.player = player;
        this.player.AddObserver(this);
    }
}
