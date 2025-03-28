using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
public class HittedIndicator : MonoBehaviour, IObserverPlayer,IGameplayUI
{
    [SerializeField] Player player;
    [SerializeField] public RectTransform uiScreenCanvas;
    [SerializeField] public RawImage hitIndicatorPrefab;

    public float heightIndicatorPos;
    public float widthIndicatorPos;
    public List<Indicator> hitIndicators = new List<Indicator>();

    private bool isEnable;

    private void Awake()
    {
        this.player.AddObserver(this);

        heightIndicatorPos = uiScreenCanvas.rect.height / 4;
        widthIndicatorPos = uiScreenCanvas.rect.width / 4;
    }
    private void Update()
    {
        if(isEnable == false)
            return;

        if (hitIndicators.Count <= 0)
            return;

        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 cameraRight = Camera.main.transform.right;

        for (int i = 0; i < hitIndicators.Count; i++)
            hitIndicators[i].Update(cameraForward, cameraRight);
       
    }

    public virtual void OnNotify(Player player, SubjectPlayer.PlayerAction playerAction)
    {
        if(isEnable == false)
            return;

        if (playerAction == SubjectPlayer.PlayerAction.GetShoot)
        {
            Vector3 hitDir = -player.playerBulletDamageAbleBehavior.damageDetail.hitDir; // Reverse direction
            ShowIndicator(hitDir);
        }
    }

    public void OnNotify(Player player) { }

    protected void ShowIndicator(Vector3 hitDir)
    {
        Vector2 dir = new Vector2(hitDir.x, hitDir.z).normalized;
        RawImage indicate = Instantiate(hitIndicatorPrefab, uiScreenCanvas);
        hitIndicators.Add(new Indicator(this, indicate, dir, 3f));
    }
    private void OnValidate()
    {
        this.player = FindAnyObjectByType<Player>();
    }

    public void EnableUI() => this.enabled = true;
    

    public void DisableUI() => this.enabled = false;
   
}

public class Indicator
{
    public Vector2 direction;
    RawImage hitIndicatorImage;
    float duration;
    HittedIndicator hittedIndicator;
    float elapsedTime = 0f;

    public Indicator(HittedIndicator hittedIndicator, RawImage hitImage, Vector2 dir, float duration)
    {
        this.hittedIndicator = hittedIndicator;
        this.direction = dir;
        this.hitIndicatorImage = hitImage;
        this.duration = duration;
    }

    public void Update(Vector3 cameraForward, Vector3 cameraRight)
    {
        // Convert world-space camera direction to 2D
        Vector2 camForward2D = new Vector2(cameraForward.x, cameraForward.z).normalized;
        Vector2 camRight2D = new Vector2(cameraRight.x, cameraRight.z).normalized;

        // Project hit direction onto camera forward and right axes
        float dotForward = Vector2.Dot(direction, camForward2D);
        float dotRight = Vector2.Dot(direction, camRight2D);

        // Calculate angle between hit direction and camera forward
        float angle = Mathf.Atan2(dotRight, dotForward) * Mathf.Rad2Deg;

        // Rotate UI element based on hit direction
        hitIndicatorImage.rectTransform.rotation = Quaternion.Euler(0, 0, -angle);

        // Determine screen position
        float xOffset = Mathf.Sin(angle * Mathf.Deg2Rad) * hittedIndicator.widthIndicatorPos;
        float yOffset = Mathf.Cos(angle * Mathf.Deg2Rad) * hittedIndicator.heightIndicatorPos;

        // Apply position relative to the center of the screen
        hitIndicatorImage.rectTransform.anchoredPosition = new Vector2(xOffset, yOffset);

        // Handle indicator fade-out over time
        elapsedTime += Time.deltaTime;

        Color color = hitIndicatorImage.color;
        color.a = 1 - elapsedTime/duration;
        hitIndicatorImage.color = color;

        if (elapsedTime >= duration)
        {
            Object.Destroy(hitIndicatorImage.gameObject);
            hittedIndicator.hitIndicators.Remove(this);
        }

    }
    
}
