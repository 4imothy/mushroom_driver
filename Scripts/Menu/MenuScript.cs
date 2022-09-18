using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public GameObject carContainer;
    public GameObject customizeMenu;

    public void openControls()
    {
        GetComponent<Animator>().Play("openControls");
        customizeMenu.SetActive(true);
    }

    public void closeControls()
    {
        GetComponent<Animator>().Play("leaveControls");
        StartCoroutine(closeControlsAfterAnim());
    }

    private IEnumerator closeControlsAfterAnim()
    {
        yield return new WaitForSeconds(.5f);
        customizeMenu.SetActive(false);
    }
    public void BeginTheGame()
    {
        //loads the game scene
        SceneManager.LoadScene(2);
    }

    public void startGame()
    {
        carContainer.GetComponent<RotateCar>().shouldRotate = false;
        //GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("LeaveMenu");
        StartCoroutine(LoadSceneAfterAnim(2));
    }

    public void openShop()
    {
        //loads shop
        GetComponent<Animator>().Play("LeaveForShop");
        StartCoroutine(LoadSceneAfterAnim(1));
    }

    public void OpenLeaderBoard()
    {
        if (GameObject.Find("ServerManager") != null)
        {
            GetComponent<Animator>().Play("LeaveForShop");
            StartCoroutine(LoadSceneAfterAnim(3));
        }
    }
    private IEnumerator LoadSceneAfterAnim(int index)
    {
        yield return new WaitForSeconds(.75f);
        SceneManager.LoadScene(index);
    }
    public void watchAdForMoney()
    {
        gameObject.GetComponent<AdsManager>().watchAdForMoney();
    }

    public void getMoneyFromAd()
    {
        Debug.Log("call get money");
        DataForMenu data = gameObject.GetComponent<DataForMenu>();
        data.addCoins(1000);
        data.saveCurrentData();
        data.updateTexts();
    }
}
