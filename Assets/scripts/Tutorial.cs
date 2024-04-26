using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private List<string> tutorialTexts;
    [SerializeField] private List<string> spanishTexts;
    [SerializeField] private TextMeshProUGUI tm;

    private int textIndex = 0;

    private void Start()
    {
        Time.timeScale = 1.0f;
        StartCoroutine(Unpause());
        if(GameInstanceManager.Instance != null && GameInstanceManager.Instance.IsSpanishMode())
        {
            tutorialTexts = spanishTexts;
        }
        tm.text = tutorialTexts[0];
    }

    private IEnumerator Unpause()
    {
        yield return new WaitForSeconds(.05f);
        FindFirstObjectByType<MenuManager>().PauseGame();
        Cursor.lockState = CursorLockMode.None;
    }

    private void Update()
    {
        if (textIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextText();
                StopAllCoroutines();
                StartCoroutine(WaitAndUpdate(8f));
            }
        }
        if (textIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                StopAllCoroutines();
                StartCoroutine(WaitAndUpdate(3f));
            }
        }
        if (textIndex == 3)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                NextText();
                StopAllCoroutines();
                StartCoroutine(WaitAndUpdate(3f));
            }
        }
        if (textIndex == 7)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StopAllCoroutines();
                StartCoroutine(WaitAndUpdate(3f));
            }
        }
        if (textIndex == 8)
        {
            if (Input.GetMouseButtonDown(1))
            {
                StopAllCoroutines();
                StartCoroutine(WaitAndUpdate(4f));
            }
        }
        if (textIndex == 9)
        {
            NextText();
            StopAllCoroutines();
            StartCoroutine(WaitAndUpdate(4f));
        }
        if (textIndex == 11)
        {
            SceneManager.LoadSceneAsync("FINAL");
            textIndex = 12;
        }
    }

    public void NextText(int i = -1)
    {
        if (i == -1)
        {
            textIndex++;
        }
        else
        {
            if (i == textIndex)
            {
                textIndex++;
            }
        }
        tm.text = tutorialTexts[textIndex];
    }

    private IEnumerator WaitAndUpdate(float delay)
    {
        yield return new WaitForSeconds(delay);
        NextText();
    }

}
