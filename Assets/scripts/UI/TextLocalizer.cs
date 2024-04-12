using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLocalizer : MonoBehaviour
{
    [SerializeField] private string spanishText;

    private void Awake()
    {
        if (GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            GetComponent<TextMeshProUGUI>().text = spanishText;
        }
    }
}
