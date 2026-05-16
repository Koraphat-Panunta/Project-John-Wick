public interface IShotgunReloadNode : IReloadNode
{
    public enum ShotgunReloadStage
    {
        ChamberLoad,
        Preload,
        LoadShell
    }
}
