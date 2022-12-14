using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject goodOutcome, badOutcome;
    [SerializeField]
    TMP_Text goodText, badText, anticipationBuildup;
    [SerializeField]
    Image anticipationVignette;

    PlayerStats playerStats;
    AnimateText anim;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        anim = anticipationBuildup.GetComponent<AnimateText>();
    }

    public void HideOutcomes()
    {
        goodOutcome.SetActive(false);
        badOutcome.SetActive(false);
    }

    public void UpdateUIAnticipation(float anticipation)
    {
        Debug.Log("<b>UI:</b> update anticipation");
            
        anticipationBuildup.gameObject.SetActive(true);
            
        if (anticipation > 10)
        {
            anticipation = 10;
        }
        
        anticipationBuildup.text = "Anticipation: " + Mathf.Round(anticipation);
        anticipationBuildup.fontSize = (anticipation * 2f) + 20f;

        anticipationVignette.color = new Color(0.4f, 0, 0, anticipation / 10f);

        if (anim == null)
        {
            anim = anticipationBuildup.GetComponent<AnimateText>();
        }
        
        anim.SetText(AnimateText.AnimTypes.SHAKE, (anticipation * 0.02f) + 0.1f, anticipationBuildup.text); 
    }

    public void UpdateUIAfterChop(bool chopped, bool faithful, bool escaped)
    {
        Debug.Log("<b>UI:</b> update stats");

        playerStats.stats.UpdateAfterChop(chopped, faithful, escaped);

        if (chopped)
        {
            goodOutcome.SetActive(true);

            goodText.text = faithful ? "Faithful chopped!" : "Traitor chopped!";
        }
        else
        {
            badOutcome.SetActive(true);

            badText.text = escaped ? "Traitor escaped!" : "Cultist missed!";
        }
    }
}