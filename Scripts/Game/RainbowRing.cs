using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainbowRing : MonoBehaviour
{
    [SerializeField] private LineRenderer circleRenderer;
    private float rotateConst = 50;
    private void Start()
    {
        circleRenderer.startWidth = -10;
        circleRenderer.endWidth = 10;
        DrawCircle(50, 200);
    }

    private void Update()
    {
        gameObject.transform.Rotate(0, 0, rotateConst * Time.deltaTime);

    }
    private void DrawCircle(int steps, float radius)
    {
        circleRenderer.positionCount = steps;

        for(int i = 0; i < steps; i++)
        {
            float circumfrenceProgress = (float)i / steps;

            float currentRadian = 2 * Mathf.PI * circumfrenceProgress;

            float xScaled = Mathf.Cos(currentRadian);
            float yScaled = Mathf.Sin(currentRadian);

            float x = xScaled * radius;
            float y = yScaled * radius;

            Vector3 curPos = new Vector3(x, y , 0);

            circleRenderer.SetPosition(i, curPos);
        }
    }
}
