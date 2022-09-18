using UnityEngine;
using LootLocker.Requests;
using System.Collections;
using System.Reflection;
using TMPro;

public class DataForMenu : MonoBehaviour
{
    public PlayerData data;

    public TextMeshProUGUI coinCountText;
    public TextMeshProUGUI highScoreText;

    public GameObject howToStart;

    private bool synced = false;

    private void Awake()
    {
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
        if (data.firstTime)
        {
            howToStart.SetActive(true);
            data.firstTime = false;
            data.savePlayer();
        }
        data.savePlayer();
        //because camera angle is a new field that didn't exist before
        try
        {
            if (data.cameraAngle == 0 || !hasGoodAngle())
            {
                data.cameraAngle = 13;
                data.savePlayer();
            }
        }
        catch (System.Exception)
        {
            data.cameraAngle = 13;
        }
    }

    private bool hasGoodAngle()
    {
        //5 9 13 17 20
        return (data.cameraAngle == 5 || data.cameraAngle == 9 || data.cameraAngle == 13 || data.cameraAngle == 17 || data.cameraAngle == 20);
    }
    //any time this method is called be sure that the data was saved to the file before this point
    void Start()
    {
        GameObject parent = GameObject.Find("/Canvas/Welcome/GameButton/CarObjectParent");
        string path = "Cars/" + data.selectedCar;
        GameObject body = Instantiate(Resources.Load(path + "/Body") as GameObject, parent.transform);
        GameObject wheels = null;
        if (!data.selectedCar.Equals("Hover"))
            wheels = Instantiate(Resources.Load(path + "/Wheels") as GameObject, parent.transform);
        body.layer = 5;
        foreach (Transform child in body.transform)
        {
            child.gameObject.layer = 5;
            foreach (Transform secondChild in child)
                secondChild.gameObject.layer = 5;
        }
        if (wheels != null)
        {
            foreach (Transform child in wheels.transform)
            {
                child.gameObject.layer = 5;
                foreach (Transform secondChild in child)
                    secondChild.gameObject.layer = 5;
            }
        }
        updateTexts();
        StartCoroutine(LoginRoutine());
    }

    public int turnConst()
    {
        return data.turnConst;
    }

    public int speedConst()
    {
        return data.speedConst;
    }

    public void incSpeedConst()
    {
        data.speedConst++;
    }

    public void decSpeedConst()
    {
        data.speedConst--;
    }

    public void incTurnConst()
    {
        data.turnConst++;
    }

    public void decTurnConst()
    {
        data.turnConst--;
    }

    public int cameraAngle()
    {
        return data.cameraAngle;
    }

    public void incCameraAngle()
    {
        //5 9 13 17 20
        if (data.cameraAngle == 5)
        {
            data.cameraAngle = 9;
            return;
        }
        if (data.cameraAngle == 9)
        {
            data.cameraAngle = 13;
            return;
        }
        if (data.cameraAngle == 13) { 
            data.cameraAngle = 17;
            return;
        }
        if (data.cameraAngle == 17) { 
            data.cameraAngle = 20;
            return;
        }
    }

    public void decCameraAngle()
    {
        //5 9 13 17 20
        if (data.cameraAngle == 20)
        {
            data.cameraAngle = 17;
            return;
        }
        if (data.cameraAngle == 17)
        {
            data.cameraAngle = 13;
            return;
        }
        if (data.cameraAngle == 13)
        {
            data.cameraAngle = 9;
            return;
        }
        if (data.cameraAngle == 9)
        {
            data.cameraAngle = 5;
            return;
        }
    }

    private IEnumerator LoginRoutine()
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

    public void updateTexts()
    {
        coinCountText.text = "Coins: <sprite=\"coinSprite\" index=0>" + numCoins();
        highScoreText.text = "High Score: " + highScore();
    }

    public void saveCurrentData()
    {
        //overides file with current data if it exists
        data.savePlayer();
    }

    public int numCoins()
    {
        return data.numCoins;
    }

    public void addCoin()
    {
        data.numCoins++;
    }
    public void addCoins(int toAdd)
    {
        data.numCoins += toAdd;
    }

    public void setSelectedCar(string car)
    {
        data.selectedCar = car;
    }
    public void addNewCar(string car)
    {
        data.ownedCars.Add(car);
    }
    public void setMusicOn(bool on)
    {
        data.musicOn = on;
    }
    public void setSoundOn(bool on)
    {
        data.soundOn = on;
    }

    public int highScore()
    {
        return data.highScore;
    }
}
