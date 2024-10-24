using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class STI_Sound : WeaponAudio
{
    private float ReloadSound_1;
    private float ReloadSound_2;
    private float ReloadSound_3;
    protected override float reloadSoundWait_0 
    {
        get 
        {
            return ReloadSound_1;
        }
        set
        {
            ReloadSound_1 = value;
        }
    }
    protected override float reloadSoundWait_1 
    {
        get 
        {
            return ReloadSound_2;
        }
        set 
        {
            ReloadSound_2 = value;
        }
    }
    protected override float reloadSoundWait_2 
    {
        get 
        {
            return ReloadSound_3;
        }
        set 
        {
            ReloadSound_3 = value;
        }
    }

    protected override void Start()
    {
        ReloadSound_1 = 0.26f;
        ReloadSound_2 = 0.54f - ReloadSound_1;
        ReloadSound_3 = 0.38f;
        base.Start();
    }
}
