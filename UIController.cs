using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject menuCanvas;

    public GameObject hudCanvas;

    public GameObject gameOverCanvas;

    public GameObject pauseCanvas;

    public GameObject settingsCanvas;

    public GameObject musicCanvas;
    public GameObject soundCanvas;

    public GameObject creditsCanvas;

    public GameObject shopCanvas;

    public TextMeshProUGUI scoreText;

    public Image hud;

    private Animator titleAnim;
    private Animator playAnim;
    private Animator lbAnim;
    private Animator shopAnim;

    private Animator shopCanvasAnim;

    private Animator hudAnim;

    private Animator resetBtnAnim;
    private Animator gameOverCVAnim;
    private Animator homeBtnAnim;

    private Animator settingsAnim;
    private Animator settingsMainAnim;

    private AudioManager audioManager;

    private TextMeshProUGUI pauseText;
    private Image pauseImage;
    private Image playImage;

    private TextMeshProUGUI goScoreText;
    private TextMeshProUGUI goBestText;

    private Transform gameOverTransform;

    private GameObject settingsImage;
    private GameObject settingsMain;


    void Start() 
    {
        gameOverTransform = gameOverCanvas.transform.Find("GameOver");

        settingsImage = settingsCanvas.transform.GetChild(0).gameObject;
        settingsMain = settingsCanvas.transform.GetChild(1).gameObject;

        goScoreText = gameOverTransform.GetChild(2).GetComponent<TextMeshProUGUI>();
        goBestText = gameOverTransform.GetChild(4).GetComponent<TextMeshProUGUI>();

        pauseText = pauseCanvas.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        pauseImage = pauseCanvas.transform.GetChild(0).GetComponent<Image>();
        playImage = pauseCanvas.transform.GetChild(2).GetComponent<Image>();

        titleAnim = menuCanvas.transform.GetChild(0).GetComponent<Animator>();
        playAnim = menuCanvas.transform.GetChild(1).GetComponent<Animator>();
        lbAnim = menuCanvas.transform.GetChild(3).GetComponent<Animator>();
        shopAnim = menuCanvas.transform.GetChild(4).GetComponent<Animator>();

        hudAnim = hudCanvas.GetComponent<Animator>();

        resetBtnAnim = gameOverCanvas.transform.GetChild(1).GetComponent<Animator>();
        gameOverCVAnim = gameOverCanvas.transform.GetChild(0).GetComponent<Animator>();
        homeBtnAnim = gameOverCanvas.transform.GetChild(2).GetComponent<Animator>();

        settingsAnim = settingsImage.GetComponent<Animator>();
        settingsMainAnim = settingsMain.GetComponent<Animator>();

        shopCanvasAnim = shopCanvas.GetComponent<Animator>();

        audioManager = FindObjectOfType<AudioManager>();
    }

    public void playRemoveTitleAnim() {
        titleAnim.Play("title", 0, 0);
        playAnim.Play("play", 0, 0);
        lbAnim.Play("play", 0, 0);
        shopAnim.Play("play", 0, 0);
    }

    public void playTitleAnim() {
        titleAnim.Play("titleIn", 0, 0);
        playAnim.Play("playIn", 0, 0);
        lbAnim.Play("playIn", 0, 0);
        shopAnim.Play("playIn", 0, 0);
    }

    public void playHudAnim() {
        hudAnim.Play("hud", 0, 0);
    }

    public void playRemoveHudAnim() {
        hudAnim.Play("hudOut", 0, 0);
    }

    public void playGameOver() {
        resetBtnAnim.Play("restartIn", 0, 0);
        gameOverCVAnim.Play("gameOverIn", 0, 0);
        homeBtnAnim.Play("restartIn", 0, 0);
    }

    public void playRemoveGameOver() {
        resetBtnAnim.Play("restart", 0, 0);
        gameOverCVAnim.Play("gameOver", 0, 0);
        homeBtnAnim.Play("restart", 0, 0);
    }

    public void playSettingsImageAnim() {
        settingsAnim.Play("settingsImageIn", 0, 0);
    }

    public void playRemoveSettingsImageAnim() {
        settingsAnim.Play("settingsImage", 0, 0);
    }

    public void playPauseImageAnim() {
        pauseCanvas.GetComponent<Animator>().Play("pauseIn", 0, 0);
    }

    public void playRemovePauseImageAnim() {
        pauseCanvas.GetComponent<Animator>().Play("pause", 0, 0);
    }

    public void playSettings(bool settings) 
    {
        audioManager.Play("select");

        string animation = "settingsMain";
        if (settings) { animation = "settingsMainIn"; }

        settingsMainAnim.Play(animation, 0, 0);
        settingsImage.transform.GetChild(0).gameObject.SetActive(!settings);
        settingsImage.transform.GetChild(1).gameObject.SetActive(settings);
    }

    public void setScore(int score) {
        scoreText.text = score.ToString();
    }

    public void setHud(float value) {
        float total = 80f;
        hud.rectTransform.sizeDelta = new Vector2(total * value, hud.rectTransform.sizeDelta.y);
    }

    public void setGameOverScores(int score, int highScore) {
        goScoreText.text = score.ToString();
        goBestText.text = highScore.ToString();
    }

    public void setPaused(bool text, bool play, bool pause) {
        pauseText.gameObject.SetActive(text);
        playImage.gameObject.SetActive(play);
        pauseImage.gameObject.SetActive(pause);
    }

    public void setPauseMusic(bool pause) {
        musicCanvas.transform.GetChild(1).gameObject.SetActive(pause);
        musicCanvas.transform.GetChild(0).gameObject.SetActive(!pause);
    }

    public void setPauseSound(bool pause) {
        soundCanvas.transform.GetChild(1).gameObject.SetActive(pause);
        soundCanvas.transform.GetChild(0).gameObject.SetActive(!pause);
    }

    public void playCredits()
    {
        creditsCanvas.SetActive(true);
        creditsCanvas.GetComponent<Animator>().Play("creditsAnim", 0, 0);
        StartCoroutine(waitForTime(15f));
    }

    public void playShopAnim()
    {
        shopCanvasAnim.Play("shopIn", 0, 0);
    }

    public void playRemoveShopAnim()
    {
        shopCanvasAnim.Play("shopOut", 0, 0);
    }

    public void clickAnimation(RectTransform transform, string methodName="") 
    {
        float animDuration = 0.07f;
        float scalingFactor = 0.9f;
        Vector3 originalSize = transform.localScale;
        IEnumerator animate()
        {
            transform.localScale = new Vector3(originalSize.x * scalingFactor,
                                               originalSize.y * scalingFactor,
                                               originalSize.z);
            yield return new WaitForSeconds(animDuration);
            transform.localScale = originalSize;
            if (methodName != "")
            {
                Invoke(methodName, animDuration);
            }
        }
        StartCoroutine(animate());
    }

    IEnumerator waitForTime(float time)
    {
        float currTime = 0f;
        while(currTime < time)
        {
            currTime += Time.deltaTime;
            yield return null;
        }
        creditsCanvas.SetActive(false);
    }
}
