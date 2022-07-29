using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public Slider SliderPlayerLife;
    public GameObject GamerOverPanel;
    public Text GameOverKillsText;
    public Text GameOverTimeSurvivedText;
    public Text ScoreKillsText;
    public Text ScoreTimeSurvivedText;
    public Text HighScoreTimeSurvivedText;

    private PlayerControl playerControl;
    private int kills = 0;
    private float maxTimeSurvived;

    // Start is called before the first frame update
    void Start()
    {
        this.playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        this.SliderPlayerLife.maxValue = playerControl.status.Life;
        this.UpdateSliderPlayerLife();
        Time.timeScale = 1;
        this.maxTimeSurvived = PlayerPrefs.GetFloat("maxTimeSurvived");
    }

    private void Update()
    {
        this.ScoreTimeSurvivedText.text = string.Format("Time: {0}", this.ConvertTime(Time.timeSinceLevelLoad));
    }

    public void UpdateSliderPlayerLife()
    {
        this.SliderPlayerLife.value = playerControl.status.Life;
    }

    public void UpdateScoreKillsText()
    {
        this.ScoreKillsText.text = string.Format("Kills: {0}", this.kills);
    }

    public void IncrementKills()
    {
        this.kills += 1;
        this.UpdateScoreKillsText();
    }

    void UpdateMaxTimeSurvived(int minutes, int seconds)
    {
        if(Time.timeSinceLevelLoad > this.maxTimeSurvived)
        {
            this.maxTimeSurvived = Time.timeSinceLevelLoad;
            PlayerPrefs.SetFloat("maxTimeSurvived", Time.timeSinceLevelLoad);
            this.HighScoreTimeSurvivedText.text = string.Format("High score\nTime survived: {0}m{1}", minutes, seconds);
        }
        else
        {
            this.HighScoreTimeSurvivedText.text = string.Format("High score\nTime survived: {0}", this.ConvertTime(this.maxTimeSurvived));
        }
    }

    private string ConvertTime(float time)
    {
        int min = (int)(time / 60);
        int sec = (int)(time % 60);
        return string.Format("{0}m{1}s", min, sec);
    }

    public void GameOver()
    {
        int minutes = (int)(Time.timeSinceLevelLoad / 60);
        int seconds = (int)(Time.timeSinceLevelLoad % 60);

        this.UpdateMaxTimeSurvived(minutes, seconds);

        this.GameOverTimeSurvivedText.text = string.Format("Time survived: {0}m{1}s", minutes, seconds);
        this.GameOverKillsText.text = string.Format("Kills: {0}", this.kills.ToString());
        
        this.GamerOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("game");
    }
}
