using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    //[SerializeField] private string firstLevelName = "Level_1";

    public void StartGame()
    {
        //GameInstanceManager.Instance.UpdateGameState(GameState.Gameplay);
        //SceneManager.LoadScene(firstLevelName);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ResumeGame()
    {
        GameInstanceManager.Instance.UnpauseGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
