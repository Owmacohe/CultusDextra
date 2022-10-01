using System;
using UnityEngine;

public class ViewManager : MonoBehaviour
{
    [SerializeField]
    GameObject choppingCanvas;

    bool isChopping;

    CultistManager cult;
    ChoppingManager chop;

    void Start()
    {
        cult = GetComponent<CultistManager>();
        chop = GetComponent<ChoppingManager>();
        
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
            StartCoroutine(cult.Reset(2));
        }
        else
        {
            choppingCanvas.SetActive(true);
            StartCoroutine(chop.Reset(0.5f));
        }

        isChopping = !isChopping;
    }

    public Cultist Current()
    {
        return cult.current;
    }
}