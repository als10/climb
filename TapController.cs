using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TapController : MonoBehaviour
{
    public Player player;
    
    private GameManager gameManager;

    private UIController uIController;

    public CameraFollow cameraFollow;

    private AudioManager audioManager;

    public PlayGamesController playGamesController;

    public AdController adController;
    
    private int score;
    public int getScore() {
        return score;
    }

    private int highScore;
    public int getHighScore() {
        return highScore;
    }
    string highScoreKey = "HighScore";

    private int i = 0;

    private float totalTime = 5.5f;

    private float currentTime;
    private float multiplier = 1f;

    private int gameCount = 0;

    private bool isPaused;


    // Start is called before the first frame update
    void Start()
    {
        
        uIController = GetComponent<UIController>();
        gameManager = GetComponent<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();

        currentTime = totalTime;

        highScore = PlayerPrefs.GetInt(highScoreKey,0); 
        playGamesController.PostToLeaderboard(highScore);
        playGamesController.UnlockAchievement(highScore);

        isPaused = false; 

        if (audioManager.themeMusic == 1) { uIController.setPauseMusic(false); }
        else { uIController.setPauseMusic(true); }

        if (audioManager.soundMain == 1) { uIController.setPauseSound(false); }
        else { uIController.setPauseSound(true); }

    }

    // Update is called once per frame
    void Update()
    {
        if (player.gameOver || isPaused) { return; }

        currentTime -= multiplier * Time.deltaTime;
        uIController.setHud(currentTime / totalTime);

        if (currentTime <= 0) {
            player.gameOver = true;
            gameOver();
            return;
        }

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.9) { onTap(); }

    }

    private void onTap() 
    {
        string direction;

        if (Input.mousePosition.x > Screen.width * 0.5f) {
            direction = "right"; 
        }
        else {
            direction = "left";
        }

        bool checkIfScore = player.changeDirection(direction);

        if (!checkIfScore) 
        {
            return;
        }

        gameManager.removeFirstBldg(score);

        if (!gameManager.getDirectionFirstBldg(score).Equals(direction)) 
        {
            player.gameOver = true;
        }

        if (!player.gameOver) 
        {
            score++;
            uIController.setScore(score);
            if (currentTime + 0.3f <= totalTime) {
                currentTime += 0.3f;
            } else {
                currentTime = totalTime;
            }

            if (score - 41 == i * 50) 
            {
                gameManager.selectColour();
                i += 1;
            }

            if (score % 50 == 0) 
            {
                audioManager.Play("levelUp");
                if (multiplier < 1) { multiplier += 0.05f; }
            }

        } 
        else 
        {
            gameOver();
        }
    }

    public void reset(bool start) {
        gameCount++;

        audioManager.Play("select");

        cameraFollow.reset();

        player.reset();

        gameManager.reset(start);

        if (!start) {
            audioManager.Play("theme");
            uIController.playRemoveGameOver();
        }

        player.gameOver = false;
        score = 0; 
        i = 0;
        uIController.setScore(score);

        currentTime = totalTime;
        uIController.setHud(1);

        uIController.playHudAnim();
        uIController.playPauseImageAnim();

    }

    public void play(RectTransform transform) {
        IEnumerator wait() 
        {
            uIController.clickAnimation(transform, "playRemoveTitleAnim");
            uIController.playRemoveSettingsImageAnim();
            yield return new WaitForSeconds(0.1f);
            reset(true);
        }
        StartCoroutine(wait());
        //uIController.playRemoveTitleAnim();
    }

    public void home() {
        audioManager.Play("select");
        uIController.playRemoveGameOver();
        uIController.playTitleAnim();
        uIController.playSettingsImageAnim();
        audioManager.Play("theme");
    }

    public void pause() {
        audioManager.Play("select");
        audioManager.Pause("theme");
        isPaused = true;
        uIController.setPaused(true, true, false);
    }

    public void unPause() {
        audioManager.Play("select");
        audioManager.Play("theme");
        isPaused = false;
        uIController.setPaused(false, false, true);
    }


    public void pauseMusic(bool pause) {
        audioManager.Play("select");
        audioManager.setMusic(!pause);
        if (pause) { audioManager.Pause("theme"); }
        else { audioManager.Play("theme"); }
        uIController.setPauseMusic(pause);
    }

    public void pauseSound(bool pause) {
        audioManager.Play("select");
        uIController.setPauseSound(pause);
        audioManager.setSound(!pause);
    }

    public void gameOver() {
        audioManager.Play("die");
        audioManager.Stop("theme");

        player.GetComponent<BoxCollider2D>().enabled = false;
        uIController.gameOverCanvas.gameObject.SetActive(true);
        uIController.playRemoveHudAnim();

        player.gameOver = true;
        if (highScore < score) {
            highScore = score;  
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();
            playGamesController.PostToLeaderboard(score);
            playGamesController.UnlockAchievement(score);
        }

        uIController.playGameOver();
        uIController.playRemovePauseImageAnim();
        uIController.setGameOverScores(getScore(), getHighScore());

        Invoke("showAd", 0.5f);        

        if (gameCount >= 5) 
        {
            gameCount = 0;
        }

    }

    private void showAd() 
    {
        adController.ShowInterstitial();
    }

}
