using UnityEngine;

public interface IReloadMagazineNode : IReloadNode
{
    public enum ReloadMagazineStage
    {
        PickUpMag_In,
        ReleaseMag,
        InputMag,
        KeepMag_Out,
        ReChamber,
    }

    public float _reloadTime { get; }
    public float _startReloadStageNormalizedTime { get; }
    public float _endReloadStageNormalizedTime { get; }
}
