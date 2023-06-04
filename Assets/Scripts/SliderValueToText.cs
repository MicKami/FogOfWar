using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderValueToText : MonoBehaviour
{
    [SerializeField]
    private Slider source;
    private TextMeshProUGUI text;

    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        UpdateSliderValueText();
    }

    public void UpdateSliderValueText()
    {
        text.text = source.value.ToString();
    }
}
