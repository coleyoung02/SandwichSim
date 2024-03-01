using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { StartMenu, Gameplay, Paused}

public class GameInstanceManager : MonoBehaviour
{
    public static GameInstanceManager Instance { get; private set; }

    [SerializeField] private string pauseMenuSceneName = "PauseMenu";
    [SerializeField] private AudioSource music;

    [SerializeField] private GameState gameState;
    [SerializeField] private List<string> levels;
    private int levelIndex;

    protected void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Instance.music.volume = .1f;
        Instance.music.Play();
    }
    

    // Update is called once per frame
    void Update()
    {
        // Pause game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.Paused)
            {
                UnpauseGame();
            }
            else if (gameState == GameState.Gameplay)
            {
                PauseGame();
            }
        }
    }

    public bool isUnpaused()
    {
        return gameState == GameState.Gameplay;
    }

    public void PauseGame()
    {
        gameState = GameState.Paused;
        Time.timeScale = 0f;
        SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
    }

    public void UnpauseGame()
    {
        SceneManager.UnloadSceneAsync(pauseMenuSceneName);
        Time.timeScale = 1f;
        gameState = GameState.Gameplay;
    }

    public void UpdateGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    public void NextLevel()
    {
        Debug.LogWarning(levels[levelIndex]);
        SceneManager.UnloadSceneAsync(levels[levelIndex]);
        SceneManager.LoadScene(levels[levelIndex + 1], LoadSceneMode.Additive);
        levelIndex++;
    }

    public void LoadLevel(int i)
    {
        SceneManager.LoadScene(levels[i], LoadSceneMode.Additive);
        levelIndex = i;
    }
}
