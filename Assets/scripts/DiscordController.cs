using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;
using Steamworks;

public class DiscordController : MonoBehaviour
{
    private const long CLIENT_ID = 1215021657744216074;
    private Discord.Discord discord;

    [Space]
    private string details = "Simulating Sandwiches";
    [Space]
    private string largeImage = "sandwichbear";
    private string largeText = "Sandwich Sim";

    private long time;

    private bool waiting = false;

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            discord = new Discord.Discord(CLIENT_ID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
        }
        catch 
        {
        }
        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();
    }

    void OnApplicationQuit()
    {
        StopAllCoroutines();
        if (discord != null)
        {
            discord.Dispose();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (discord == null)
        {
            if (!waiting)
            {
                waiting = true;
                StartCoroutine(ReTryDiscordConnect());
            }
        }
        else
        {
            try
            {
                MainUpdate();
                discord.RunCallbacks();
            }
            catch
            {
                discord = null;
            }
        }
    }

    private IEnumerator ReTryDiscordConnect()
    {
        yield return new WaitForSecondsRealtime(10f);
        try
        {
            discord = new Discord.Discord(CLIENT_ID, (UInt64)Discord.CreateFlags.NoRequireDiscord);
        }
        catch
        {
        }
        waiting = false;
    }

    private void MainUpdate()
    {
        var activityManager = discord.GetActivityManager();
        var activity = new Discord.Activity
        {
            Details = details,
            Assets =
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
            Timestamps =
                {
                    Start = time
                }
        };
        activityManager.UpdateActivity(activity, (res) =>
        {
            if (res != Discord.Result.Ok) Debug.LogError("Failed connecting to Discord!");
        });


    }
}
