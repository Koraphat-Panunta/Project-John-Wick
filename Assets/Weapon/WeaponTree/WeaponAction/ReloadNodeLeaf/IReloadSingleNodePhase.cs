using UnityEngine;

public interface IReloadSingleNodePhase : IReloadNodePhase
{
    public enum ReloadSinglePhase
    {
        EnterPreload,

        EnterLoadChamber,
        ChamberLoadIn,

        EnterLoading,
        LoadIn,

        Exit
    }
}
