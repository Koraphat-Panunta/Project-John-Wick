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
    public Vector3 GetPointDirection(Vector3 OriginPos)
    {
        Vector3 Dir;
        Dir = crosshairController.TargetAim.transform.position - OriginPos;
        Dir = Dir.normalized;
        return Dir;
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
