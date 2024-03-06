using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private string firstLevelName = "FINAL";

    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;

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
