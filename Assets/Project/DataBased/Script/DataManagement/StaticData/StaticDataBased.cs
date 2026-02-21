using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StaticDataBased : MonoBehaviour,IInitializedAble
{
    public static StaticDataBased Instance { get; protected set; }

    [SerializeField] public DataEntities<WeaponDataScriptableObject> weaponDataBased;
    [SerializeField] public DataEntities<AttachmentDataScriptableObject> weaponAttachmentDataBased;
    [SerializeField] public DataEntities<LevelDataScriptableObject> levelDataBased;

    [SerializeField] public ContinueData defaultContinueData;
    [SerializeField] public GameProgressionData defaultGameProgressionData;
    [SerializeField] public LoadoutData defaultLoadoutData;
    [SerializeField] public SettingDataScriptableObject defaultSettingData;

    public void Initialized()
    {
        Debug.Log("Initialized" + this);

        Instance = this;

        this.weaponDataBased.InitilaizedData();
        this.weaponAttachmentDataBased.InitilaizedData();
        this.levelDataBased.InitilaizedData();

        DontDestroyOnLoad(gameObject);
    }

}

[Serializable]
public class DataEntities<T> where T : DataScriptableObject
{
    [SerializeField] public T[] dataScrp;
    private Dictionary<string, T> dataLookUp;
    public void InitilaizedData()
    {
        this.dataLookUp = new Dictionary<string, T>();
        foreach (T data in dataScrp)
        {
            this.dataLookUp.Add(data.ObjectID, data);
        }
    }

    public T GetObjectDataFormID(string objectID)
    {
        if(this.dataLookUp.TryGetValue(objectID,out T data))
            return data;

        Debug.Log("objectID = " + objectID);
        Debug.LogWarning("No Object ID " + objectID + " in data based "+typeof(T));
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
