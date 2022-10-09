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

    Stats stats;
    AnimateText anim;

    void Start()
    {
        stats = new Stats();

        anim = anticipationBuildup.GetComponent<AnimateText>();
        
        UpdateUI();
    }

    void UpdateUI()
    {
        Debug.Log("<b>UI:</b> update all");
        
        totalCultistsChopped.text = "Total cultists chopped: " + stats.TotalChopped;
        faithfulChopped.text = "Faithful chopped: " + stats.FaithfulChopped;
        traitorsChopped.text = "Traitors chopped: " + stats.TraitorsChopped;
        
        cultistsMissed.text = "Cultists missed: " + stats.TotalMissed;
        traitorsEscaped.text = "Traitors escaped: " + stats.TotalEscaped;
        
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

        anticipationVignette.color = new Color(100, 0, 0, anticipation / 10f);
        
        anim.SetText(AnimateText.AnimTypes.SHAKE, (anticipation * 0.02f) + 0.1f, anticipationBuildup.text);
    }

    public void UpdateUIAfterChop(bool chopped, bool faithful, bool escaped)
    {
        Debug.Log("<b>UI:</b> update stats");
        
        stats.UpdateAfterChop(chopped, faithful, escaped);
        
        UpdateUI();
    }
}