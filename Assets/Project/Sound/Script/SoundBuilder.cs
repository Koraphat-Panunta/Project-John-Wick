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
    public SoundBuilder WithPosition(Vector3 position)
    {
        this.position = position;
        return this;
    }
    public SoundBuilder WithRotation(Quaternion rotation)
    {
        this.rotation = rotation;
        return this;
    }
    public SoundBuilder WithSoundData(SoundData soundData)
    {
        this.soundData = soundData;
        return this;
    }
    public void Play()
    {
        this.soundEmitterManager.GetPoolSoundEmitter(out SoundEmitter soundEmitter);
        soundEmitter.Build(this.soundData);
        soundEmitter.transform.SetPositionAndRotation(this.position, this.rotation);
        soundEmitter.Play();
    }
    

}
