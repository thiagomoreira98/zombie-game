using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Interface : MonoBehaviour
{
    public Slider SliderPlayerLife;
    public GameObject GamerOverPanel;
    public Text GameOverDeathZombiesAmountText;
    public Text GameOverTimeSurvivedText;
    public Text ScoreDeathZombiesAmountText;
    public Text ScoreTimeSurvivedText;
    public Text HighScoreTimeSurvivedText;
    public Text HighScoreDeathZombiesAmountText;
    public Text BossWarnText;
    public Text IncreaseDificultWarnText;
    public Text LevelText;
    public float nextDificultTime = 15;
    [HideInInspector]
    public float countDificultTime;

    private PlayerControl playerControl;
    private int deathZombiesAmount = 0;
    private float maxTimeSurvived;
    private int maxDeathZombiesAmount;
    private int level = 1;
    

    // Start is called before the first frame update
    void Start()
    {
        this.playerControl = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        this.SliderPlayerLife.maxValue = playerControl.status.Life;
        this.UpdateSliderPlayerLife();
        Time.timeScale = 1;
        this.maxTimeSurvived = PlayerPrefs.GetFloat("maxTimeSurvived");
        this.maxDeathZombiesAmount = PlayerPrefs.GetInt("maxDeathZombiesAmount");
        this.countDificultTime = this.nextDificultTime;
    }

    private void Update()
    {
        this.ScoreTimeSurvivedText.text = string.Format("{0}", this.ConvertTime(Time.timeSinceLevelLoad));

        if (this.IsToIncreaseDificult())
        {
            this.IncreaseDificult();
        }
    }

    public void UpdateSliderPlayerLife()
    {
        this.SliderPlayerLife.value = playerControl.status.Life;
    }

    public void UpdateScoreKillsText()
    {
        this.ScoreDeathZombiesAmountText.text = string.Format("x {0}", this.deathZombiesAmount);
    }

    public void UpdateDeathZombiesAmount()
    {
        this.deathZombiesAmount += 1;
        this.UpdateScoreKillsText();
    }

    void UpdateMaxTimeSurvived(int minutes, int seconds)
    {
        if(Time.timeSinceLevelLoad > this.maxTimeSurvived)
        {
            this.maxTimeSurvived = Time.timeSinceLevelLoad;
            PlayerPrefs.SetFloat("maxTimeSurvived", Time.timeSinceLevelLoad);
            this.HighScoreTimeSurvivedText.text = string.Format("Time survived: {0}m{1}", minutes, seconds);
        }
        else
        {
            this.HighScoreTimeSurvivedText.text = string.Format("Time survived: {0}", this.ConvertTime(this.maxTimeSurvived));
        }
    }

    void UpdateMaxDeathZombiesAmount()
    {
        if (this.deathZombiesAmount > this.maxDeathZombiesAmount)
        {
            this.maxDeathZombiesAmount = this.deathZombiesAmount;
            PlayerPrefs.SetInt("maxDeathZombiesAmount", this.deathZombiesAmount);
            this.HighScoreDeathZombiesAmountText.text = string.Format("Kills: {0}", this.deathZombiesAmount);
        }
        else
        {
            this.HighScoreDeathZombiesAmountText.text = string.Format("Kills: {0}", this.maxDeathZombiesAmount);
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
        this.UpdateMaxDeathZombiesAmount();

        this.GameOverTimeSurvivedText.text = string.Format("Time survived: {0}m{1}s", minutes, seconds);
        this.GameOverDeathZombiesAmountText.text = string.Format("Kills: {0}", this.deathZombiesAmount.ToString());
        
        this.GamerOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("game");
    }

    public void ShowBossWarnText()
    {
        StartCoroutine(PopupText(2, this.BossWarnText));
    }

    public bool IsToIncreaseDificult()
    {
        return Time.timeSinceLevelLoad > this.countDificultTime;
    }

    public void IncreaseDificult()
    {
        this.countDificultTime = Time.timeSinceLevelLoad + this.nextDificultTime;
        this.level += 1;
        Debug.Log("Level: " + this.level);
        StartCoroutine(PopupText(2, this.IncreaseDificultWarnText));
        this.LevelText.text = string.Format("Level: {0}", this.level);
    }

    IEnumerator PopupText(float hideTime, Text text)
    {
        text.gameObject.SetActive(true);
        Color textColor = text.color;
        textColor.a = 1;
        text.color = textColor;

        yield return new WaitForSeconds(1);

        float count = 0;

        while(text.color.a > 0)
        {
            count += Time.deltaTime / hideTime;
            textColor.a = Mathf.Lerp(1, 0, count);
            text.color = textColor;

            if(text.color.a <= 0)
            {
                text.gameObject.SetActive(false);
            }

            yield return null;
        }
    }
}
