using UnityEngine;

public interface IReloadMagazineNode : IReloadNode
{
    public enum ReloadMagazineEvent
    {
        PickUpMag_In,
        ReleaseMag,
        InputMag,
        KeepMag_Out,
        ReChamber,
    }

    public float _reloadTime { get; }
}
