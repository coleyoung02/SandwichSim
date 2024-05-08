using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarBearWin : MonoBehaviour
{
    void Start()
    {
        if (SteamManager.Initialized)
        {
            Steamworks.SteamUserStats.GetAchievement("SUGAR_BEAR_1_1", out bool won);
            if (!won)
            {
                SteamUserStats.SetAchievement("SUGAR_BEAR_1_1");
                SteamUserStats.StoreStats();
            }
        }
    }
}
