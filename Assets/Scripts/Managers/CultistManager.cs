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
    List<Cultist> finished;
    
    public Cultist current;

    ViewManager view;

    void Start()
    {
        procession = new Queue<Cultist>();
        finished = new List<Cultist>();

        view = FindObjectOfType<ViewManager>();
    }

    public void AddCultist()
    {
        float target = procession.Count * cultistSpacing;
        float startingDistance = startingOffset + target;
        
        // Create cultist prefab at end of procession
        GameObject temp = Instantiate(cultistPrefab, transform);
        temp.transform.localPosition = Vector2.left * startingDistance;

        bool isFaithful = Random.Range(0f, 1f) >= 0.3f;

        // Enqueue new Cultist
        procession.Enqueue(new Cultist(temp, isFaithful));
        
        // Move cultist up
        StartCoroutine(AdvanceCultist(temp, -target));
    }
    
    IEnumerator AdvanceCultist(GameObject obj, float target, bool slowOnApproach = false, bool switchOnFinish = false)
    {
        while (obj.transform.localPosition.x < target)
        {
            float speedOffset = (target - obj.transform.localPosition.x) / target;

            if (!slowOnApproach)
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

            obj.transform.localPosition += (Vector3)(Vector2.right * (cultistSpeed * speedOffset * 0.001f));
            
            yield return new WaitForSeconds(0);
        }
        
        steps.loopSounds = false;

        if (switchOnFinish)
        {
            view.ToggleView();
        }
    }

    void AdvanceProcession()
    {
        // Dequeue cultist
        current = procession.Dequeue();
        
        // Add a replacement cultist
        AddCultist();
        
        // Move front cultist to altar
        StartCoroutine(AdvanceCultist(
            current.Object, 
            current.Object.transform.localPosition.x + (cultistSpacing * 3),
            true,
            true
        ));

        // Move all cultists forward
        foreach (Cultist i in procession)
        {
            StartCoroutine(AdvanceCultist(
                i.Object,
                i.Object.transform.localPosition.x + cultistSpacing
            ));
        }
    }

    void FinishCurrent()
    {
        // TODO: cultist leaving animation
        
        Destroy(current.Object);
        
        finished.Add(current);
        current = null;
    }

    public IEnumerator Reset(float waitTime)
    {
        if (current != null)
        {
            FinishCurrent();   
        }
        
        yield return new WaitForSeconds(waitTime);
        
        AdvanceProcession();
    }

    public void NewDay()
    {
        day.Play();
    }
}