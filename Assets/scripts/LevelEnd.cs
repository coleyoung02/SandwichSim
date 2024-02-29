using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] private LevelExit bear;
    [SerializeField] private LevelExit bees;

    public void tryExit()
    {
        if (bear.Done() && bees.Done())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
