using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI tm;
    private string baseText;

    void Start()
    {
        baseText = tm.text;
    }
    private void OnEnable()
    {
        if (baseText != null)
        {
            tm.text = baseText;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tm.text = baseText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tm.text = "[" + baseText + "]";
    }
}
