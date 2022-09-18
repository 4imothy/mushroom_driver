using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private float rotateConst = 100f;
    private float r;

    void Start()
    {
        r = Random.Range(0, 360);
        gameObject.transform.Rotate(0, r, 0);
    }
    void Update()
    {
        gameObject.transform.Rotate(0, rotateConst * Time.deltaTime, 0);   
    }
}
