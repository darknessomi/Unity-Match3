using UnityEngine;
using System.Collections;

public class SetBackground : MonoBehaviour
{

    public Sprite[] Background;     // array background

    void Start()
    {
        // set background image by PLayerInfo.BACKGROUND
        GetComponent<SpriteRenderer>().sprite = Background[PLayerInfo.BACKGROUND];
    }

}
