
using System;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : GameplayUI,IObserverPlayer,IPointerAble
{
    public Action<Vector3> crosshairLookPostion;

    //[SerializeField] WeaponSocket weaponSocket;
    public RectTransform Crosshair_lineUp;
    public RectTransform Crosshair_lineDown;
    public RectTransform Crosshair_lineLeft;
    public RectTransform Crosshair_lineRight;
    public RectTransform Crosshair_CenterPosition;
    public RectTransform PointPosition;

    public RectTransform crosshairBlock;

    private RectTransform crosshairRect;

    public Vector3 targetAimPaint { get; protected set; }
    public Vector3 targetAim { get; protected set; }
    [SerializeField] public Player player;
    public bool isVisable = false;

    public CrosshairSpread CrosshairSpread { get; private set; }
    public CrosshiarShootpoint CrosshiarShootpoint { get; private set; }

    public Vector3 pointerPos => player.transform.position;

    [SerializeField] public LayerMask layerMask;

    private Image _crosshairBlockImage;
    private Image _lineUpImage;
    private Image _lineDownImage;
    private Image _lineLeftImage;
    private Image _lineRightImage;

    public Vector2 crosshairBloomRange;
    public Vector2 crosshairPositionKickRange;
    public Vector2 crosshairBloomRecoveryRange;
    public Vector2 crosshairPositionRecoveryRange;

    public override void Initialized()
    {
        this.crosshairRect = GetComponent<RectTransform>();
        player.AddObserver(this);
        Cursor.lockState = CursorLockMode.Locked;
        CrosshairSpread = new CrosshairSpread(this);
        CrosshiarShootpoint = new CrosshiarShootpoint(this);

        _crosshairBlockImage = crosshairBlock.GetComponent<Image>();
        _lineUpImage = Crosshair_lineUp.GetComponent<Image>();
        _lineDownImage = Crosshair_lineDown.GetComponent<Image>();
        _lineLeftImage = Crosshair_lineLeft.GetComponent<Image>();
        _lineRightImage = Crosshair_lineRight.GetComponent<Image>();
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

        if(player != null)
        CrosshairSpread.CrosshairSpreadUpdate();
        if (player._currentWeapon == null || player._weaponManuverManager.aimingWeight <= 0)
        {
            this.DisableUI();
            return;
        }
        else
            this.EnableUI();
    }
    private void LateUpdate()
    {
        GetCrosshairUpdatePosition();
        calculateCrosshairBlock();
    }
    private float castDistance = 5;
    public Vector3 GetCrosshairUpdatePosition()
    {
        
        Vector3 CrosshairPos;
        //CrosshairPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Crosshair_Position.position));
        CrosshairPos = Crosshair_CenterPosition.position;
        Ray ray = Camera.main.ScreenPointToRay(CrosshairPos);
        RaycastHit hit;


        if (Physics.Raycast(ray, out hit, this.castDistance, layerMask,QueryTriggerInteraction.Ignore))
        {
            Vector3 worldPosition = hit.point;
            targetAimPaint = worldPosition;
            if (hit.collider.TryGetComponent<IGotPointingAble>(out IGotPointingAble gotPointingAble) && Vector3.Distance(player.transform.position, hit.point) < 24)
                gotPointingAble.NotifyPointingAble(this);
        }
        else if (Physics.Raycast(ray, out hit, this.castDistance, 1, QueryTriggerInteraction.Ignore))
        {
            Vector3 worldPosition = hit.point;
            targetAimPaint = worldPosition;
        }
        else
        {
            Vector3 worldPosition = ray.GetPoint(this.castDistance);
            targetAimPaint = worldPosition;
        }

        targetAim = ray.GetPoint(this.castDistance);

        if(crosshairLookPostion != null)
        crosshairLookPostion.Invoke(targetAim);
        return targetAimPaint;
    }
    private void calculateCrosshairBlock()
    {
        if(this.player._currentWeapon == null)
        {
            _crosshairBlockImage.enabled = false;
            return;
        }

        float castDistance = Vector3.Distance(this.player._currentWeapon.bulletSpawner.transform.position, targetAimPaint);
        Vector3 startCastPos = this.player._currentWeapon.bulletSpawner.transform.position;


        if (Physics.Raycast(startCastPos, (targetAimPaint - startCastPos).normalized, out RaycastHit hitInfo, castDistance, layerMask, QueryTriggerInteraction.Ignore)
            && Vector3.Distance(this.targetAimPaint, hitInfo.point) > .25f)
        {
            _crosshairBlockImage.enabled = true;
            this.crosshairBlock.transform.position = Camera.main.WorldToScreenPoint(hitInfo.point);
        }
        else
        {
            _crosshairBlockImage.enabled = false;
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

        if (player._currentWeapon == null || player._weaponManuverManager.aimingWeight <=0 )
            return;
        
        if (node is SubjectPlayer.NotifyEvent playerEvent)
        {

            if (playerEvent == SubjectPlayer.NotifyEvent.Firing)
            {
                CrosshairSpread.Performed(player._currentWeapon);
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
        _lineUpImage.enabled = true;
        _lineDownImage.enabled = true;
        _lineLeftImage.enabled = true;
        _lineRightImage.enabled = true;
        this.crosshairBlock.gameObject.SetActive(true);
    }
    public override void DisableUI()
    {
        _lineUpImage.enabled = false;
        _lineDownImage.enabled = false;
        _lineLeftImage.enabled = false;
        _lineRightImage.enabled = false;
        this.crosshairBlock.gameObject.SetActive(false);
    }

   
}
