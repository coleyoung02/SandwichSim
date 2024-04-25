using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextLocalizer : MonoBehaviour
{
    [SerializeField] private string spanishText;
    [TextArea]
    [SerializeField]
    private string blockSpanishText;
    [SerializeField] private bool shrinkToFit = false;

    private void Awake()
    {
        if (blockSpanishText != null && blockSpanishText.Length > 0)
        {
            spanishText = blockSpanishText;
            // this is literally only used once for the sugar bear text scroll
            if (shrinkToFit)
            {
                GetComponent<TextMeshProUGUI>().fontSize = GetComponent<TextMeshProUGUI>().fontSize - 6;
            }
        }
        if (GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            if (GetComponent<TextMeshPro>() != null)
            {
                GetComponent<TextMeshPro>().text = spanishText;
            }
            if (GetComponent<TextMeshProUGUI>())
            {
                GetComponent<TextMeshProUGUI>().text = spanishText;
            }
        }
    }
}
