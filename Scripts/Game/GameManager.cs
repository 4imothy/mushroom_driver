using UnityEngine;
using TMPro;
using System.Collections;
using LootLocker.Requests;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    bool isGameOver = false;
    public float restartDelay = 0;

    private float time = 0;
    private int numCycles = 0;
    private int timePerCycle = 90;

    private int oldScore = 0;
    public GameObject newHighScoreMenu;
    public Censor censor;
    public TMP_InputField userNameInput;
    private bool synced = false;
    private bool playerNameSet = false;

    public GameObject gameOverUI;
    public GameObject pauseScreen;
    public GameObject pauseMenu;
    public GameObject tutorial;
    public TextMeshProUGUI coinCount;
    public TextMeshProUGUI showScore;

    //for lighting
    public GameObject carLight;
    public GameObject mainLight;

    private static float gamesPlayed = 0;
    public static bool isContinuedGame = false;

    private string allTime = "MDBoardAllTime";
    private string weekly = "MDBoardWeekly";

    //0: unchecked
    //1: checked and on board
    //-1: checked and not on board
    private int onAllTime = 0;
    private int onWeekly = 0;

    bool delayActivated = false;

    private void Start()
    {
        Time.timeScale = 1;
        if (!isContinuedGame)
        {
            PlayerPrefs.DeleteAll();
            StartCoroutine(LoginDuringGame());
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time > timePerCycle)
            time -= timePerCycle * numCycles;
        //now time is between 0-90

        //first 50 seconds is day
        //10 seconds to transition
        //if (time > 50 && time < 60)
        if (time > 50 && time < 60)
        {
            transitionToNight();
        }
        else if (time > 80 && time < 90)
        {
            transitionToDay();
        }
        else if (time > 90)
        {
            numCycles++;
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus && !delayActivated)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            if (tutorial != null && tutorial.activeInHierarchy)
            {
                tutorial.SetActive(false);
            }
            if (pauseScreen.GetComponent<PauseScreen>().countdown.enabled)
            {
                pauseScreen.GetComponent<PauseScreen>().endCountdown();
            }
        }
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause && !delayActivated)
        {
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
            if (tutorial.activeInHierarchy)
            {
                tutorial.SetActive(false);
            }
        }
    }

    private void transitionToNight()
    {
        //the last second place final values
        if (time < 59.5)
        {
            //ten seconds to go from 0 to 3 for car
            carLight.GetComponent<Light>().intensity += .3f * Time.deltaTime;
            //ten seconds to go from 0 to 1 for directional
            mainLight.GetComponent<Light>().intensity -= .1f * Time.deltaTime;
        }
        else
        {
            carLight.GetComponent<Light>().intensity = 3;
            mainLight.GetComponent<Light>().intensity = 0;
        }
    }

    private void transitionToDay()
    {
        //the last second place final values
        if (time < 89.5)
        {
            //ten seconds to go from 0 to 3 for car
            carLight.GetComponent<Light>().intensity -= .3f * Time.deltaTime;
            //ten seconds to go from 0 to 1 for directional
            mainLight.GetComponent<Light>().intensity += .1f * Time.deltaTime;
        }
        else
        {
            carLight.GetComponent<Light>().intensity = 0;
            mainLight.GetComponent<Light>().intensity = 1;
        }
    }

    public void loadContinuedGame()
    {
        pauseScreen.SetActive(true);
        FindObjectOfType<Score>().enabled = true;
        pauseScreen.GetComponent<PauseScreen>().endPausedGame();
        PauseScreen.isContinuedGame = true;
        Debug.Log("relocating");
        GameObject.Find("LevelGenerator").GetComponent<GroundManager>().relocateHills();
        GameObject.Find("LevelGenerator").GetComponent<GroundManager>().handleLands();
        delayActivated = false;
        isGameOver = false;
    }

    public void endGameAfterDelay(float timeDelay)
    {
        //so pause button isn't visible
        pauseScreen.SetActive(false);
        delayActivated = true;
        Invoke("endGame", timeDelay);
    }

    public void endGame()
    {
        if (!isGameOver)
        {
            isGameOver = true;
            Score scoreScript = FindObjectOfType<Score>();
            scoreScript.enabled = false;
            pauseScreen.SetActive(false);
            Invoke("showGameOver", restartDelay);
        }
    }

    void showGameOver()
    {
        //show ad if they played three games
        gamesPlayed++;
        if (gamesPlayed % 3 == 0)
        {
            gameObject.GetComponent<AdsManager>().PlayPopUpAd();
        }
        //remove the other ui elements then the game over screen
        gameOverUI.SetActive(true);
        gameOverUI.GetComponent<GameOverComponents>().updateTexts();
        pauseScreen.SetActive(false);

        coinCount.enabled = false;
        showScore.enabled = false;

        //this is the animation time to show the game over screen
        Invoke("endUpdate", 1f);
        if (GameObject.Find("ServerManager") != null)
            StartCoroutine(checkHighScore());
        else
            StartCoroutine(LoginDuringGame());
    }

    public void startNewSession()
    {
        StartCoroutine(LoginDuringGame());
    }

    IEnumerator LoginDuringGame()
    {
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("guest session started: " + response.player_id);
                synced = true;
            }
            else
            {
                Debug.Log("couldn't start session" + response.Error);
                synced = true;
            }
        });
        yield return new WaitWhile(() => !synced);
    }

    public void SetAttemptedName()
    {
        Debug.Log("attempting name");
        string s = userNameInput.text;
        if (s.Length == 0)
            return; 
        string attemptUser = censor.CensorText(s);
        if(attemptUser.Equals(s))
        {
            StartCoroutine(SetPlayerName(attemptUser));
        }
        else
        {
            userNameInput.text = "";
            TextMeshProUGUI placeholder = (TextMeshProUGUI)userNameInput.placeholder;
            placeholder.text = "No Bad Words!";
        }
    }
    IEnumerator SetPlayerName(string name)
    {
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                StartCoroutine(SubmitScoreRoutine((int)Score.score));
                playerNameSet = true;
            }
            else
            {
                playerNameSet = true;
                Debug.Log(response.Error);
            }
        });
        yield return new WaitWhile(() => !playerNameSet);
    }

    private IEnumerator checkHighScore()
    {
        int scoresChecked = 0;
        //check for weekly
        LootLockerSDKManager.GetScoreList(weekly, 10, 0, (response) =>
        {
            if (response.success)
            {
                if (response.items.Length > 9)
                    oldScore = response.items[9].score;
                else
                    oldScore = 0;
                if ((int)Score.score > oldScore)
                {
                    //so one person can be on the scoreboard twice
                    PlayerPrefs.DeleteAll();
                    onWeekly = 1;
                }
                else
                    onWeekly = -1;
                scoresChecked++;
            }
            else
            {
                scoresChecked++;
            }
        });
        //check for all time
        LootLockerSDKManager.GetScoreList(allTime, 10, 0, (response) =>
        {
            if (response.success)
            {
                if (response.items.Length > 9)
                    oldScore = response.items[9].score;
                else
                    oldScore = 0;
                if ((int)Score.score > oldScore)
                {
                    //so one person can be on the scoreboard twice
                    PlayerPrefs.DeleteAll();
                    onAllTime = 1;
                    Debug.Log("on the global");
                }
                else
                    onAllTime = -1;
                scoresChecked++;
            }
            else
            {
                scoresChecked++;
            }
        });

        yield return new WaitWhile(() => scoresChecked != 2);
        if (onAllTime == 1 || onWeekly == 1)
        {
            newHighScoreMenu.SetActive(true);
        }
    }

    IEnumerator SubmitScoreRoutine(int scoreToUpload)
    {
        int submittedTo = 0;
        string playerID = "20";
        if (onAllTime == 1)
        {
            LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, allTime, (response) =>
            {
                if (response.success)
                {
                    submittedTo++;
                    newHighScoreMenu.SetActive(false);
                }
                else
                {
                    Debug.Log("submit fail!" + response.Error);
                }
            }); ; ;
        }
        else if (onAllTime == -1)
        {
            submittedTo++;
        }
        if (onWeekly == 1)
        {
            LootLockerSDKManager.SubmitScore(playerID, scoreToUpload, weekly, (response) =>
            {
                if (response.success)
                {
                    submittedTo++;
                    newHighScoreMenu.SetActive(false);
                }
                else
                {
                    Debug.Log("submit fail!" + response.Error);
                }
            });
        }
        else if (onWeekly == -1)
        {
            submittedTo++;
        }
        yield return new WaitWhile(() => submittedTo != 2);
    }

    void endUpdate()
    {
        Time.timeScale = 0;
    }
}