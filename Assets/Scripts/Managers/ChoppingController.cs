using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
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
    GameObject blood, finger;
    [SerializeField]
    Sprite handDefault, handChopped;

    [SerializeField]
    SoundEffectManager withdrawl, gain, loss, chop, anticipation, breathing, coughing;

    [HideInInspector] public bool canChop;
    bool isInPosition, hasChopped;
    float anticipationStartTime;

    ViewManager view;

    bool isSliding, isSlidingUp;
    Vector2 slidingDirection;
    float slidingSpeed;

    bool isChopping;

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
        if (isSliding)
        {
            if ((isSlidingUp && !hasChopped && hand.transform.localPosition.y < 0)
                   || (!isSlidingUp && hand.transform.localPosition.y > -350))
            {
                hand.transform.localPosition += (Vector3)(slidingDirection * slidingSpeed);

                if (isSlidingUp && !isInPosition && hand.transform.localPosition.y > -50)
                {
                    isInPosition = true;
                    anticipationStartTime = Time.time;
                }
            }
            else
            {
                isSliding = false;
                isSlidingUp = false;
                slidingDirection = Vector2.zero;
                slidingSpeed = 0;
            }
        }

        if (isChopping)
        {
            if (cleaver.localPosition.y > 222)
            {
                cleaver.localPosition += (Vector3)(Vector2.down * cleaverSpeed);

                if (cleaver.localScale.x > 0.4f)
                {
                    cleaver.localScale -= (Vector3)(Vector2.one * 0.1f);   
                }
            }
            else
            {
                cleaver.localPosition = new Vector2(cleaver.localPosition.x, 222);
                cleaver.localScale = Vector2.one * 0.4f;

                isChopping = false;
            }
        }
        
        if (!hasChopped && isInPosition)
        {
            float temp = (Time.time - anticipationStartTime) * 2f;
            
            UI.UpdateUIAnticipation(temp);

            if (Time.time % 1 == 0 && temp < 10)
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
                
                yield return new WaitForSeconds(0.2f);
            }

            isSliding = true;
            isSlidingUp = up;
            slidingDirection = direction;
            slidingSpeed = speed;

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
        isChopping = true;
        
        breathing.Stop();
        coughing.makeSoundsRandomly = false;

        if (isInPosition)
        {
            view.UpdateUI(2);
            
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
            
            blood.SetActive(true);
            finger.SetActive(true);
            hand.GetComponentInChildren<Image>().sprite = handChopped;
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

    public IEnumerator Reset(float waitTime)
    {
        Debug.Log("<b>CHOPPING:</b> reset");
        
        UI.HideOutcomes();
        UI.UpdateUIAnticipation(0);
        
        blood.SetActive(false);
        finger.SetActive(false);
        hand.GetComponentInChildren<Image>().sprite = handDefault;
        
        hand.transform.localPosition = Vector2.down * 200;

        cleaver.localPosition = new Vector2(100, 800);
        cleaver.localScale = Vector2.one;
        
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