using UnityEngine;

public interface MagazineType 
{
    public ReloadMagazineFullStage reloadMagazineFullStage { get; set; }
    public TacticalReloadMagazineFullStage tacticalReloadMagazineFullStage { get; set; }
    public bool isMagIn { get; set; }
}
