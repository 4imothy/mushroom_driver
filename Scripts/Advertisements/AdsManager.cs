using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
#if UNITY_IOS
    // string gameID = "4805486";
    string gameID = AdKeys.iOSID();
    string rewarded = "Rewarded_iOS";
    string interstitial = "Interstitial_iOS";
#else
    string gameID = AdKeys.androidID();
    string rewarded = "Rewarded_Android";
    string interstitial = "Interstitial_Android";
#endif

    public static bool isInitialized = false;
    private PlayerData data;
    // Start is called before the first frame update
    void Start()
    {
        //game id for ads
        if (!isInitialized)
        {
            Advertisement.Initialize(gameID);
            Advertisement.AddListener(this);
            isInitialized = true;
        }

        //if file doesn't exist set to knew else get the data
        if ((data = SaveTheData.loadFromFile()) == null)
        {
            data = new PlayerData();
            data.selectedCar = "Default";
            data.ownedCars.Add("Default");
            data.numCoins = 0;
            data.highScore = 0;
            data.soundOn = true;
            data.musicOn = true;
        }
        data.savePlayer();
    }

    public void PlayPopUpAd()
    {
        if (Advertisement.IsReady(interstitial) && data.adsEnabled)
        {
            Time.timeScale = 0;
            Advertisement.Show(interstitial);
        }
        else
        {
            Debug.Log("not showing ad");
        }
    }

    public void watchAdForLife()
    {
        if (Advertisement.IsReady(rewarded))
        {
            Advertisement.Show(rewarded);
        }
        else
        {
            Debug.Log("ad not ready");
        }
    }

    public void watchAdForMoney()
    {
        if (Advertisement.IsReady(rewarded))
        {
            Debug.Log("show ad");
            Advertisement.Show(rewarded);
        }
        else
        {
            Debug.Log("ad not ready");
        }
    }

    //for interface

    public void OnUnityAdsReady(string placementId)
    {
    }

    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (placementId == interstitial)
        {
            Time.timeScale = 1;
        }
        if (placementId == rewarded && showResult == ShowResult.Finished)
        {
            //what happens if the ad gets finished
            //if we are in game
            if (SceneManager.GetActiveScene().buildIndex == 2)
            {
                GameObject.Find("UICanvas/GameOver").GetComponent<GameOverComponents>().continueGame();
            }
            if (SceneManager.GetActiveScene().buildIndex == 0)
            {
                Debug.Log("get reward from menu");
                GameObject.Find("/Canvas/Welcome").GetComponent<MenuScript>().getMoneyFromAd();
            }
        }
    }
}
