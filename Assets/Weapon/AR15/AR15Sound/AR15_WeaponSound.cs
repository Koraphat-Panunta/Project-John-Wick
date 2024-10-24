using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR15_WeaponSound : WeaponAudio
{
    private float ReloadSoundWait_0 ;
    private float ReloadSoundWait_1 ;
    private float ReloadSoundWait_2 ;
    protected override float reloadSoundWait_0 
    { 
        get 
        {
            return ReloadSoundWait_0; 
        } 
        set 
        {
            ReloadSoundWait_0 = value; 
        } 
    }
    protected override float reloadSoundWait_1 
    {
        get 
        {
            return ReloadSoundWait_1;
        }
        set 
        {
            ReloadSoundWait_1 = value;
        }
    }
    protected override float reloadSoundWait_2 
    {
        get { return ReloadSoundWait_2; }
        set {ReloadSoundWait_2 = value; }
    }
    protected override void Start()
    {
        reloadSoundWait_0 = 0.26f;
        ReloadSoundWait_1 = 0.54f - reloadSoundWait_0;
        ReloadSoundWait_2 = 0.38f;
        base.Start();
    }
}
