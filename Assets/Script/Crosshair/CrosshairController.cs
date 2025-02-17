using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrosshairController : MonoBehaviour,IObserverPlayer,IObserverPlayerSpawner
{
    //[SerializeField] WeaponSocket weaponSocket;
    [SerializeField] [Range(15,30)] private float MinAccuracy = 0;
    [SerializeField] [Range(0,100)] private float MaxAccuracy = 0;
    public RectTransform Crosshair_lineUp;
    public RectTransform Crosshair_lineDown;
    public RectTransform Crosshair_lineLeft;
    public RectTransform Crosshair_lineRight;
    public RectTransform Crosshair_CenterPosition;
    public RectTransform PointPosition;
    public Transform TargetAim;
    [SerializeField] public Player player;
    public bool isVisable = false;

    public CrosshairSpread CrosshairSpread { get; private set; }
    public CrosshiarShootpoint CrosshiarShootpoint { get; private set; }
    [SerializeField] public LayerMask layerMask;

    [SerializeField] private PlayerSpawner playerSpawner;
    private void Awake()
    {
        playerSpawner = FindAnyObjectByType<PlayerSpawner>();
        playerSpawner.AddObserverPlayerSpawner(this);
    }
    void Start()
    {
        CrosshairSpread = new CrosshairSpread(this);
        CrosshiarShootpoint = new CrosshiarShootpoint(this);
    }

    // UpdateNode is called once per frame
    void Update()
    {
        CrosshairUpdate();
    }
    float lerpSpeed = 10;
    void CrosshairUpdate()
    {
        if(TargetAim == null)
            return;

        Vector3 CrosshairPos;
        //CrosshairPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Crosshair_Position.position));
        CrosshairPos = Crosshair_CenterPosition.position;
        Ray ray = Camera.main.ScreenPointToRay(CrosshairPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = Vector3.Lerp(TargetAim.transform.position, worldPosition, lerpSpeed * Time.deltaTime);
        }
        else if (Physics.Raycast(ray, out hit, 1000, 1))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = Vector3.Lerp(TargetAim.transform.position, worldPosition, lerpSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 worldPosition = ray.GetPoint(100);
            TargetAim.transform.position = Vector3.Lerp(TargetAim.transform.position, worldPosition, lerpSpeed * Time.deltaTime);
        }
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
       
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {

        if(playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            CrosshairSpread.Performed(player.currentWeapon);
        }
        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            CrosshairSpread.Performed(player.currentWeapon);
        }
    }

    public void OnNotify(Player player)
    {
    }

    public void GetNotify(Player player)
    {
        this.player = player;
        player.AddObserver(this);
        player.crosshairController = this;
        TargetAim = player._aimPosRef;
    }
}
