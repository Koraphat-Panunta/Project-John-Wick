using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshiarShootpoint : ICrosshairAction
{
    private CrosshairController crosshairController;
    private RectTransform CrosshairCenterPosition;
    public CrosshiarShootpoint(CrosshairController crosshairController)
    {
        this.crosshairController = crosshairController;
        this.CrosshairCenterPosition = crosshairController.Crosshair_CenterPosition;
    }
    public RectTransform GetPointPosScreen()
    {
       
        float PosX = Random.Range(crosshairController.Crosshair_lineLeft.anchoredPosition.x + CrosshairCenterPosition.anchoredPosition.x,
            crosshairController.Crosshair_lineRight.anchoredPosition.x + CrosshairCenterPosition.anchoredPosition.x);

        float PosY = CrosshairCenterPosition.anchoredPosition.y;
        PosY = Random.Range(crosshairController.Crosshair_lineDown.anchoredPosition.y+CrosshairCenterPosition.anchoredPosition.y, 
            crosshairController.Crosshair_lineUp.anchoredPosition.y + CrosshairCenterPosition.anchoredPosition.y);
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
