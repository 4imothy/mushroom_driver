using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    //position rotation and scale
    //to ref the text
    public TextMeshProUGUI scoreText;
    public GameObject player;
    public static float score = 0;

    private Vector3 prevPos = Vector3.zero;

    // Update is called once per frame
    private void Start()
    {
        score = Mathf.Floor(score);
        score -= 25;
    }
    void Update()
    {
        if (prevPos.z > player.transform.position.z)
        {
            prevPos.z = player.transform.position.z;
            return;
        }
        score += player.transform.position.z - prevPos.z;
        prevPos.z = player.transform.position.z;

        //how far moved on z axis if starts at zero player.position.z
        scoreText.text = Mathf.Floor(score).ToString("0");
    }
}
