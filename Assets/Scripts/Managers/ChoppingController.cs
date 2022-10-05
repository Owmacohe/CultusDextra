using System;
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
    SoundEffectManager withdrawl, gain, loss, chop, place;

    [HideInInspector]
    public bool canChop;
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
            UI.UpdateUIAnticipation((Time.time - anticipationStartTime) * 2f);
            
            if (
                canChop
                && view != null && view.Current() != null && view.Current().IsTraitor
                && Random.Range(0f, 1f) <= traitorPullChance)
            {
                hasChopped = true;
                StartCoroutine(SlideHand(false, true));
                loss.Play();
                withdrawl.Play();
            
                UI.UpdateUIAfterChop(false, false, true);
            }   
        }
    }

    IEnumerator SlideHand(bool up, bool switchOnFinish = false)
    {
        isInPosition = false;
        
        Vector2 direction = Vector2.up;
        float speed = handSpeed;

        if (!up)
        {
            direction = Vector2.down;
            speed *= 15;
        }

        while ((up && hand.transform.localPosition.y < 0) || (!up && hand.transform.localPosition.y > -200))
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
            view.ToggleView();
        }

        if (!hasChopped)
        {
            place.Play();
        }
    }

    void Chop()
    {
        hasChopped = true;
        
        // TODO: chop animation

        if (isInPosition)
        {
            if (view.Current().IsTraitor)
            {
                UI.UpdateUIAfterChop(true, false, false);
            }
            else
            {
                UI.UpdateUIAfterChop(true, true, false);
            }
            
            gain.Play();
            chop.Play();
        }
        else
        {
            UI.UpdateUIAfterChop(false, true, false);
            loss.Play();
        }

        StartCoroutine(SlideHand(false, true));
        
        withdrawl.Play();
    }

    public IEnumerator Reset(float waitTime)
    {
        hand.transform.localPosition = Vector2.down * 200;
        
        yield return new WaitForSeconds(waitTime);
        
        hasChopped = false;

        StartCoroutine(SlideHand(true));
    }
}