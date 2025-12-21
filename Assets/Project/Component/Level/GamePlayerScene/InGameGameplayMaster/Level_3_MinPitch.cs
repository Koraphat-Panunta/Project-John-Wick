using UnityEngine;
using UnityEngine.SceneManagement;

public class Level_3_MinPitch : MonoBehaviour
{
    [SerializeField] AudioClip bg_music;
    [SerializeField] AudioSource AudioSource;
    private void Start()
    {
        this.AudioSource.clip = bg_music;
       this.AudioSource.Play();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("New Scene");
        }
    }
}
