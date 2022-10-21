using System;
using Febucci.UI;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
[RequireComponent(typeof(TextAnimator))]
public class AnimateText : MonoBehaviour
{
    public enum AnimTypes { WIGGLE, SHAKE }
    [SerializeField]
    AnimTypes animationType;
    [SerializeField, Range(0, 1)]
    float amplitude = 0.1f;

    TextAnimator anim;
    TextAnimatorPlayer animPlayer;
    string innerText;

    void Start()
    {
        anim = GetComponent<TextAnimator>();
        animPlayer = GetComponent<TextAnimatorPlayer>();
        
        innerText = GetComponent<TMP_Text>().text;
        
        SetText(animationType, amplitude);
    }

    public void SetText(AnimTypes type, float a, string newText = "")
    {
        string typeString;

        switch (type)
        {
            case AnimTypes.SHAKE:
                typeString = "shake";
                break;
            default:
                typeString = "wiggle";
                break;
        }

        if (newText != "")
        {
            innerText = newText;
        }

        if (anim != null)
        {
            string temp = "<" + typeString + " a=" + a + ">" + innerText + "</" + typeString + ">";
            
            if (animPlayer != null)
            {
                animPlayer.ShowText(temp);
            }
            else
            {
                anim.SetText(temp, false);   
            }
        }
    }
}