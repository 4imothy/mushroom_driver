using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public GameObject screen;
    public GameObject tutorial;
    public GameObject coinText;
    public GameObject scoreText;
    public TextMeshProUGUI countdown;

    private AudioSource player;

    private bool countingandPaused = false;

    public static bool isContinuedGame = false;

    private void Start()
    {
        player = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }

    public void pauseGame()
    {
        //stop updates
        Time.timeScale = 0;
        //show the pause screen
        screen.SetActive(true);
        //get rid of tutorial screen if it is active
        Destroy(tutorial);
        if (countdown.enabled)
        {
            countingandPaused = true;
            countdown.enabled = false;
        }
    }

    public void endPausedGame()
    {
        coinText.SetActive(true);
        scoreText.SetActive(true);
        StartCoroutine(endPauseOverTime());
    }

    public void endCountdown()
    {
        countdown.enabled = false;
        StopAllCoroutines();
    }

    private IEnumerator endPauseOverTime()
    {
        screen.SetActive(false);
        countdown.enabled = true;
        countdown.text = "3";

        if (countingandPaused)
        {
            countdown.enabled = false;
            countingandPaused = false;
            yield break;
        }
        yield return new WaitForSecondsRealtime(1f);
        if (countingandPaused)
        {
            countdown.enabled = false;
            countingandPaused = false;
            yield break;
        }
        countdown.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        if (countingandPaused)
        {
            countdown.enabled = false;
            countingandPaused = false;
            yield break;
        }
        countdown.text = "1";
        yield return new WaitForSecondsRealtime(1f);

        if (countingandPaused)
        {
            countdown.enabled = false;
            countingandPaused = false;
            yield break;
        }
        countdown.enabled = false;
        Time.timeScale = 1;
        if (isContinuedGame)
        {
            GameObject.Find("Car").GetComponent<CarController>().startNewCar();
            isContinuedGame = false;
        }
    }

    public void exitToMenu()
    {
        //save data and leave to menu
        Time.timeScale = 1;
        FindObjectOfType<CarController>().endCharacter();
        //reset the score
        Score.score = 0;
        //reset the coins
        CarController.coinsThisGame = 0;
        SceneManager.LoadScene(0);
    }
}
