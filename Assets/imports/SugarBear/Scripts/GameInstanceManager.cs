using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { StartMenu, Gameplay, Inactive, Paused }

public class GameInstanceManager : MonoBehaviour
{
    public static GameInstanceManager Instance { get; private set; }

    [SerializeField] private AudioSource music;

    [SerializeField] private GameState gameState = GameState.Inactive;
    [SerializeField] private List<string> levels;
    [SerializeField] private PlayerController pc;
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
        gameState = GameState.Inactive;
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

    public void SetPlayer(PlayerController p)
    {
        pc = p;
    }

    // Update is called once per frame
    void Update()
    {
        Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.StartMenu)
            {
                ExitDesktop();
            }
            else if (gameState == GameState.Gameplay)
            {
                ExitGame();
            }
            else if (pc == null)
            {
                // if there is no player in the scene, ignore
                return;
            }
            else if (gameState == GameState.Inactive)
            {

                pc.SetPause(true);
                gameState = GameState.Paused;
            }
            else if (gameState == GameState.Paused)
            {
                pc.SetPause(false);
                gameState = GameState.Inactive;
            }
        }
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
        if (gameState == GameState.Paused)
        {
            pc.SetPause(false);
            gameState = GameState.Inactive;
        }
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