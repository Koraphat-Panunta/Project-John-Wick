using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshiarShootpoint : ICrosshairAction
{
    private CrosshairController crosshairController;
    private RectTransform CrosshairCenterPosition;
    private RectTransform CrossRight;
    private RectTransform CrossLeft;
    private RectTransform CrossUp;
    private RectTransform CrossDown;
    public CrosshiarShootpoint(CrosshairController crosshairController)
    {
        this.crosshairController = crosshairController;
        this.CrosshairCenterPosition = crosshairController.Crosshair_CenterPosition;
        this.CrossRight = crosshairController.Crosshair_lineRight;
        this.CrossLeft = crosshairController.Crosshair_lineLeft;
        this.CrossUp = crosshairController.Crosshair_lineUp;
        this.CrossDown = crosshairController.Crosshair_lineDown;
    }
    public RectTransform GetPointPosScreen()
    {
        Vector2 lineUpPos = CrossUp.anchoredPosition;
        Vector2 lineDownPos = CrossDown.anchoredPosition;
        Vector2 lineLeftPos = CrossLeft.anchoredPosition;
        Vector2 lineRightPos = CrossRight.anchoredPosition;
        Vector2 CenterPos = CrosshairCenterPosition.anchoredPosition;


        float PosX = Random.Range(lineLeftPos.x + CenterPos.x,
            lineRightPos.x + CenterPos.x);

        float raduis = Mathf.Abs(Vector2.Distance(CenterPos,lineUpPos));

        float PosYBelow = -(Mathf.Sqrt(Mathf.Pow(raduis, 2) - Mathf.Pow(PosX - CenterPos.y, 2)) + CenterPos.x);
        float PosYAbove =  Mathf.Sqrt(Mathf.Pow(raduis, 2) - Mathf.Pow(PosX - CenterPos.y, 2)) + CenterPos.x;

        float PosY = Random.Range(PosYBelow, PosYAbove);

        Debug.Log("R = " + raduis);
        Debug.Log("CenterPos = "+CenterPos);
        Debug.Log("Circle X =" + PosX);
        Debug.Log("Circle Y = "+ PosYAbove);

        //float PosY = CenterPos.y;
        //PosY = Random.Range(lineDownPos.y + CenterPos.y,
        //    lineUpPos.y + CenterPos.y);

        crosshairController.PointPosition.anchoredPosition = new Vector2(PosX, PosY);
        
        
        return crosshairController.PointPosition;
    }
    public Vector3 GetPointDirection()
    {
        Vector3 pointPos;
        Ray ray = Camera.main.ScreenPointToRay(GetPointPosScreen().position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000, crosshairController.layerMask))
        {
           pointPos = hit.point;
        }
        else if (Physics.Raycast(ray, out hit, 1000, 1))
        {
            pointPos = hit.point;
        }
        else
        {
            //Vector3 worldPosition = ray.GetPoint(100);
            pointPos = ray.GetPoint(100);
        }
        return pointPos;
    }
    public void Performed(Weapon weapon)
    {
        throw new System.NotImplementedException();
    }

    public void Performed(PlayerStateManager playerStateManager)
    {
        throw new System.NotImplementedException();
    }

}
