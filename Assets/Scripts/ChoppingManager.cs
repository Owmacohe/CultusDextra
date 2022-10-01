using System;
using System.Collections;
using UnityEngine;

public class ChoppingManager : MonoBehaviour
{
    [SerializeField]
    GameObject hand;
    [SerializeField]
    float handSpeed = 0.15f;

    bool isDown, hasChopped;
    
    ViewManager view;

    void Start()
    {
        view = GetComponent<ViewManager>();
    }

    void Update()
    {
        if (!hasChopped && Input.GetMouseButtonDown(0))
        {
            Chop();
        }

        if (view.Current().IsTraitor)
        {
            // TODO: possible traitor pull away
        }
    }

    IEnumerator SlideHand(bool down, bool switchOnFinish = false)
    {
        isDown = false;
        
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
        }

        if (down)
        {
            isDown = true;
        }

        if (switchOnFinish)
        {
            view.ToggleView();
        }
    }

    void Chop()
    {
        // TODO: chop animation

        if (isDown)
        {
            if (view.Current().IsTraitor)
            {
                // TODO: traitor chop
            }
            else
            {
                // TODO: faithful chop
            }
        }
        else
        {
            // TODO: pre-emptive chop
        }

        StartCoroutine(SlideHand(false, true));
    }

    public IEnumerator Reset(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        hasChopped = false;

        StartCoroutine(SlideHand(true));
    }
}