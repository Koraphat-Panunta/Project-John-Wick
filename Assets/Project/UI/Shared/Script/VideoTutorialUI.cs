using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class VideoTutorialUI : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private TextMeshProUGUI tutorialMassage;

    public void SetVideoPlayer(VideoPlayer videoPlayer)
    {
        this.videoPlayer = videoPlayer;
    }

    public void SetTutorialMassage(TextMeshProUGUI tutorialMassage)
    {
        this.tutorialMassage = tutorialMassage;
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
