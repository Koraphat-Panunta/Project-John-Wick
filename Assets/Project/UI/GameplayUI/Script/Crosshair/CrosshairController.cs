
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : GameplayUI,IObserverPlayer,IPointerAble
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

    public override void Initialized()
    {
        player.AddObserver(this);
        Cursor.lockState = CursorLockMode.Locked;
        CrosshairSpread = new CrosshairSpread(this);
        CrosshiarShootpoint = new CrosshiarShootpoint(this);
    }
  
    void Start()
    {
        if (player._currentWeapon == null)
            this.DisableUI();
        else
            this.EnableUI();
    }

    void Update()
    {
        CrosshairUpdate();
        if(player != null)
        CrosshairSpread.CrosshairSpreadUpdate();
        if (player._currentWeapon == null || player.weaponAdvanceUser._weaponManuverManager.aimingWeight <= 0)
        {
            this.DisableUI();
            return;
        }
        else
            this.EnableUI();
    }
    float lerpSpeed = 25;
    void CrosshairUpdate()
    {
        if(TargetAim == null)
            return;

        Vector3 CrosshairPos;
        //CrosshairPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Crosshair_Position.position));
        CrosshairPos = Crosshair_CenterPosition.position;
        Ray ray = Camera.main.ScreenPointToRay(CrosshairPos);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, 10, layerMask,QueryTriggerInteraction.Ignore))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = worldPosition;
            if (hit.collider.TryGetComponent<IGotPointingAble>(out IGotPointingAble gotPointingAble) && Vector3.Distance(player.transform.position, hit.point) < 24)
                gotPointingAble.NotifyPointingAble(this);
        }
        else if (Physics.Raycast(ray, out hit, 10, 1, QueryTriggerInteraction.Ignore))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = worldPosition;
        }
        else
        {
            Vector3 worldPosition = ray.GetPoint(10);
            TargetAim.transform.position = worldPosition;
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

    
    public void OnNotify<T>(Player player, T node)
    {

        if (player._currentWeapon == null || player.weaponAdvanceUser._weaponManuverManager.aimingWeight <=0 )
            return;
        
        if (node is SubjectPlayer.NotifyEvent playerEvent)
        {

            if (playerEvent == SubjectPlayer.NotifyEvent.Firing)
            {
                CrosshairSpread.Performed(player._currentWeapon);
                CrosshairSpread.CrosshairKickUp(player._currentWeapon.RecoilKickBack - player._currentWeapon.RecoilController);
                //CrosshairSpread.TriggerFocusSpanRate();
            }


            if (playerEvent == SubjectPlayer.NotifyEvent.GetShoot)
            {
                CrosshairSpread.Performed(35);
                //CrosshairSpread.TriggerFocusSpanRate();
            }
        }
            
        else if (node is WeaponManuverLeafNode weaponManuverLeafNode)
            switch (weaponManuverLeafNode)
            {
                case LowReadyWeaponManuverNodeLeaf lowReadyWeaponManuverLeafNode:
                    {
                        //CrosshairSpread.TriggerFocusSpanRate();
                        CrosshairSpread.isAiming = false;
                        break;
                    }
                case AimDownSightWeaponManuverNodeLeaf aimDownSightWeaponManuverNodeLeaf:
                    {
                        CrosshairSpread.isAiming = true;
                        break;
                    }
                case ReloadMagazineFullStageNodeLeaf:
                case TacticalReloadMagazineFullStageNodeLeaf:
                case PrimaryToSecondarySwitchWeaponManuverLeafNode:
                case SecondaryToPrimarySwitchWeaponManuverLeafNode:
                case DrawPrimaryWeaponManuverNodeLeaf:
                case DrawSecondaryWeaponManuverNodeLeaf:
                case HolsterPrimaryWeaponManuverNodeLeaf:
                case HolsterSecondaryWeaponManuverNodeLeaf:
                    {
                        //CrosshairSpread.TriggerFocusSpanRate();
                        break;
                    }
            }
    }



    public override void EnableUI()
    {
        this.Crosshair_lineUp.GetComponent<RawImage>().enabled = true;
        this.Crosshair_lineDown.GetComponent<RawImage>().enabled = true;
        this.Crosshair_lineLeft.GetComponent<RawImage>().enabled = true;
        this.Crosshair_lineRight.GetComponent<RawImage>().enabled = true;
    }
    public override void DisableUI() 
    {
        this.Crosshair_lineUp.GetComponent<RawImage>().enabled = false;
        this.Crosshair_lineDown.GetComponent<RawImage>().enabled = false;
        this.Crosshair_lineLeft.GetComponent<RawImage>().enabled = false;
        this.Crosshair_lineRight.GetComponent<RawImage>().enabled = false;
    }

   
}
