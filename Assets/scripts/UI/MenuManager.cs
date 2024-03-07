using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string firstLevelName = "FINAL";
    [SerializeField] private TextMeshProUGUI resText;
    [SerializeField] private GameObject noirObject;
    [SerializeField] private GameObject clockObject;
    [SerializeField] private CustomButtonHover fullscreenToggle;
    [SerializeField] private CustomButtonHover noirToggle;
    [SerializeField] private CustomButtonHover vhsToggle;
    [SerializeField] private CustomButtonHover clockToggle;

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

    private int maxWidth;
    private int maxHeight;
    private int[,] validResolutions;
    private int currentRes;
    private bool fullscreen;
    private bool noirMode;
    private bool vhsScan;
    private bool clock;

    void Awake()
    {
        maxHeight = Display.main.systemHeight;
        maxWidth = Display.main.systemWidth;
        validResolutions = new int[6, 2] { { 1280, 720 }, { 1366, 768 }, { 1600, 900 }, { 1920, 1080 }, { 2560, 1440 }, { 3840, 2160 } };
        int usedIndex = -1;
        fullscreen = PlayerPrefs.GetInt("Fullscreen", -1) != 0;
        noirMode = PlayerPrefs.GetInt("Noir", -1) == 1;
        clock = PlayerPrefs.GetInt("Scan", -1) != 0;
        vhsScan = PlayerPrefs.GetInt("Clock", -1) == 1;
        if (PlayerPrefs.GetInt("Res", -1) == -1)
        {
            for (int i = 0; i < validResolutions.GetLength(0); ++i)
            {
                if (validResolutions[i, 0] <= maxWidth && validResolutions[i, 1] <= maxHeight)
                {
                    usedIndex = i;
                    if (validResolutions[i, 0] == maxWidth && validResolutions[i, 1] == maxHeight)
                    {
                        fullscreen = true;
                    }
                }
            }
            currentRes = usedIndex;
            setResText();
        }
        else
        {
            currentRes = PlayerPrefs.GetInt("Res");
            SaveResolution();
            setResText();
        }
        SetUI();
    }

    private void SetUI()
    {
        noirToggle.SetToggle(noirMode, false);
        vhsToggle.SetToggle(vhsScan, false);
        clockToggle.SetToggle(clock, false);
        if (noirObject != null)
        {
            noirObject.SetActive(noirMode);
        }
        if (clockObject != null)
        {
            clockObject.SetActive(clock);
        }
    }

    public void ToggleNoir()
    {
        noirMode = !noirMode;
        if (noirMode)
        {
            PlayerPrefs.SetInt("Noir", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Noir", 0);
        }
        if (noirObject != null)
        {
            noirObject.SetActive(noirMode);
        }
        noirToggle.SetToggle(noirMode);
    }

    public void ToggleClock()
    {
        clock = !clock;
        if (clock)
        {
            PlayerPrefs.SetInt("Clock", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Clock", 0);
        }
        if (clockObject != null)
        {
            clockObject.SetActive(clock);
        }
        clockToggle.SetToggle(clock);
    }

    public void ToggleVHS()
    {
        vhsScan = !vhsScan;
        if (vhsScan)
        {
            PlayerPrefs.SetInt("Scan", 1);
            Shader.SetGlobalFloat("_SLineOn", 1f);
        }
        else
        {
            PlayerPrefs.SetInt("Scan", 0);
            Shader.SetGlobalFloat("_SLineOn", -1f);
        }
        vhsToggle.SetToggle(vhsScan);
    }

    public void ToggleFullscreen()
    {
        fullscreen = !fullscreen;
        fullscreenToggle.SetToggle(fullscreen);
        SaveResolution();
    }

    public void BumpResolution(bool up)
    {
        if (up && currentRes != validResolutions.Length &&
            validResolutions[currentRes + 1, 0] <= maxWidth && validResolutions[currentRes + 1, 1] <= maxHeight)
        {
            currentRes += 1;
        }
        else if (!up && currentRes != 0)
        {
            currentRes -= 1;
        }
        setResText();
        PlayerPrefs.SetInt("Res", currentRes);
        SaveResolution();
    }

    public void SaveResolution()
    {
        if (currentRes >= 0)
        {
            Screen.SetResolution(validResolutions[currentRes, 0], validResolutions[currentRes, 1], fullscreen);
        }
        if (fullscreen)
        {
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
    }

    private void setResText()
    {
        if (currentRes >= 0)
        {
            resText.text = validResolutions[currentRes, 0] + "x" + validResolutions[currentRes, 1];
        }
        else
        {
            resText.text = Screen.width + "x" + Screen.height;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void SetPause(bool p)
    {
        if (!p)
        {
            ResumeGame();
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            PauseGame();
            Cursor.lockState = CursorLockMode.None;
        }
    }


    public void UIUnpause()
    {
        GameInstanceManager.Instance.UnpauseGame();
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

}
