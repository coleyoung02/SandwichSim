using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    private bool fadingIn = false;
    private bool fadingOut = false;
    private float a = 0f;
    [SerializeField] private Image image;
    [SerializeField] private float fadeDuration;
    private bool winOnFinish = false;

    //might allow fade out eventually
    public void BeginFade(bool fadeIn=true)
    {
        fadingIn = true;
        image.gameObject.SetActive(true);
    }

    public void SetWinOnFinish(bool shouldWin)
    {
        winOnFinish = shouldWin;
    }

    private void Update()
    {
        if (fadingIn)
        {
            a += Time.deltaTime * (1f / fadeDuration);
            if (a >= 1f && winOnFinish)
            {
                SceneManager.LoadScene("WinScene");
            }
            Color c = image.color;
            c.a = a;
            image.color = c;
            Debug.LogWarning(image.color.a);

        }
    }
}
