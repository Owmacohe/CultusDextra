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
    SoundEffectManager steps;

    Queue<Cultist> procession;
    
    public Cultist current;

    ViewManager view;
    PlayerStats playerStats;

    List<Cultist> advancingCultists;
    List<float> advancingCultistTargets;
    Cultist frontCultist;
    float frontCultistTarget;

    void Start()
    {
        procession = new Queue<Cultist>();

        view = FindObjectOfType<ViewManager>();
        playerStats = FindObjectOfType<PlayerStats>();

        advancingCultists = new List<Cultist>();
        advancingCultistTargets = new List<float>();
    }

    void FixedUpdate()
    {
        if (advancingCultists.Count > 0)
        {
            for (int i = 0; i < advancingCultists.Count; i++)
            {
                Transform cultistTransform = advancingCultists[i].Object.transform;
        
                if (cultistTransform.localPosition.x < advancingCultistTargets[i])
                {
                    float speedOffset = (advancingCultistTargets[i] - cultistTransform.localPosition.x) / advancingCultistTargets[i];

                    float minOffset = 0.5f;

                    if (speedOffset < minOffset)
                    {
                        speedOffset = minOffset;
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
                }
                else
                {
                    steps.loopSounds = false;
        
                    advancingCultists[i].Animator.SetBool("isWalking", false);

                    advancingCultists.RemoveAt(i);
                    advancingCultistTargets.RemoveAt(i);
                }
            }
        }

        if (frontCultist != null)
        {
            Transform cultistTransform = frontCultist.Object.transform;
        
            if (cultistTransform.localPosition.x < frontCultistTarget)
            {
                float speedOffset = (frontCultistTarget - cultistTransform.localPosition.x) / frontCultistTarget;

                speedOffset = 1;

                if (!view.isChopping)
                {
                    steps.loopSounds = true;   
                }
                else
                {
                    steps.loopSounds = false;
                }

                cultistTransform.localPosition += (Vector3)(Vector2.right * (cultistSpeed * speedOffset * 0.001f));
                cultistTransform.localPosition += (Vector3)(Vector2.up * (cultistSpeed * speedOffset * 0.0002f));
            }
            else
            {
                steps.loopSounds = false;
        
                frontCultist.Animator.SetBool("isWalking", false);

                view.ToggleView();

                frontCultist = null;
                frontCultistTarget = 0;
            }
        }
    }

    public void AddCultist()
    {
        float target = procession.Count * cultistSpacing + (cultistSpacing * 1);
        float startingDistance = startingOffset + target;
        
        GameObject temp = Instantiate(cultistPrefab, transform);
        temp.transform.localPosition = Vector2.left * startingDistance;

        float traitorThreshold = 0.2f + (playerStats.stats.Day * 0.1f);

        if (traitorThreshold > 0.8f)
        {
            traitorThreshold = 0.8f;
        }
        
        bool isTraitor = playerStats.stats.Day > 1 && (Random.Range(0f, 1f) <= traitorThreshold);

        if (isTraitor)
        {
            temp.GetComponent<Tremble>().enabled = true;
        }
        
        Cultist tempCultist = new Cultist(temp, isTraitor);
        
        procession.Enqueue(tempCultist);
        
        AdvanceCultist(tempCultist, -target);
    }
    
    void AdvanceCultist(Cultist cultist, float target, bool isFrontCultist = false)
    {
        cultist.Animator.SetBool("isWalking", true);
        cultist.Animator.speed = Random.Range(-0.05f, 0.05f) + 0.6f;
        cultist.Animator.Play("walk", 0, Random.Range(0, 8));

        if (isFrontCultist)
        {
            frontCultist = cultist;
            frontCultistTarget = target;
        }
        else
        {
            advancingCultists.Add(cultist);
            advancingCultistTargets.Add(target);   
        }
    }

    void AdvanceProcession()
    {
        Debug.Log("<b>CULTISTS:</b> advance");

        current = procession.Dequeue();
        
        AdvanceCultist(
            current,
            current.Object.transform.localPosition.x + (cultistSpacing * 2),
            true
        );

        foreach (Cultist i in procession)
        {
            AdvanceCultist(i, i.Object.transform.localPosition.x + cultistSpacing);
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