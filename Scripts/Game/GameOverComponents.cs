using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameOverComponents : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI finalScore;
    [SerializeField] TextMeshProUGUI finalCoinCount;
    [SerializeField] TextMeshProUGUI currentScore;
    [SerializeField] TextMeshProUGUI currentCoinCount;
    [SerializeField] TextMeshProUGUI totalCoinText;
    [SerializeField] GameObject continuePart;
    [SerializeField] GameObject nonContinuePart;

    private DataForPlayer data;
    private static bool wasContinued = false;

    public void Start()
    {
        data = FindObjectOfType<DataForPlayer>();
        wasContinued = false;
        handleContinuePart();
    }

    private void handleContinuePart()
    {
        if (!wasContinued)
        {
            continuePart.SetActive(true);
        }
        else
        {
            continuePart.SetActive(false);
            nonContinuePart.transform.position = new Vector3(nonContinuePart.transform.position.x, 1100, nonContinuePart.transform.position.z);
        }
    }
    public void restartGame()
    {
        //return time to normal
        Time.timeScale = 1;
        GameManager.isContinuedGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Score.score = 0;
        Directions.isNewGame = true;
        CarController.coinsThisGame = 0;
    }

    public void watchAdForLife()
    {
        GameObject.Find("GameManager").GetComponent<AdsManager>().watchAdForLife();
    }

    public void attemptContinueGame()
    {
        //check if player has 1000 coins
        if (data.numCoins() >= 1000)
        {
            data.decCoins(1000);
            continueGame();
        }
        else
        {
            StartCoroutine(changeColor());
        }
    }

    public void continueGame()
    {
        wasContinued = true;
        handleContinuePart();
        Directions.isNewGame = false;
        GameManager.isContinuedGame = true;
        //increment score and increment coins
        //don't start new session with lootlocker so that the same person can only be on the score once
        GameObject.Find("Car/").GetComponent<CarController>().loadNewCar();
        FindObjectOfType<GameManager>().loadContinuedGame();
        gameObject.SetActive(false);
        currentCoinCount.enabled = true;
        currentScore.enabled = true;
        data.saveCurrentData();
    }

    private IEnumerator changeColor()
    {
        totalCoinText.color = new Color32(232, 95, 85, 255);
        yield return new WaitForSecondsRealtime(.2f);
        totalCoinText.color = new Color32(255, 255, 255, 255);
    }

    public void exitToMenu()
    {
        Score.score = 0;
        CarController.coinsThisGame = 0;
        Directions.isNewGame = true;
        Score.score = 0;
        //0 is index of welcome scene
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }

    public void updateTexts()
    {
        finalScore.text = "Score: " + currentScore.text;
        finalCoinCount.text = "Coins:  <sprite=\"coinSprite\" index=0>" + CarController.coinsThisGame;
        totalCoinText.text = "Total Coins: <sprite=\"coinSprite\" index=0>" + FindObjectOfType<DataForPlayer>().numCoins();
    }
}
