using UnityEngine;

public class SoundBuilder
{
    protected Vector3 position;
    protected Quaternion rotation;
    protected SoundData soundData;
    protected SoundEmitterManager soundEmitterManager;
    public SoundBuilder(SoundEmitterManager soundEmitterManager,Vector3 pos,SoundData soundData)
    {
        this.soundEmitterManager = soundEmitterManager;
        this.position = pos;
        this.soundData = soundData;
    }
    public void WithPosition(Vector3 position)
    {
        this.position = position;
    }
    public void WithRotation(Quaternion rotation)
    {
        this.rotation = rotation;   
    }
    public void WithSoundData(SoundData soundData)
    {
        this.soundData = soundData;
    }
    public void Play()
    {
        this.soundEmitterManager.GetPoolSoundEmitter(out SoundEmitter soundEmitter);
        soundEmitter.Build(this.soundData);
        soundEmitter.transform.SetPositionAndRotation(this.position, this.rotation);
        soundEmitter.Play();
    }
    

}
