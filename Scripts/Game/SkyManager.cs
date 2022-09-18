using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{

    [SerializeField] private GameObject[] possibleClouds;
    [SerializeField] private GameObject[] possibleRings;

    [SerializeField] private GameObject player;

    private List<GameObject> activeClouds = new List<GameObject>();
    private List<GameObject> activeRings = new List<GameObject>();

    int cloudDistance = 200;
    int ringDistance = 400;
    int maxClouds = 5;
    int maxRings = 1;
    int scoreCheck;
    float moveConst = 3f;

    bool firstClouds = true;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 6; i++)
            createCloud();
        createRing();
        firstClouds = false;
    }

    // Update is called once per frame
    void Update()
    {
        scoreCheck = (int) Score.score;
        if(scoreCheck % cloudDistance == 0 && scoreCheck != 0 && activeClouds.Count < maxClouds)
        {
            //so it doesn't summon many times
            createCloud();
            createCloud();
            createCloud();
        }
        HandleRings();
        UpdateClouds();
    }

    private void HandleRings()
    {
        if (scoreCheck % ringDistance == 0 && scoreCheck != 0 && activeRings.Count < maxRings)
        {
            createRing();
        }
        if (activeRings.Count != 0 && activeRings[0].transform.position.z < player.transform.position.z)
        {
            Destroy(activeRings[0]);
            activeRings.RemoveAt(0);
        }
    }

    private void createRing()
    {
        GameObject newRing = Instantiate(possibleRings[Random.Range(0, possibleRings.Length)]);
        newRing.transform.position = new Vector3(0, -80, Random.Range(player.transform.position.z + 1000, player.transform.position.z + 1100));
        activeRings.Add(newRing);
    }

    private void UpdateClouds()
    {
        Vector3 positon;
        GameObject cloud;
        for(int i = 0; i < activeClouds.Count; i++)
        {
            cloud = activeClouds[i]; 
            positon = cloud.transform.position;
            if (positon.z < player.transform.position.z)
            {
                deleteCloud(cloud);
                scoreCheck--;
                continue;
            }
            if (positon.z - 500 < player.transform.position.z)
                cloud.transform.position = new Vector3(positon.x + (moveConst * Time.deltaTime), positon.y, positon.z);
        }
    }

    private void createCloud()
    {
        GameObject newCloud = Instantiate(possibleClouds[Random.Range(0, possibleClouds.Length)]);
        if(firstClouds)
            newCloud.transform.position = new Vector3(Random.Range(-60, 40), Random.Range(30, 40), Random.Range(player.transform.position.z, player.transform.position.z + 500));
        else
            newCloud.transform.position = new Vector3(Random.Range(-45, 30), Random.Range(45, 50), Random.Range(player.transform.position.z + 500, player.transform.position.z + 700));
        float scale = Random.Range(1f, 1.5f);
        newCloud.transform.localScale = new Vector3(scale, scale, scale);
        activeClouds.Add(newCloud);
    }

    private void deleteCloud(GameObject toDelete)
    {
        //delete if any cloud is behind player
        Destroy(toDelete);
        activeClouds.Remove(toDelete);
    }
}
