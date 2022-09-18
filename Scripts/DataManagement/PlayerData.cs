using System.Collections.Generic;
using System;

[System.Serializable]
public class PlayerData
{
    public int numCoins = 0;
    public int highScore = 0;
    public List<string> ownedCars = new List<string>();
    public string selectedCar;
    public bool musicOn = true;
    public bool soundOn = true;
    public bool firstTime = true;
    public bool adsEnabled = true;
    public bool ownsAllCars = false;

    public int turnConst = 5;
    public int speedConst = 9;
    public int cameraAngle = 13;

    public void savePlayer()
    {
        SaveTheData.createSaveFile(this);
    }

}
