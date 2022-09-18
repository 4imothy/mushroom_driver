using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TurnController : MonoBehaviour
{
    [SerializeField] private RawImage three;
    [SerializeField] private RawImage four;
    [SerializeField] private RawImage five;
    [SerializeField] private RawImage six;
    [SerializeField] private RawImage seven;

    [SerializeField] private Texture filledImg;
    [SerializeField] private Texture emptyImg;

    DataForMenu dataInteractor;

    // Start is called before the first frame update
    void Start()
    {
        dataInteractor = GameObject.Find("/Canvas/Welcome").GetComponent<DataForMenu>();
        setFill();
    }

    private void setFill()
    {
        getBox(dataInteractor.turnConst()).texture = filledImg;
    }

    private void removeFill()
    {
        getBox(dataInteractor.turnConst()).texture = emptyImg;
    }

    public void incTurnConst()
    {
        if(dataInteractor.turnConst() < 7)
        {
            removeFill();
            dataInteractor.incTurnConst();
            dataInteractor.saveCurrentData();
            setFill();
        }
    }

    public void decTurnConst()
    {
        if(dataInteractor.turnConst() > 3)
        {
            removeFill();
            dataInteractor.decTurnConst();
            dataInteractor.saveCurrentData();
            setFill();
        }

    }

    private RawImage getBox(int turn)
    {
        switch(turn){
            case 3:
                return three;
            case 4:
                return four;
            case 5:
                return five;
            case 6:
                return six;
            case 7:
                return seven;
        }
        return null;
    }
}
