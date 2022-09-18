using UnityEngine;

public class Directions : MonoBehaviour
{
    public RectTransform rect;
    private float changeConst = 250;
    private bool moveLeft = true;
    public GameObject tutorialScreen;
    private float time;

    public static bool isNewGame = true;

    private void Start()
    {
        if (!isNewGame)
        {
            tutorialScreen.SetActive(false);
        }
    }
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2.5)
            tutorialScreen.SetActive(false);
        float posX = rect.anchoredPosition.x;
        if (posX < -140)
            moveLeft = false;
        if (posX > 140)
            moveLeft = true;
        if (moveLeft)
        {
            rect.anchoredPosition = new Vector3(posX - (changeConst * Time.deltaTime), 0, 0);
        }
        else{
            rect.anchoredPosition = new Vector3(posX + (changeConst * Time.deltaTime), 0, 0);

        }
    }
}
