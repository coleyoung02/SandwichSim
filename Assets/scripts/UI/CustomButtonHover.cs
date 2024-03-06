using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI tm;
    [SerializeField] private bool toggle;
    private string baseText;
    [SerializeField] private bool toggleActiveDefault;
    [SerializeField] private string playerPrefsIntName;
    private bool toggleActive;

    void Start()
    {
        baseText = tm.text;
        if (toggle)
        {
            int pref = PlayerPrefs.GetInt(playerPrefsIntName, -1);
            if (pref >= 0)
            {
                if (pref == 0)
                {
                    toggleActive = false;
                }
                else
                {
                    toggleActive = true;
                }
            }
            else if (!toggleActiveDefault)
            {
                toggleActive = false;
            }
            else
            {
                toggleActive = true;
            }
            tm.text = GetText();
        }
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
        tm.text = GetText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tm.text = "[" + GetText() + "]";
    }

    private string GetText()
    {
        if (!toggle)
        {
            return baseText;
        }
        if (toggleActive)
        {
            return baseText + " (X)";
        }
        else
        {
            return baseText + " ( )";
        }
    }
}
