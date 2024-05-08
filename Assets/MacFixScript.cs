using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MacFixScript : MonoBehaviour
{
    [SerializeField] private Image bear;
    private float alpha = 1;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadLanguageScene());
    }

    private IEnumerator LoadLanguageScene()
    {
        Color c;
        for (float t = 0; t < 1.5f; t += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            c = Color.white;
            alpha = 1 - (t / 1.5f);
            c.a = alpha;
            bear.color = c;
        }

        c = Color.white;
        c.a = 0;
        bear.color = c;

        SceneManager.LoadScene("LanguageSelect");
    }
}
