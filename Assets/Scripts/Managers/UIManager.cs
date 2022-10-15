using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TMP_Text totalCultistsChopped, faithfulChopped, traitorsChopped;
    [SerializeField]
    TMP_Text cultistsMissed, traitorsEscaped;
    [SerializeField]
    TMP_Text anticipationBuildup;
    [SerializeField]
    Image anticipationVignette;

    PlayerStats playerStats;
    AnimateText anim;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        anim = anticipationBuildup.GetComponent<AnimateText>();
        
        UpdateUI();
    }

    void UpdateUI()
    {
        Debug.Log("<b>UI:</b> update all");

        Stats stats = playerStats.stats;
        int day = stats.Day;

        totalCultistsChopped.text = "Total cultists chopped: " + stats.TotalChopped[day];
        faithfulChopped.text = "Faithful chopped: " + stats.FaithfulChopped[day];
        traitorsChopped.text = "Traitors chopped: " + stats.TraitorsChopped[day];
        
        cultistsMissed.text = "Cultists missed: " + stats.TotalMissed[day];
        traitorsEscaped.text = "Traitors escaped: " + stats.TotalEscaped[day];
        
        UpdateUIAnticipation(0);
    }

    public void UpdateUIAnticipation(float anticipation)
    {
        Debug.Log("<b>UI:</b> update anticipation");
        
        if (anticipation > 10)
        {
            anticipation = 10;
        }
        
        anticipationBuildup.text = "Anticipation: " + Mathf.Round(anticipation);
        anticipationBuildup.fontSize = (anticipation * 2f) + 20f;

        anticipationVignette.color = new Color(0.4f, 0, 0, anticipation / 10f);
        
        anim.SetText(AnimateText.AnimTypes.SHAKE, (anticipation * 0.02f) + 0.1f, anticipationBuildup.text);
    }

    public void UpdateUIAfterChop(bool chopped, bool faithful, bool escaped)
    {
        Debug.Log("<b>UI:</b> update stats");
        
        playerStats.stats.UpdateAfterChop(chopped, faithful, escaped);
        
        UpdateUI();
    }
}