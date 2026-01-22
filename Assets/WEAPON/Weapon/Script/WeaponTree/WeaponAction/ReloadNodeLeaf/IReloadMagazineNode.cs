using UnityEngine;

public interface IReloadMagazineNode : IReloadNode
{
    public enum ReloadMagazineEvent
    {
        ReleaseMag,
        InputMag,
        ReChamber,
    }

    public float _reloadTime { get; }
}
