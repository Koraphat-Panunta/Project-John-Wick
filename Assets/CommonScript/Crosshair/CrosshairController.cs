using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] WeaponSocket weaponSocket;
    [SerializeField] [Range(15,30)] private float MinAccuracy = 0;
    [SerializeField] [Range(0,100)] private float MaxAccuracy = 0;
    public RectTransform Crosshair_lineUp;
    public RectTransform Crosshair_lineDown;
    public RectTransform Crosshair_lineLeft;
    public RectTransform Crosshair_lineRight;
    public RectTransform Crosshair_CenterPosition;
    public RectTransform PointPosition;
    [SerializeField] public GameObject TargetAim;
    public bool isVisable = false;

    public CrosshairSpread CrosshairSpread { get; private set; }
    public CrosshiarShootpoint CrosshiarShootpoint { get; private set; }
    [SerializeField] public LayerMask layerMask;
    void Start()
    {
        CrosshairSpread = new CrosshairSpread(this);
        CrosshiarShootpoint = new CrosshiarShootpoint(this);
        StartCoroutine(SetSpreadEvent(CrosshairSpread));
    }

    // Update is called once per frame
    void Update()
    {
        CrosshairUpdate();
    }
    void CrosshairUpdate()
    {
        Vector3 CrosshairPos;
        //CrosshairPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Crosshair_Position.position));
        CrosshairPos = Crosshair_CenterPosition.position;
        Ray ray = Camera.main.ScreenPointToRay(CrosshairPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = worldPosition;
        }
        else if(Physics.Raycast(ray, out hit, 1000,1))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = worldPosition;
        }
        else
        {
            Vector3 worldPosition = ray.GetPoint(100);
            TargetAim.transform.position = worldPosition;
        }
    }
    IEnumerator SetSpreadEvent(CrosshairSpread crosshairSpread)
    {
        while (weaponSocket.weaponSingleton == null)
        {
            yield return null;
        }
        weaponSocket.weaponSingleton.FireEvent += crosshairSpread.Performed;
    }
    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
       
    }
}
