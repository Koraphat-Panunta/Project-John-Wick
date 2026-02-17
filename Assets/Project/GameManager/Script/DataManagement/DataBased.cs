using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataBased : MonoBehaviour
{
    public static DataBased Instance;

    [SerializeField] public DataBasedSCRP<WeaponDataScriptableObject> weaponDataBased;
    [SerializeField] public DataBasedSCRP<AttachmentDataScriptableObject> weaponAttachmentDataBased;
    [SerializeField] public DataBasedSCRP<LevelDataScriptableObject> levelDataBased;

    public void Awake()
    {
        Instance = this;
    }
  

   public class SettingData
    {
        public float mouseSensitivivty = 1;
        public float mouseAimDownSightSensitivity = 1;
        public float volumeMaster = 1;
        public float volumeEffect = 1;
        public float volumeMusic = 1;
    }
    public SettingData settingData = new SettingData();

}

[Serializable]
public class DataBasedSCRP<T> where T : DataScriptableObject
{
    [SerializeField] T[] dataScrp;

    public T GetWeaponDataFormID(string WeaponID)
    {
        for (int i = 0; i < this.dataScrp.Length; i++)
        {
            if (this.dataScrp[i].ObjectID == WeaponID)
                return this.dataScrp[i];
        }

        Debug.LogError("No Object ID " + WeaponID + " in data based");
        return null;
    }
}
