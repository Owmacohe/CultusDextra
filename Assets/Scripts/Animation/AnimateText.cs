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
    [SerializeField]
    bool isButton = true;

    TextAnimator anim;
    string innerText;

    void Start()
    {
        anim = GetComponent<TextAnimator>();
        innerText = GetComponent<TMP_Text>().text;
        
        SetText(animationType, amplitude);
    }

    public void SetText(AnimTypes type, float a)
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
        
        anim.SetText("<" + typeString + " a=" + a + ">" + innerText + "</" + typeString + ">", false);
    }

    void OnMouseOver()
    {
        if (isButton)
        {
            SetText(animationType, amplitude * 2f);   
        }
    }

    void OnMouseExit()
    {
        if (isButton)
        {
            SetText(animationType, amplitude);
        }
    }
}