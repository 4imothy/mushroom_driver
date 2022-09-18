using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class ShopScript : MonoBehaviour
{
    //default car
    public GameObject defaultContainer;

    //mushroom car
    public GameObject mushroomCarContainer;
    public TextMeshProUGUI mushroomStatus;

    //truck
    public GameObject truckContainer;
    public TextMeshProUGUI truckStatus;

    //cheese
    public GameObject cheeseContainer;
    public TextMeshProUGUI cheeseStatus;

    //sports
    public GameObject sportsContainer;
    public TextMeshProUGUI sportsStatus;

    //hover
    public GameObject hoverContainer;
    public TextMeshProUGUI hoverStatus;

    //boat
    public GameObject boatContainer;
    public TextMeshProUGUI boatStatus;

    //mini
    public GameObject miniContainer;
    public TextMeshProUGUI miniStatus;

    //beetle
    public GameObject beetleContainer;
    public TextMeshProUGUI beetleStatus;

    public GameObject selectionBox;
    public GameObject confirmDealMenu;
    public TextMeshProUGUI dealText;

    public TextMeshProUGUI coinText;
    private PlayerData data;

    private bool didAddCarOnStart = false;

    //this stores the car that is currently being dealt with
    private GameObject clicked = null;
    // Start is called before the first frame update
    void Start()
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
        if (data.ownsAllCars)
        {
            addIfDontHave("Truck");
            addIfDontHave("Cheese");
            addIfDontHave("Hover");
            addIfDontHave("Mushroom");
            addIfDontHave("Sports");
            addIfDontHave("Mini");
            addIfDontHave("Boat");
            addIfDontHave("Beetle");
        }
        data.savePlayer();
        if (didAddCarOnStart)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        updateBoxes();
        //update status on all cars
        updateCoinText();
        updateSelectionBox();
    }

    // Update is called once per frame
    void Update()
    {
        rotateCointainer(defaultContainer);
        rotateCointainer(mushroomCarContainer);
        rotateCointainer(truckContainer);
        rotateCointainer(cheeseContainer);
        rotateCointainer(sportsContainer);
        rotateCointainer(hoverContainer);
        rotateCointainer(boatContainer);
        rotateCointainer(miniContainer);
        rotateCointainer(beetleContainer);
    }

    public void attemptSelect()
    {
        clicked = EventSystem.current.currentSelectedGameObject;
        if (data.ownedCars.Contains(clicked.name))
        {
            selectionBox.transform.SetParent(clicked.transform);
            selectionBox.transform.localPosition = Vector3.zero;
            data.selectedCar = clicked.name;
            data.savePlayer();
        }
        else
        {
            //doesn't have the car
            if (data.numCoins < CarCosts.cost(clicked.name))
            {
                //can't purchase
                StartCoroutine(changeColorOfCoinText());
            }
            else
            {
                //show a purchase interface
                dealText.text = "Purchase the " + CarCosts.carName(clicked.name) + " for " + " <sprite=\"coinSprite\" index=0> " + CarCosts.cost(clicked.name);
                confirmDealMenu.SetActive(true);
            }
        }
    }

    public void leaveDeal()
    {
        confirmDealMenu.SetActive(false);
        clicked = null;
    }

    public void confDeal()
    {
        data.numCoins -= CarCosts.cost(clicked.name);
        data.ownedCars.Add(clicked.name);
        data.selectedCar = clicked.name;
        data.savePlayer();
        clicked = null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private IEnumerator changeColorOfCoinText()
    {
        coinText.color = new Color32(232, 95, 85, 255);
        yield return new WaitForSecondsRealtime(.2f);
        coinText.color = new Color32(255, 255, 255, 255);
    }

    private void updateSelectionBox()
    {
        selectionBox.transform.SetParent(GameObject.Find("/Canvas/Scroll/Panel/" + data.selectedCar).transform);
        selectionBox.transform.localPosition = Vector3.zero;
    }

    private void rotateCointainer(GameObject toRotate)
    {
        toRotate.transform.Rotate(0, 100 * Time.deltaTime, 0);
    }

    private void updateCoinText()
    {
        coinText.text = "Your Coins: <sprite=\"coinSprite\" index=0> " + data.numCoins;
    }
    private void updateBoxes()
    {
        if (data.ownedCars.Contains("Mushroom"))
        {
            mushroomStatus.text = "Owned";
        }
        else
        {
            mushroomStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Mushroom");
        }

        if (data.ownedCars.Contains("Truck"))
        {
            truckStatus.text = "Owned";
        }
        else
        {
            truckStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Truck");
        }

        if (data.ownedCars.Contains("Cheese"))
        {
            cheeseStatus.text = "Owned";
        }
        else
        {
            cheeseStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Cheese");

        }

        if (data.ownedCars.Contains("Sports"))
        {
            sportsStatus.text = "Owned";
        }
        else
        {
            sportsStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Sports");

        }

        if (data.ownedCars.Contains("Hover"))
        {
            hoverStatus.text = "Owned";
        }
        else
        {
            hoverStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Hover");

        }

        if (data.ownedCars.Contains("Boat"))
        {
            boatStatus.text = "Owned";
        }
        else
        {
            boatStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Boat");
        }

        if (data.ownedCars.Contains("Mini"))
        {
            miniStatus.text = "Owned";
        }
        else
        {
            miniStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Mini");
        }
        if (data.ownedCars.Contains("Beetle"))
        {
            beetleStatus.text = "Owned";
        }
        else
        {
            beetleStatus.text = "<sprite=\"coinSprite\" index=0> " + CarCosts.cost("Beetle");
        }
    }

    public void leaveShop()
    {
        data.savePlayer();
        GetComponent<Animator>().Play("LeaveShop");
        StartCoroutine(LeaveAfterAnim());
    }

    private IEnumerator LeaveAfterAnim()
    {
        yield return new WaitForSeconds(.4f);
        SceneManager.LoadScene(0);

    }

    private void addIfDontHave(string car)
    {
        if (!data.ownedCars.Contains(car))
        {
            data.ownedCars.Add(car);
            didAddCarOnStart = true;
        }
    }
}
