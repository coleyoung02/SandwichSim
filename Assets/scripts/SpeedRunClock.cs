using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedRunClock : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tm;
    private float time;
    void Start()
    {
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        tm.text = FormatTime();
    }


    private string FormatTime()
    {
        string minutes = ((int)(time / 60)).ToString();
        if (minutes.Length == 0) 
        {
            minutes = "00";
        }
        else if (minutes.Length == 1)
        {
            minutes = "0" + minutes;
        }
        return minutes + ":" + (time % 60).ToString("00.00");
    }
}
