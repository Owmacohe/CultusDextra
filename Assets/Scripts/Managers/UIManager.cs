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

    public void UpdateUIAfterChop(bool chopped, bool faithful, bool escaped)
    {
        stats.UpdateAfterChop(chopped, faithful, escaped);
        
        UpdateUI();
    }

    // TODO: update anticipation
}