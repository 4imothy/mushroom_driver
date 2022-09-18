using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeSwinging : MonoBehaviour
{

    public float rotateConst;

    private bool moveLeft = true;
    private Transform playerTrans;

    private void Start()
    {
        playerTrans = GameObject.Find("/Car").transform;
    }
    // Update is called once per frame
    void Update()
    {
        float rotX = transform.rotation.eulerAngles.x;
        if (moveLeft)
        {
            if (rotX > 29 && rotX < 31)
                moveLeft = false;

            transform.Rotate(rotateConst * Time.deltaTime, 0, 0);
        }
        else
        {
            if (rotX > 329 && rotX < 331)
                moveLeft = true;
            transform.Rotate(-rotateConst * Time.deltaTime, 0, 0);
        }
    }
}
