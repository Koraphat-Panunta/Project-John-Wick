using UnityEngine;

public interface IReloadMagazineNodePhase : IReloadNode
{
   public enum ReloadMagazinePhase
    {
        Enter,
        MagOut,
        MagIn,
        ChamberLoad,
        Exit,
    }
    public ReloadMagazinePhase curReloadPhase { get; set; }
}
