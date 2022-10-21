using System;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField]
    GameObject choppingCanvas;
    [SerializeField]
    AnimateText goalText, goalTextOffset;

    [HideInInspector]
    public bool isChopping;

    CultistManager cult;
    ChoppingController chop;
    PlayerStats playerStats;

    void Start()
    {
        cult = FindObjectOfType<CultistManager>();
        chop = FindObjectOfType<ChoppingController>();
        playerStats = FindObjectOfType<PlayerStats>();
        
        choppingCanvas.SetActive(false);
        
        Invoke(nameof(Initialize), 0.1f);
    }

    void Initialize()
    {
        Debug.Log("<b>VIEW:</b> initialize");
        
        UpdateUI();
        
        Debug.Log("<b>CULTISTS:</b> add (x" + (5 + playerStats.stats.Day) + ")");
        
        for (int i = 0; i < 5 + playerStats.stats.Day; i++)
        {
            cult.AddCultist();
        }

        StartCoroutine(cult.Reset(10));
    }
    
    public void ToggleView()
    {
        UpdateUI();
        
        if (isChopping)
        {
            Debug.Log("<b>VIEW:</b> cultists");
            
            choppingCanvas.SetActive(false);
            chop.canChop = false;
            StartCoroutine(cult.Reset(2));
        }
        else
        {
            Debug.Log("<b>VIEW:</b> chopping");
            
            choppingCanvas.SetActive(true);
            chop.canChop = true;
            StartCoroutine(chop.Reset(0.5f));
        }

        isChopping = !isChopping;
    }

    public void UpdateUI(int scoreAdd = 0)
    {
        Debug.Log("<b>VIEW:</b> update stats");
        
        Stats stats = playerStats.stats;

        playerStats.stats.Score[stats.Day] += scoreAdd;

        if (playerStats.stats.Score[stats.Day] < 0)
        {
            playerStats.stats.Score[stats.Day] = 0;
        }

        string temp = "Goal: " + stats.Goal[stats.Day] + "\nScore: " + stats.Score[stats.Day];
        goalText.SetText(AnimateText.AnimTypes.WIGGLE, 0.1f, temp);
        goalTextOffset.SetText(AnimateText.AnimTypes.WIGGLE, 0.1f, temp);
    }

    public Cultist Current()
    {
        return cult.current;
    }
}