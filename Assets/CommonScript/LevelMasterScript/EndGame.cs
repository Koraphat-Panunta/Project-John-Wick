using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour,IObseverLevel
{
    [SerializeField] private TextMeshProUGUI ShowScore;
    [SerializeField] private TextMeshProUGUI PressRestart;
    private bool isEndGmae;
    private float score;
    public void OnNotify(LevelManager level, LevelSubject.LevelEvent levelEvent)
    {
        if(levelEvent == LevelSubject.LevelEvent.LevelClear)
        {
            level.TryGetComponent<Level1>(out Level1 level1);
            this.score = level1.score.score;
            ShowScore.text = "Score : " + score;
            ShowScore.enabled = true;
            PressRestart.text = "Press R to Restart";
            PressRestart.enabled = true;
            isEndGmae = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isEndGmae = false;
        ShowScore.enabled = false;
        PressRestart.enabled = false;
        FindObjectOfType<Level1>().AddObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        if(isEndGmae == true)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
