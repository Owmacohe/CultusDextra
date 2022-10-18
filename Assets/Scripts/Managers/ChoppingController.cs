﻿using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChoppingController : MonoBehaviour
{
    [SerializeField]
    GameObject hand;
    [SerializeField]
    float handSpeed = 0.15f;
    [SerializeField, Range(0, 1)]
    float traitorPullChance = 0.1f;
    [SerializeField]
    UIManager UI;
    [SerializeField]
    Transform cleaver;
    [SerializeField]
    float cleaverSpeed = 1;

    [SerializeField]
    SoundEffectManager withdrawl, gain, loss, chop, anticipation, breathing, coughing;

    [HideInInspector] public bool canChop;
    bool isInPosition, hasChopped;
    float anticipationStartTime;

    ViewManager view;

    void Start()
    {
        view = FindObjectOfType<ViewManager>();
    }

    void Update()
    {
        if (canChop && !hasChopped && Input.GetMouseButtonDown(0))
        {
            Chop();
        }
    }

    void FixedUpdate()
    {
        if (!hasChopped && isInPosition)
        {
            float temp = (Time.time - anticipationStartTime) * 2f;
            
            UI.UpdateUIAnticipation(temp);

            if (Time.time % 0.5f == 0 && temp < 10)
            {
                view.UpdateUI(1);   
            }

            if (
                canChop
                && view != null && view.Current() != null && view.Current().IsTraitor
                && (Random.Range(0f, 1f) <= traitorPullChance || (Time.time - anticipationStartTime >= 2.5f)))
            {
                Debug.Log("<b>CHOPPING:</b> pull away");
                
                view.UpdateUI(-5);
                
                hasChopped = true;
                StartCoroutine(SlideHand(false, true));
                loss.Play();
                withdrawl.Play();
                
                breathing.Stop();
                coughing.makeSoundsRandomly = false;

                UI.UpdateUIAfterChop(false, false, true);
            }
        }
    }

    IEnumerator SlideHand(bool up, bool switchOnFinish = false)
    {
        if (!hasChopped || !up)
        {
            if (up)
            {
                Debug.Log("<b>CHOPPING:</b> hand slide up");
            }
            else
            {
                Debug.Log("<b>CHOPPING:</b> hand slide down");
            }
            
            isInPosition = false;
        
            Vector2 direction = Vector2.up;
            float speed = handSpeed;

            if (!up)
            {
                direction = Vector2.down;
                speed *= 25;
            }

            if (!up)
            {
                yield return new WaitForSeconds(0.2f);
            }

            while ((up && !hasChopped && hand.transform.localPosition.y < 0)
                   || (!up && hand.transform.localPosition.y > -350))
            {
                hand.transform.localPosition += (Vector3)(direction * speed);
                yield return new WaitForSeconds(0);

                if (up && !isInPosition && hand.transform.localPosition.y > -50)
                {
                    isInPosition = true;
                    anticipationStartTime = Time.time;
                }
            }

            if (switchOnFinish)
            {
                yield return new WaitForSeconds(2);
                view.ToggleView();
            }
        }
    }

    void Chop()
    {
        hasChopped = true;
        
        StartCoroutine(ChopAnimation());
        breathing.Stop();
        coughing.makeSoundsRandomly = false;

        if (isInPosition)
        {
            view.UpdateUI(5);
            
            if (view.Current().IsTraitor)
            {
                Debug.Log("<b>CHOPPING:</b> traitor chop");
                UI.UpdateUIAfterChop(true, false, false);
            }
            else
            {
                Debug.Log("<b>CHOPPING:</b> faithful chop");
                UI.UpdateUIAfterChop(true, true, false);
            }
            
            gain.Play();
            chop.Play();
        }
        else
        {
            Debug.Log("<b>CHOPPING:</b> early chop");
            
            view.UpdateUI(-5);
            
            UI.UpdateUIAfterChop(false, true, false);
            loss.Play();
        }

        StartCoroutine(SlideHand(false, true));
        
        withdrawl.Play();
    }

    IEnumerator ChopAnimation()
    {
        while (cleaver.localPosition.y > 39)
        {
            cleaver.localPosition += (Vector3)(Vector2.down * cleaverSpeed);

            if (cleaver.localScale.x > 0.4f)
            {
                cleaver.localScale -= (Vector3)(Vector2.one * 0.1f);   
            }
            
            yield return new WaitForSeconds(0);
        }

        cleaver.localPosition = new Vector2(cleaver.localPosition.x, 39);
        cleaver.localScale = Vector2.one * 0.4f;
    }

    public IEnumerator Reset(float waitTime)
    {
        Debug.Log("<b>CHOPPING:</b> reset");
        
        UI.HideOutcomes();
        
        hand.transform.localPosition = Vector2.down * 200;

        cleaver.localPosition = new Vector2(100, 400);
        cleaver.localScale = Vector2.one * 2;
        
        yield return new WaitForSeconds(waitTime);
        
        hasChopped = false;
        
        Tremble temp = hand.GetComponent<Tremble>();

        if (view.Current().IsTraitor)
        {
            breathing.Play();
            coughing.makeSoundsRandomly = true;
            
            temp.xSpeed = 10;
            temp.xAmplitude = 1;
            temp.ySpeed = 15;
            temp.yAmplitude = 0.1f;
        }
        else
        {
            temp.xSpeed = 5;
            temp.xAmplitude = 0.5f;
            temp.ySpeed = 7.5f;
            temp.yAmplitude = 0.05f;
        }

        StartCoroutine(SlideHand(true));
        anticipation.Play();
    }
}