using Sirenix.OdinInspector;
using System;
using UnityEngine;

[Serializable]
public class ResolutionDisplay 
{
    public enum ResolutionPreset
    {
        _800x600,
        _1280x720,
        _1920x1080,
        _2560x1440,
        _3840x2160
    }
    public enum TargetFPS
    {
        _30,
        _60,
        _90,
        _120,
        _144,
        Unlimit,
    }

    public static int[,] resolutionPreset 
        ={
        { 800,600}, 
        { 1280, 720 },
        { 1920, 1080 },
        { 2560, 1440 },
        { 3840, 2160 }
    };

    public static int[] limitFPSPreset = { 30, 60, 90, 120, 144,-1 };
       

    public static (int width, int height) GetResolution(ResolutionPreset resolution)
    {
        int index = (int)resolution;

        int width = resolutionPreset[index, 0];
        int height = resolutionPreset[index, 1];

        return (width, height);
    }

    public static int GetLimitFPS(TargetFPS limitFPS)
    {
        int index = (int)limitFPS;

        return limitFPSPreset[index];
    }
}
