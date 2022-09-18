using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private RawImage seven;
    [SerializeField] private RawImage eight;
    [SerializeField] private RawImage nine;
    [SerializeField] private RawImage ten;
    [SerializeField] private RawImage eleven;

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
        getBox(dataInteractor.speedConst()).texture = filledImg;
    }
    private void removeFill()
    {
        getBox(dataInteractor.speedConst()).texture = emptyImg;
    }

    public void incSpeed()
    {
        if (dataInteractor.speedConst() < 11)
        {
            removeFill();
            dataInteractor.incSpeedConst();
            dataInteractor.saveCurrentData();
            setFill();
        }
        Debug.Log(dataInteractor.speedConst());
    }
    public void decSpeed()
    {
        if (dataInteractor.speedConst() > 7)
        {
            removeFill();
            dataInteractor.decSpeedConst();
            dataInteractor.saveCurrentData();
            setFill();
        }
        Debug.Log(dataInteractor.speedConst());
    }

    private RawImage getBox(int speed)
    {
        switch (speed)
        {
            case 7:
                return seven;
            case 8:
                return eight;
            case 9:
                return nine;
            case 10:
                return ten;
            case 11:
                return eleven;
        }
        return null;
    }
}
