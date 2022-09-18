using UnityEngine;
using UnityEngine.SceneManagement;


public class RotateCar : MonoBehaviour
{
    private float rotateConst = 100f;
    public bool shouldRotate = true;
    public bool valuesSet = false;
    private float changeConst = 0;
    private float changeScale = 1.5f;
    private bool startedInvoke = false;

    void Update()
    {
        if (shouldRotate)
        {
            gameObject.transform.Rotate(0, rotateConst * Time.deltaTime, 0);
        }
        else
        {
            if (!valuesSet)
            {
                //dist to 360 in 20 increments

                if (transform.rotation.eulerAngles.y > 180)
                    changeConst = -(transform.rotation.eulerAngles.y - 360) / 20;
                else
                    changeConst = -(transform.rotation.eulerAngles.y) / 20;
                valuesSet = true;
            }
            if (!startedInvoke)
            {
                //calls 20 times in .75 second which is the time to end anim
                InvokeRepeating("rotateAndCenter", 0f, .0375f);
                startedInvoke = true;
            }
        }
    }

    private void rotateAndCenter()
    {
        rotateToZero();
        updateScale();
    }

    private void rotateToZero()
    {
        gameObject.transform.Rotate(0, changeConst, 0);
    }

    private void updateScale()
    {
        Vector3 curScale = transform.localScale;
        if (curScale.x < 1)
        {
            gameObject.transform.localScale = Vector3.zero;
            return;
        }
        gameObject.transform.localScale = new Vector3(curScale.x - changeScale, curScale.y - changeScale, curScale.z - changeScale);
    }
}
