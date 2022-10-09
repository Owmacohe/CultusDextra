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
    bool isButton;

    TextAnimator anim;
    TextAnimatorPlayer animPlayer;
    string innerText;

    void Start()
    {
        anim = GetComponent<TextAnimator>();
        animPlayer = GetComponent<TextAnimatorPlayer>();
        
        innerText = GetComponent<TMP_Text>().text;
        
        SetText(animationType, amplitude);
        
        anim.onEvent += OnEvent;
    }

    void OnDestroy()
    {
        anim.onEvent -= OnEvent;
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

    void ChangeScene()
    {
        SceneChange.StaticChange("Main");
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

    void OnEvent(string message)
    {
        if (message.Equals("done"))
        {
            Invoke(nameof(ChangeScene), 2);
        }
    }
}