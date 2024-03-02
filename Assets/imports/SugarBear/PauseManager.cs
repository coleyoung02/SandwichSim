using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseUI;
    public void TogglePause(bool pause)
    {
        pauseUI.SetActive(pause);
    }
}
