using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

public class VideoTutorialUI : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private GameObject uiCanvasZone;

    private void OnEnable()
    {
        this.videoPlayer.Play();
        this.uiCanvasZone.SetActive(true);
    }
    private void OnDisable()
    {
        this.videoPlayer.Stop();
        this.uiCanvasZone.SetActive(false);
    }

}
