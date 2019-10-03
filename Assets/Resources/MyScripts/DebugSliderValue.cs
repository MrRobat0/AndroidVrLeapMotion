using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugSliderValue : MonoBehaviour
{

    [SerializeField]
    float sliderValue;



    public void SetSliderValue(float value)
    {
        sliderValue = value;
        Debug.Log("Slider current value is : " + sliderValue);
    }
}
