using System;
using TMPro;
using UnityEngine;

public class DayBeginEndUIManager : MonoBehaviour
{
    [SerializeField]
    bool isBegin;
    [SerializeField]
    TMP_Text day, info;
    [SerializeField]
    TextAsset narrative;
    [SerializeField]
    GameObject cont, click;

    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        Stats stats = playerStats.stats;

        if (isBegin)
        {
            playerStats.stats.NewDay();
        }
        
        int playerDay = stats.Day;

        day.text = "Day " + (playerDay + 1);
        
        if (isBegin)
        {
            Debug.Log("<b>DAY BEGIN/END:</b> load begin");
            
            string[] split = narrative.text.Split("[DAY]");

            if (playerDay < split.Length)
            {
                info.text = split[playerDay].Trim();   
            }
            else
            {
                info.text = 
                    "<font=Essays1743-Italic SDF><color=#C90000>"
                    + "Goal: " + stats.Goal[playerDay]
                    + "\n\nThe possibility for traitors has increased.";
            }
        }
        else
        {
            Debug.Log("<b>DAY BEGIN/END:</b> load end");
            
            info.text = playerStats.stats.ToString();

            if (stats.Score[playerDay] < stats.Goal[playerDay])
            {
                cont.SetActive(false);
                click.SetActive(false);

                playerStats.Reset();
                
                Invoke(nameof(Lose), 12);
            }
        }
    }

    void Lose()
    {
        Debug.Log("<b>DAY BEGIN/END:</b> lose");
        
        SceneChange.StaticChange("Title");
    }
}