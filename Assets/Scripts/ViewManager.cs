using System;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField]
    GameObject choppingCanvas;

    [HideInInspector]
    public bool isChopping;

    CultistManager cult;
    ChoppingController chop;

    void Start()
    {
        cult = FindObjectOfType<CultistManager>();
        chop = FindObjectOfType<ChoppingController>();
        
        choppingCanvas.SetActive(false);
        
        Invoke(nameof(Initialize), 0.1f);
    }

    void Initialize()
    {
        for (int i = 0; i < 10; i++)
        {
            cult.AddCultist();
        }

        StartCoroutine(cult.Reset(10));
    }
    
    public void ToggleView()
    {
        if (isChopping)
        {
            choppingCanvas.SetActive(false);
            chop.canChop = false;
            //cult.NewDay();
            StartCoroutine(cult.Reset(2));
        }
        else
        {
            choppingCanvas.SetActive(true);
            chop.canChop = true;
            StartCoroutine(chop.Reset(0.5f));
        }

        isChopping = !isChopping;
    }

    public Cultist Current()
    {
        return cult.current;
    }
}