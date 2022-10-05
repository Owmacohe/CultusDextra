using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text totalCultistsChopped, faithfulChopped, traitorsChopped;
    [SerializeField]
    TMP_Text cultistsMissed, traitorsEscaped;
    [SerializeField]
    TMP_Text anticipationBuildup;

    Stats stats;

    void Start()
    {
        stats = new Stats();
        UpdateUI();
    }

    void UpdateUI()
    {
        totalCultistsChopped.text = "Total cultists chopped: " + stats.TotalChopped;
        faithfulChopped.text = "Faithful chopped: " + stats.FaithfulChopped;
        traitorsChopped.text = "Traitors chopped: " + stats.TraitorsChopped;
        
        cultistsMissed.text = "Cultists missed: " + stats.TotalMissed;
        traitorsEscaped.text = "Traitors escaped: " + stats.TotalEscaped;
    }

    public void UpdateUIAnticipation(float anticipation)
    {
        if (anticipation > 10)
        {
            anticipation = 10;
        }
        
        anticipationBuildup.text = "Anticipation: " + (Mathf.Round(anticipation * 100f) / 100f);
    }

    public void UpdateUIAfterChop(bool chopped, bool faithful, bool escaped)
    {
        stats.UpdateAfterChop(chopped, faithful, escaped);
        
        UpdateUI();
    }
}