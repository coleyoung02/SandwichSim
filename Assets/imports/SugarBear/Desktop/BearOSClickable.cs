using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearOSClickable : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Color highlightColor;
    [SerializeField] private bool launcesLevel;
    [SerializeField] private int levelIndex;
    [SerializeField] private GameObject activates;
    [SerializeField] private GameObject deactivates;
    [SerializeField] private bool inWindow;
    private Color baseColor;
    private static bool windowUp;

    private void Start()
    {
        baseColor = sprite.color;
        windowUp = false;
    }

    bool hovered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hovered = true;
        sprite.color = highlightColor;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hovered = false;
        sprite.color = baseColor;
    }

    private void Update()
    {
        if (hovered && Input.GetMouseButtonDown(0) && (!windowUp || inWindow))
        {
            if (activates != null)
            {
                activates.SetActive(true);
                windowUp = true;
            }
            if (deactivates != null)
            {
                deactivates.SetActive(false);
                windowUp = false;
            }
            if (launcesLevel)
            {
                windowUp = true;
                GameInstanceManager.Instance.LoadLevel(levelIndex, true);
            }
        }
    }

}
