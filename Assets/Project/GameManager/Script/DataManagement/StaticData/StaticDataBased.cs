using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StaticDataBased : MonoBehaviour
{
    public static StaticDataBased Instance { get; protected set; }

    [SerializeField] public DataEntities<WeaponDataScriptableObject> weaponDataBased;
    [SerializeField] public DataEntities<AttachmentDataScriptableObject> weaponAttachmentDataBased;
    [SerializeField] public DataEntities<LevelDataScriptableObject> levelDataBased;

    [SerializeField] public SettingDataScriptableObject defaultSettingData;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        this.weaponDataBased.InitilaizedData();
        this.weaponAttachmentDataBased.InitilaizedData();
        this.levelDataBased.InitilaizedData();
    }
  

   public class SettingData
    {
        public float mouseSensitivivty = 5;
        public float mouseAimDownSightSensitivity = 5;
        public float volumeMaster = 1;
        public float volumeEffect = 1;
        public float volumeMusic = 1;
    }
    public SettingData settingData = new SettingData();

}

[Serializable]
public class DataEntities<T> where T : DataScriptableObject
{
    [SerializeField] T[] dataScrp;
    private Dictionary<string, T> dataLookUp;
    public void InitilaizedData()
    {
        this.dataLookUp = new Dictionary<string, T>();
        foreach (var data in dataScrp)
        {
            this.dataLookUp[data.ObjectID] = data;
        }
    }

    public T GetObjectDataFormID(string objectID)
    {
        if(this.dataLookUp.TryGetValue(objectID,out T data))
            return data;

        Debug.LogError("No Object ID " + objectID + " in data based");
        return null;
    }

    public T GetObjectDataFormIndex(int index)
    {
        if (this.dataScrp[index] != null)
            return this.dataScrp[index];

        Debug.LogError("No Object Index " + index + " in data based");
        return null;
    }


}
