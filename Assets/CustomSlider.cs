using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject fillParent;
    [SerializeField] private Color emptyColor;
    [SerializeField] private Color filledColor;
    [SerializeField] private TextMeshProUGUI tm;
    private List<Image> fills;
    private float range;

    private void Awake()
    {
        range = slider.maxValue - slider.minValue;
        slider.onValueChanged.AddListener(delegate { UpdateSliderUI(); });
        if (tm != null)
        {
            slider.onValueChanged.AddListener(delegate { SetSliderText(); });
            SetSliderText();
        }
        fills = new List<Image>();
        foreach (Transform t in fillParent.transform)
        {
            fills.Add(t.gameObject.GetComponent<Image>());
        }
        UpdateSliderUI();
    }

    public void UpdateSliderUI()
    {
        int toFill = (int)Mathf.Floor(Mathf.Lerp(0, fills.Count, (slider.value - slider.minValue) / range));
        for (int i = 0; i < fills.Count; i++)
        {
            if (i <= toFill)
            {
                fills[i].color = filledColor;
            }
            else
            {
                fills[i].color = emptyColor;
            }
        }
    }

    public void SetSliderText()
    {
        tm.text = slider.value.ToString("0.000");
    }
}
