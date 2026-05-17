using UnityEngine;

public interface IShotgunReloadNode : IReloadNode
{
    public float _reloadTime { get; }
    public enum ShotgunReloadStage
    {
        Load
    }
    public static void QuadLoad(AutomaticShotgunModel automaticShotgunModel)
    {
        if (Mathf.Abs(automaticShotgunModel.maxAmmoCapacity - automaticShotgunModel.curBulletCapacity) >= 2)
        {
            automaticShotgunModel.LoadShellsIntoTube(2);
        }
        else
        {
            automaticShotgunModel.LoadShellsIntoTube(1);
        }


        automaticShotgunModel.Notify(automaticShotgunModel, IShotgunReloadNode.ShotgunReloadStage.Load);
    }
}
