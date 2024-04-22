using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PngLocalizer : MonoBehaviour
{
    [SerializeField] private Sprite spanishSprite;

    private void Awake()
    {
        if (GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            SpriteRenderer sprite = GetComponent<SpriteRenderer>();
            Image img = GetComponent<Image>();
            if (sprite != null)
            {
                sprite.sprite = spanishSprite;
            }
            if (img != null)
            {
                img.sprite = spanishSprite;
            }
        }
    }
}
