using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { StartMenu, Gameplay, Inactive }

public class GameInstanceManager : MonoBehaviour
{
    public static GameInstanceManager Instance { get; private set; }

    [SerializeField] private AudioSource music;

    [SerializeField] private GameState gameState = GameState.Inactive;
    [SerializeField] private List<string> levels;
    private int levelIndex = -1;
    private bool loading = false;

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
    }

    public void TurnOn()
    {
        gameState = GameState.StartMenu;
        LoadDesktop(true);
        FindFirstObjectByType<PlayerController>().LockControls(true);
    }
    
    public void TurnOnMusic()
    {
        music.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.StartMenu)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitDesktop();
            }
        }
        if (gameState == GameState.Gameplay)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ExitGame();
            }
        }
        // Pause game
    }

    public void LoadDesktop(bool fromScratch=false)
    {
        if (fromScratch)
        {
            SceneManager.LoadScene("BearOS", LoadSceneMode.Additive);
            return;
        }
        music.Stop();
        if (levelIndex >= 0)
        {
            SceneManager.UnloadSceneAsync(levels[levelIndex]);
            levelIndex = -1;
        }
        SceneManager.LoadScene("BearOS", LoadSceneMode.Additive);

    }

    public bool isUnpaused()
    {
        return gameState == GameState.Gameplay;
    }

    public void ExitGame()
    {
        gameState = GameState.StartMenu;
        if (levelIndex >= 0)
        {
            LoadDesktop();
        }
    }

    public void ExitDesktop()
    {
        SceneManager.UnloadSceneAsync("BearOS");
        gameState = GameState.Inactive;
        foreach (Computer c in FindObjectsByType<Computer>(FindObjectsSortMode.None))
        {
            c.TurnOff();
        }
        FindFirstObjectByType<PlayerController>().LockControls(false);
    }

    public void UnpauseGame()
    {
        FindAnyObjectByType<PauseManager>().TogglePause(false);
        Time.timeScale = 1f;
        gameState = GameState.Gameplay;
    }

    public void UpdateGameState(GameState newGameState)
    {
        gameState = newGameState;
    }

    public void NextLevel()
    {
        SceneManager.UnloadSceneAsync(levels[levelIndex]);
        SceneManager.LoadScene(levels[levelIndex + 1], LoadSceneMode.Additive);
        levelIndex++;
    }

    public void LoadLevel(int i, bool fromDesktop=false)
    {
        gameState = GameState.Gameplay;
        if (fromDesktop)
        {
            SceneManager.UnloadSceneAsync("BearOS");
        }
        SceneManager.LoadScene(levels[i], LoadSceneMode.Additive);
        levelIndex = i;
    }

    public void Retry()
    {
        if (!loading)
        {
            loading = true;
            StartCoroutine(StartRetry());
        }
    }

    IEnumerator StartRetry()
    {
        AsyncOperation ao = SceneManager.UnloadSceneAsync(levels[levelIndex]);
        yield return ao; 
        ao = SceneManager.LoadSceneAsync(levels[levelIndex], LoadSceneMode.Additive);
        yield return ao;
        loading = false;
    }
}
