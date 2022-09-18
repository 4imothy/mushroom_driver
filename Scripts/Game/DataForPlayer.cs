using UnityEngine;

public class DataForPlayer : MonoBehaviour
{
    private PlayerData data;
    public GameObject body;
    public GameObject wheels;

    //any time this method is called be sure that the data was saved to the file before this point
    void Awake()
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
        GameObject parent = GameObject.Find("/Car/Visual");
        string path = "Cars/" + data.selectedCar;
        body = Instantiate(Resources.Load(path + "/Body") as GameObject, parent.transform);
        if (data.selectedCar.Equals("Hover"))
            CarController.isHover = true;
        else
            wheels = Instantiate(Resources.Load(path + "/Wheels") as GameObject, parent.transform);
    }

    public int getTurnConst() {
        return data.turnConst;
    }
    public int getSpeedConst()
    {
        return data.speedConst;
    }
    public int cameraAngle()
    {
        return data.cameraAngle;
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

    public void decCoins(int toDec)
    {
        data.numCoins -= toDec;
    }

    public string getSelectedCar()
    {
        return data.selectedCar;
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

    public void updateHighScore()
    {
        if ((int)Score.score > data.highScore)
            data.highScore = (int)Score.score;
    }
}
