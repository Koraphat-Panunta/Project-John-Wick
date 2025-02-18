using UnityEngine;

public interface IReloadSingleNodePhase : IReloadNode
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
