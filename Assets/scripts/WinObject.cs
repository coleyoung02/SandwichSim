using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinObject : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WinEndUI());
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement("WIN_1_0", out bool won);
            if (!won)
            {
                SteamUserStats.SetAchievement("WIN_1_0");
                SteamUserStats.StoreStats();
            }
        }
    }

    private IEnumerator WinEndUI()
    {
        yield return new WaitForSeconds(5f);
        FindFirstObjectByType<MenuManager>().ShowUI();
    }
}
