using UnityEngine;
using UnityEngine.UI;

public class AngleController : MonoBehaviour
{
    [SerializeField] private RawImage five;
    [SerializeField] private RawImage nine;
    [SerializeField] private RawImage thirteen;
    [SerializeField] private RawImage seventeen;
    [SerializeField] private RawImage twenty;

    [SerializeField] private Texture filledImg;
    [SerializeField] private Texture emptyImg;

    DataForMenu dataInteractor;

    // Start is called before the first frame update
    void Start()
    {
        dataInteractor = GameObject.Find("/Canvas/Welcome").GetComponent<DataForMenu>();
        setFill();
    }

    //makes correct object be filled
    private void setFill()
    {
        getBox(dataInteractor.cameraAngle()).texture = filledImg;
    }
    private void removeFill()
    {
        getBox(dataInteractor.cameraAngle()).texture = emptyImg;
    }

    public void incAngle()
    {
        if(dataInteractor.cameraAngle() < 20)
        {
            removeFill();
            dataInteractor.incCameraAngle();
            dataInteractor.saveCurrentData();
            setFill();
        }
    }
    public void decAngle()
    {
        if (dataInteractor.cameraAngle() > 5)
        {
            removeFill();
            dataInteractor.decCameraAngle();
            dataInteractor.saveCurrentData();
            setFill();
        }
    }

    private RawImage getBox(int angle)
    {
        switch (angle)
        {
            case 5:
                return five;
            case 9:
                return nine;
            case 13:
                return thirteen;
            case 17:
                return seventeen;
            case 20:
                return twenty;
        }
        return null;
    }
}
