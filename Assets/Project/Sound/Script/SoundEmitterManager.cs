using System.Collections.Generic;
using UnityEngine;

public class SoundEmitterManager : MonoBehaviour,IInitializedAble
{
    public static SoundEmitterManager Instance;
    [SerializeField] private SoundEmitter soundEmitterPrefab;
    public int maxPoolObject;
    public ObjectPooling<SoundEmitter> soundEmitters;
    public List<SoundEmitter> soundEmittersList;

    public void Initialized()
    {
        this.soundEmitters
           = new ObjectPooling<SoundEmitter>(
               this.soundEmitterPrefab
               , this.maxPoolObject
               , this.maxPoolObject
               , Vector3.zero
               , this.transform
               );

        this.soundEmittersList = new List<SoundEmitter>();

        Instance = this;
    }
    public SoundBuilder CreateSoundBuilder(Vector3 pos,SoundData soundData) => new SoundBuilder( this,pos,soundData );
    public bool CanPool()
    {
        if (this.soundEmitters.CanGet() == false)
            return false;

        return true;
    }
    public bool GetPoolSoundEmitter(out SoundEmitter soundEmitter)
    {
        soundEmitter = null;

        //Retrun the old and reuse
        if (this.CanPool() == false)
        {
            this.soundEmitters.ReturnToPool(this.soundEmittersList[0]);
            this.soundEmittersList.RemoveAt(0);
        }

        soundEmitter = this.soundEmitters.Get();
        this.soundEmittersList.Add(soundEmitter);

        return true;
    }
    public void ReturnSoundEmitter(SoundEmitter soundEmitter)
    {
        this.soundEmitters.ReturnToPool(soundEmitter);
        this.soundEmittersList.Remove(soundEmitter);
    }

   
}
