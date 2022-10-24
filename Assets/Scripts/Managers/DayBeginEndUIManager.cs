using System;
using Febucci.UI;
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

    bool hasCompletedText;
    TextAnimatorPlayer player;

    void Start()
    {
        PlayerStats playerStats = FindObjectOfType<PlayerStats>();
        Stats stats = playerStats.stats;
        player = info.GetComponent<TextAnimatorPlayer>();

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

    public void Click()
    {
        if (hasCompletedText)
        {
            SceneChange.StaticChange(isBegin ? "Main" : "Day Begin");
        }
        else
        {
            player.waitForNormalChars = 0.002f;
            player.waitLong = 0.1f;
            player.waitMiddle = 0.04f;

            hasCompletedText = true;
        }
    }

    void Lose()
    {
        Debug.Log("<b>DAY BEGIN/END:</b> lose");
        
        SceneChange.StaticChange("Title");
    }
}