using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoadTransition : MonoBehaviour
{
    [SerializeField] private float transitionTime;


    // Start is called before the first frame update
    void Start()
    {
        LoadNextLevel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameInstanceManager.Instance.NextLevel();
        }
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {

        yield return new WaitForSeconds(transitionTime);

        GameInstanceManager.Instance.NextLevel();
    }
}
