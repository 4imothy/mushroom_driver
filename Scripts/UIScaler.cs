using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScaler : MonoBehaviour
{
    private float resoX;
    private float resoY;

    private CanvasScaler canvScal;
    // Start is called before the first frame update
    void Start()
    {
        canvScal = GetComponent<CanvasScaler>();
        setInfo();
    }

    void setInfo()
    {
        resoX = (float)Screen.currentResolution.width;
        resoY = (float)Screen.currentResolution.height;

        canvScal.referenceResolution = new Vector2(resoX, resoY);
    }
}
