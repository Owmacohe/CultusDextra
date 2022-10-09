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
        Debug.Log("<b>VIEW:</b> initialize");
        
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
            Debug.Log("<b>VIEW:</b> cultists");
            
            choppingCanvas.SetActive(false);
            chop.canChop = false;
            //cult.NewDay();
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

    public Cultist Current()
    {
        return cult.current;
    }
}