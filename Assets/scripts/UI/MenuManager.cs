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
        //GameInstanceManager.Instance.UpdateGameState(GameState.Gameplay);
        SceneManager.LoadScene(firstLevelName);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(GameIsPaused){
                ResumeGame();
                Cursor.lockState = CursorLockMode.Locked;
            }
            else{
                PauseGame();
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        //GameInstanceManager.Instance.UnpauseGame();
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;

        //GameInstanceManager.Instance.PauseGame();
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }

}
