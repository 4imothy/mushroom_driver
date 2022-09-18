using UnityEngine;
using System.Collections.Generic;

public class GroundManager : MonoBehaviour
{
    //contains all possible grounds
    public GameObject[] groundPreFabs;
    public GameObject leftHills;
    public GameObject rightHills;
    public float hillsLength;
    public float hillsWidth;
    private GameObject prevHill = null;

    public GameObject car;

    //z position of newly spawned ground
    public int numberOfGrounds = 0;
    private List<GameObject> activeGrounds = new List<GameObject>();
    private List<GameObject> activeHills = new List<GameObject>();
    private bool saveFirstHills = true;
    private GameObject prevGround = null;
    public float groundWidth = 0;
    private int prevGroundIndex = 0;
    public float groundLength = 0;
    public GameObject farAwayForest;
    public bool resummonForContinue = false;

    private int groundSummonIndex = 0;
    private int hillSummonIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        //0 is index of start ground
        SpawnGround(0);
        for (int i = 1; i < numberOfGrounds; i++)
        {
            SpawnGround(Random.Range(1, groundPreFabs.Length));
        }
        summonHills();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if player is past this ground delete it and summon new

        float distTravel = car.transform.position.z;
        //first summon is delayed
        // check 0
        if (distTravel > activeGrounds[groundSummonIndex].transform.position.z + groundLength)
        {
            //means character passed the newest land
            SpawnGround(Random.Range(1, groundPreFabs.Length));
            groundSummonIndex = groundSummonIndex < 2 ? groundSummonIndex + 1 : groundSummonIndex;
        }
        if (distTravel > activeGrounds[2].transform.position.z + groundLength)
        {
            //means character passed the newest land
            DeleteFirstGround();
        }

        if (distTravel > activeHills[hillSummonIndex].transform.position.z + (hillsLength /2))
        {
            summonHills();
            hillSummonIndex = 2;
        }
        //check 2 because 0 and 1 are taken by the first round
        if (activeHills.Count > 2 && distTravel > activeHills[2].transform.position.z + (hillsLength/2))
        {
            DeleteFirstHills();
        }
        farAwayForest.transform.position = new Vector3(prevGround.transform.position.x + groundWidth / 2, 0, 300 + distTravel);
    }

    public void relocateHills() {
        foreach(GameObject hill in activeHills) {
            hill.transform.position = new Vector3(hill.transform.position.x, hill.transform.position.y, hill.transform.position.z - car.transform.position.z - 50);
        }
    }

    //handle if we need to resummon the lands
    public void handleLands()
    {
        if (doesNeedNewLands())
        {
            foreach(GameObject ground in activeGrounds)
            {
                Destroy(ground);
            }
            activeGrounds = new List<GameObject>();
            resummonForContinue = true;
            SpawnGround(0);
            for (int i = 1; i < numberOfGrounds; i++)
            {
                SpawnGround(Random.Range(1, groundPreFabs.Length));
            }
        }
    }

    private bool doesNeedNewLands()
    {
        foreach(GameObject land in activeGrounds)
        {
            //if every single one the z is greater then the car, then summon new ones
            //if ones z value is behind return false
            if(land.transform.position.z < car.transform.position.z)
            {
                return false;
            }
        }
        Debug.Log("summoning new lands");
        return true;
    }

    private void summonHills()
    {
        float prevHillZ = prevHill == null ? 0 : (prevHill.transform.position.z + hillsLength);
        float prevX = prevGround.transform.position.x;
        GameObject newLeft = Instantiate(leftHills, new Vector3(prevX- 3.5f, 0, prevHillZ), transform.rotation);
        activeHills.Add(newLeft);
        GameObject newRight = Instantiate(rightHills, new Vector3(prevX + 96.5f, 0, prevHillZ), transform.rotation);
        activeHills.Add(newRight);
        prevHill = newLeft;
    }

    private void DeleteFirstHills()
    {
        GameObject toRemove = activeHills[0];
        activeHills.RemoveAt(0);
        Destroy(toRemove);
        //0 is now the one that was at 1 before
        toRemove = activeHills[0];
        activeHills.RemoveAt(0);
        Destroy(toRemove);
    }

    private void SpawnGround(int groundIndex)
    {
        GameObject newGround;
        if (resummonForContinue)
        {
            newGround = Instantiate(groundPreFabs[groundIndex], new Vector3(-groundWidth / 2, 0, car.transform.position.z - 20), transform.rotation);
            resummonForContinue = false;
        }
        else if (prevGround == null)
        {
            newGround = Instantiate(groundPreFabs[groundIndex], new Vector3(-groundWidth / 2, 0, 0), transform.rotation);
        }
        else
        {
            //to not repeat levels twice in a row
            while(prevGroundIndex == groundIndex)
            {
                prevGroundIndex = Random.Range(1, groundPreFabs.Length);
            }
            newGround = Instantiate(groundPreFabs[groundIndex], new Vector3(prevGround.transform.position.x, prevGround.transform.position.y, prevGround.transform.position.z + groundLength), transform.rotation);
        }
        //so the next ground is summoned further along
        //this happens five times
        //add to list
        activeGrounds.Add(newGround);
        prevGround = newGround;
    }

    private void DeleteFirstGround()
    {
        //remove first in game and in internal list
        Destroy(activeGrounds[0]);
        activeGrounds.RemoveAt(0);
    }

    public GameObject getCurrentGround()
    {
        return activeGrounds[0];
    }
}
