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
    }
    public void ShootSpread(float spreadrate)
    {
        this.CrosshairSpread += 1;
    }

}
