using System;
using System.Collections;
using UnityEngine;

public class ChoppingController : MonoBehaviour
{
    [SerializeField]
    GameObject hand;
    [SerializeField]
    float handSpeed = 0.15f;

    [SerializeField]
    SoundEffectManager withdrawl, gain, loss, chop, place;

    [HideInInspector]
    public bool canChop;
    bool isInPosition, hasChopped;
    
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
        if (!hasChopped && view != null && view.Current() != null && view.Current().IsTraitor)
        {
            // TODO: possible traitor pull away
        }
    }

    IEnumerator SlideHand(bool down, bool switchOnFinish = false)
    {
        isInPosition = false;
        
        Vector2 direction = Vector2.down;
        float speed = handSpeed;

        if (!down)
        {
            direction = Vector2.up;
            speed *= 10;
        }

        while ((down && hand.transform.localPosition.y > 0) || (!down && hand.transform.localPosition.y < 200))
        {
            hand.transform.localPosition += (Vector3)(direction * speed);
            yield return new WaitForSeconds(0);

            if (down && !isInPosition && hand.transform.localPosition.y < 50)
            {
                isInPosition = true;
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
        // TODO: chop animation

        if (isInPosition)
        {
            if (view.Current().IsTraitor)
            {
                // TODO: traitor chop
            }
            else
            {
                // TODO: faithful chop
            }
            
            gain.Play();
            chop.Play();
        }
        else
        {
            // TODO: pre-emptive chop
            loss.Play();
        }

        StartCoroutine(SlideHand(false, true));
        
        withdrawl.Play();
    }

    public IEnumerator Reset(float waitTime)
    {
        hand.transform.localPosition = Vector2.up * 200;
        
        yield return new WaitForSeconds(waitTime);
        
        hasChopped = false;

        StartCoroutine(SlideHand(true));
    }
}