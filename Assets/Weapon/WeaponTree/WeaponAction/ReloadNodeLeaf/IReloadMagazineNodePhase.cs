using UnityEngine;

public interface IReloadMagazineNodePhase : IReloadNodePhase
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
