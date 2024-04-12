using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI tm;
    [SerializeField] private string spanishText;
    [SerializeField] private bool toggle;
    private string baseText;
    [SerializeField] private bool toggleActiveDefault;
    [SerializeField] private string playerPrefsIntName;
    private AudioManager audioManager;
    private bool toggleActive;
    private bool hovered = false;

    void Awake()
    {
        if (GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            if (spanishText != null && spanishText.Length > 0)
            {
                tm.text = spanishText;
            }
        }

        audioManager = FindFirstObjectByType<AudioManager>();
        baseText = tm.text;
        if (toggle)
        {
            int pref = PlayerPrefs.GetInt(playerPrefsIntName, -1);
            if (pref >= 0)
            {
                if (pref == 0)
                {
                    //Debug.Log("I " + gameObject.name + " am INactive because of playerpref");
                    toggleActive = false;
                }
                else
                {
                    toggleActive = true;
                    //Debug.Log("I " + gameObject.name + " am active because of playerpref");

                }
            }
            else if (!toggleActiveDefault)
            {
                toggleActive = false;
                //Debug.Log("I " + gameObject.name + " am INactive because of NO playerpref");

            }
            else
            {
                toggleActive = true;
                //Debug.Log("I " + gameObject.name + " am active because of NO playerpref");
            }
            tm.text = GetText();
        }
        UnityEngine.UI.Button b = GetComponent<UnityEngine.UI.Button>();
        b.onClick.AddListener(PlaySound);
    }

    private void OnEnable()
    {
        hovered = false;
        if (baseText != null)
        {
            tm.text = GetText();
        }
    }

    public void SetToggle(bool t, bool refreshUI=true)
    {
        toggleActive = t;
        if (refreshUI)
        {
            if (hovered)
            {
                tm.text = "[" + GetText() + "]";
            }
        }
    }

    private void PlaySound()
    {
        audioManager.PlayPooledClip(ClipPool.BUTTON);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
        tm.text = GetText();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
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
