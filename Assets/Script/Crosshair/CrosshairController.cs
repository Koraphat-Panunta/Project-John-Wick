using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class CrosshairController : MonoBehaviour,IObserverPlayer,IPointerAble
{
    //[SerializeField] WeaponSocket weaponSocket;
    public RectTransform Crosshair_lineUp;
    public RectTransform Crosshair_lineDown;
    public RectTransform Crosshair_lineLeft;
    public RectTransform Crosshair_lineRight;
    public RectTransform Crosshair_CenterPosition;
    public RectTransform PointPosition;
    public Transform TargetAim => player._aimPosRef;
    [SerializeField] public Player player;
    public bool isVisable = false;

    public CrosshairSpread CrosshairSpread { get; private set; }
    public CrosshiarShootpoint CrosshiarShootpoint { get; private set; }

    public Vector3 pointerPos => player.transform.position;

    [SerializeField] public LayerMask layerMask;
    

    private void Awake()
    {
        player.AddObserver(this);
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
        if(player != null)
        CrosshairSpread.CrosshairSpreadUpdate();
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
            if (hit.collider.TryGetComponent<IGotPointingAble>(out IGotPointingAble gotPointingAble) && Vector3.Distance(player.transform.position, hit.point) < 24)
                gotPointingAble.NotifyPointingAble(this);
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
    private void OnValidate()
    {
        player = FindAnyObjectByType<Player>();
    }

    public void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(player._currentWeapon == null)
        {
            this.Crosshair_CenterPosition.gameObject.SetActive(false);
            if (playerAction == SubjectPlayer.PlayerAction.LowReady)
            {
                CrosshairSpread.TriggerFocusSpanRate();
                CrosshairSpread.isAiming = false;
            }
            return;
        }
        this.Crosshair_CenterPosition.gameObject.SetActive(true);

        if (playerAction == SubjectPlayer.PlayerAction.Firing)
        {
            CrosshairSpread.Performed(player._currentWeapon);
            CrosshairSpread.CrosshairKickUp(player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilController);
        }
        if(playerAction == SubjectPlayer.PlayerAction.SwitchWeapon)
        {
            CrosshairSpread.Performed(player._currentWeapon);
        }

        if(playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            CrosshairSpread.Performed(35);
            CrosshairSpread.TriggerFocusSpanRate();
        }

        if (player._currentWeapon.currentEventNode is IReloadNode)
            CrosshairSpread.TriggerFocusSpanRate();
        
        if(playerAction == SubjectPlayer.PlayerAction.Aim)
            CrosshairSpread.isAiming = true;
        if (playerAction == SubjectPlayer.PlayerAction.LowReady)
        {
            CrosshairSpread.TriggerFocusSpanRate();
            CrosshairSpread.isAiming = false;
        }
    }

    public void OnNotify(Player player)
    {
    }

   
    public void EnableCrosshairVisable()
    {
        this.Crosshair_lineUp.GetComponent<RawImage>().enabled = true;
        this.Crosshair_lineDown.GetComponent<RawImage>().enabled = true;
        this.Crosshair_lineLeft.GetComponent<RawImage>().enabled = true;
        this.Crosshair_lineRight.GetComponent<RawImage>().enabled = true;
    }
    public void DisableCrosshairVisable() 
    {
        this.Crosshair_lineUp.GetComponent<RawImage>().enabled = false;
        this.Crosshair_lineDown.GetComponent<RawImage>().enabled = false;
        this.Crosshair_lineLeft.GetComponent<RawImage>().enabled = false;
        this.Crosshair_lineRight.GetComponent<RawImage>().enabled = false;
    }
}
