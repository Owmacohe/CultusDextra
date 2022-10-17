using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CultistManager : MonoBehaviour
{
    [SerializeField]
    GameObject cultistPrefab;
    [SerializeField]
    float cultistSpacing = 2;
    [SerializeField]
    float cultistSpeed = 5;
    [SerializeField]
    float startingOffset = 12;
    
    [SerializeField]
    SoundEffectManager steps, day;

    Queue<Cultist> procession;
    
    public Cultist current;

    ViewManager view;

    void Start()
    {
        procession = new Queue<Cultist>();

        view = FindObjectOfType<ViewManager>();

        day.Play();
    }

    public void AddCultist()
    {
        Debug.Log("<b>CULTISTS:</b> add");
        
        float target = procession.Count * cultistSpacing + (cultistSpacing * 1);
        float startingDistance = startingOffset + target;
        
        GameObject temp = Instantiate(cultistPrefab, transform);
        temp.transform.localPosition = Vector2.left * startingDistance;

        bool isTraitor = Random.Range(0f, 1f) <= 0.3f;

        if (isTraitor)
        {
            temp.GetComponent<Tremble>().enabled = true;
        }
        
        Cultist tempCultist = new Cultist(temp, isTraitor);
        
        procession.Enqueue(tempCultist);
        
        StartCoroutine(AdvanceCultist(tempCultist, -target));
    }
    
    IEnumerator AdvanceCultist(Cultist cultist, float target, bool frontCultist = false)
    {
        cultist.Animator.SetBool("isWalking", true);
        cultist.Animator.speed = Random.Range(-0.05f, 0.05f) + 0.6f;
        cultist.Animator.Play("walk", 0, Random.Range(0, 8));
        
        Transform cultistTransform = cultist.Object.transform;
        
        while (cultistTransform.localPosition.x < target)
        {
            float speedOffset = (target - cultistTransform.localPosition.x) / target;

            if (!frontCultist)
            {
                speedOffset = 1;
            }
            else
            {
                float minOffset = 0.5f;

                if (speedOffset < minOffset)
                {
                    speedOffset = minOffset;
                }   
            }

            if (!view.isChopping)
            {
                steps.loopSounds = true;   
            }
            else
            {
                steps.loopSounds = false;
            }

            cultistTransform.localPosition += (Vector3)(Vector2.right * (cultistSpeed * speedOffset * 0.001f));

            if (frontCultist)
            {
                cultistTransform.localPosition += (Vector3) (Vector2.up * (cultistSpeed * speedOffset * 0.0002f));
            }
            
            yield return new WaitForSeconds(0);
        }
        
        steps.loopSounds = false;
        
        cultist.Animator.SetBool("isWalking", false);

        if (frontCultist)
        {
            view.ToggleView();
        }
    }

    void AdvanceProcession()
    {
        Debug.Log("<b>CULTISTS:</b> advance");

        current = procession.Dequeue();
        
        StartCoroutine(AdvanceCultist(
            current,
            current.Object.transform.localPosition.x + (cultistSpacing * 2),
            true
        ));

        foreach (Cultist i in procession)
        {
            StartCoroutine(AdvanceCultist(i, i.Object.transform.localPosition.x + cultistSpacing));
        }
    }

    void FinishCurrent()
    {
        Debug.Log("<b>CULTISTS:</b> finish current");
        
        // TODO: cultist leaving animation?
        
        Destroy(current.Object);
        
        if (procession.Count == 0)
        {
            SceneChange.StaticChange("Day End");
        }
    }

    public IEnumerator Reset(float waitTime)
    {
        Debug.Log("<b>CULTISTS:</b> reset");
        
        if (current != null)
        {
            FinishCurrent();   
        }
        
        yield return new WaitForSeconds(waitTime);
        
        AdvanceProcession();
    }
}