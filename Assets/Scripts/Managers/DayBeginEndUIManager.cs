using System;
using TMPro;
using UnityEngine;

public class DayBeginEndUIManager : MonoBehaviour
{
    [SerializeField]
    bool isBegin;
    [SerializeField]
    TMP_Text day, info;

    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();

        if (isBegin)
        {
            playerStats.stats.NewDay();
        }

        day.text = "Day " + (playerStats.stats.Day + 1);
        
        if (isBegin)
        {
            info.text = "test";
        }
        else
        {
            info.text = playerStats.stats.ToString();
        }
    }
}