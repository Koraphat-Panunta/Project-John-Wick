using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CrosshairController : MonoBehaviour
{
    [SerializeField] [Range(0,1)] private float CrosshairSpread = 0;
    [SerializeField] [Range(15,30)] private float MinAccuracy = 0;
    [SerializeField] [Range(0,100)] private float MaxAccuracy = 0;
    public RectTransform Crosshair_lineUp;
    public RectTransform Crosshair_lineDown;
    public RectTransform Crosshair_lineLeft;
    public RectTransform Crosshair_lineRight;
    public RectTransform Crosshair_Position;
    [SerializeField] private GameObject TargetAim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CrosshairUpdate();
    }
    void CrosshairUpdate()
    {
       
        Crosshair_lineUp.anchoredPosition = new Vector2(0, MinAccuracy + (MaxAccuracy * CrosshairSpread));
        Crosshair_lineDown.anchoredPosition = new Vector2(0, -MinAccuracy - (MaxAccuracy * CrosshairSpread));
        Crosshair_lineLeft.anchoredPosition = new Vector2(-MinAccuracy - (MaxAccuracy * CrosshairSpread), 0);
        Crosshair_lineRight.anchoredPosition = new Vector2(MinAccuracy + (MaxAccuracy * CrosshairSpread), 0);
        if(CrosshairSpread > 0)
        {
            CrosshairSpread = Mathf.Lerp(CrosshairSpread, 0, 2f*Time.deltaTime);   
        }
        Vector3 CrosshairPos;
        CrosshairPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(Crosshair_Position.position));
        Ray ray = Camera.main.ScreenPointToRay(CrosshairPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit,1000,1))
        {
            Vector3 worldPosition = hit.point;
            TargetAim.transform.position = worldPosition;   
        }      
        else /*if(Physics.Raycast(ray ,out hit)==false)*/
        {
            Vector3 worldPosition = ray.GetPoint(100);
            TargetAim.transform.position = worldPosition;
        }
    }
   
    private void ShootSpread(Weapon weapon)
    {
        this.CrosshairSpread += weapon.Recoil;
        Debug.Log("ShootSpread");
    }
    private void OnEnable()
    {
        WeaponActionEvent.Scubscibtion(WeaponActionEvent.WeaponEvent.Fire,ShootSpread);
    }
    private void OnDisable()
    {
        WeaponActionEvent.UnSubscirbe(WeaponActionEvent.WeaponEvent.Fire,ShootSpread);
    }

}
