using UnityEngine;

public class AnimationTriggerAudioEventPlayer 
{
    int audioIndex = 0;
    AnimationTriggerEventPlayer animationTriggerEventPlayer;
    AudioAnimationTriggerEvent[] audioAnimationTriggerEvents;

    SoundData[] soundData;

    public Vector3 playSoundPos;

    public AnimationTriggerAudioEventPlayer(AnimationClip clip,float enterNormalized,float exitNormalized, AudioAnimationTriggerEvent[] audioAnimationTriggerEvents)
    {
        this.audioIndex = 0;
        this.audioAnimationTriggerEvents = audioAnimationTriggerEvents;
        this.soundData = new SoundData[audioAnimationTriggerEvents.Length];

        AnimationTriggerEventDetail[] animationTriggerEventDetail = new AnimationTriggerEventDetail[audioAnimationTriggerEvents.Length];

        for (int i = 0; i < audioAnimationTriggerEvents.Length; i++)
        {
            animationTriggerEventDetail[i] = new AnimationTriggerEventDetail
            {
                normalizedTime = audioAnimationTriggerEvents[i].normalizedTime,
                eventName = "Audio " + i
            };
            this.soundData[i] = audioAnimationTriggerEvents[i].soundData;
        }

        this.animationTriggerEventPlayer = new AnimationTriggerEventPlayer(clip,enterNormalized,exitNormalized,animationTriggerEventDetail);

        for (int i = 0;i < audioAnimationTriggerEvents.Length; i++)
        {
            this.animationTriggerEventPlayer.SubscribeEvent("Audio "+i,this.PlayAudio);
        }
    }

    public void Rewind()
    {
        this.animationTriggerEventPlayer.Rewind();
        this.audioIndex = 0;
    }
    public void Update(float deltaTime,Vector3 playAudioPos)
    {
        this.SetPlaySoundPos(playAudioPos);
        this.animationTriggerEventPlayer.UpdatePlay(deltaTime);
    }

    public void SetPlaySoundPos(Vector3 playSoundPos) => this.playSoundPos = playSoundPos;
    public void PlayAudio()
    {
        SoundEmitterManager.Instance.CreateSoundBuilder(this.playSoundPos, this.soundData[this.audioIndex]).Play();
        this.audioIndex++;
    }
}
