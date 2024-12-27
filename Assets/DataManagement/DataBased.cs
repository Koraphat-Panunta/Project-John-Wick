using System.Collections.Generic;
using UnityEngine;

public class DataBased : MonoBehaviour
{
    //WeaponDatabase
    public WeaponDataBased weaponDataBased;

    //Level progression
    public Dictionary<InGameSceneManager, bool> levelProgressionClear = new Dictionary<InGameSceneManager, bool>();
    public InGameSceneManager curInGameScene;

    //PlayerLoadout
    public PrimaryWeapon primaryWeapon;
    public SecondaryWeapon secondaryWeapon;
    void Start()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
