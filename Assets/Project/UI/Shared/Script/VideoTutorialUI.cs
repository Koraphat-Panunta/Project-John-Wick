using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoTutorialUI : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI tutorialMassage;

    public void SetVideoPlayer(VideoClip videoClip)
    {
        this.videoPlayer.clip = videoClip;
    }

    public void SetTutorialMassage(string tutorialMassage)
    {
        this.tutorialMassage.SetText(tutorialMassage);
    }

    public VideoPlayer GetVideoPlayer() => this.videoPlayer;
    public TextMeshProUGUI GetTutorialMassage() => this.tutorialMassage;

    private void OnEnable()
    {
        this.videoPlayer.Play();
    }
    private void OnDisable()
    {
        this.videoPlayer.Stop();
    }

}
