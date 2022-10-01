using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    Queue<Cultist> procession;
    List<Cultist> finished;
    
    public Cultist current;

    ViewManager view;

    void Start()
    {
        procession = new Queue<Cultist>();
        finished = new List<Cultist>();

        view = GetComponent<ViewManager>();
    }

    public void AddCultist()
    {
        float target = procession.Count * cultistSpacing;
        float startingDistance = startingOffset + target;
        
        // Create cultist prefab at end of procession
        GameObject temp = Instantiate(cultistPrefab, transform);
        temp.transform.localPosition = Vector2.left * startingDistance;

        // Enqueue new Cultist
        procession.Enqueue(new Cultist(temp, false));
        
        // Move cultist up
        StartCoroutine(AdvanceCultist(temp, -target));
    }
    
    IEnumerator AdvanceCultist(GameObject obj, float target, bool switchOnFinish = false)
    {
        while (obj.transform.localPosition.x < target)
        {
            // TODO: slow speed as target is reached
            
            obj.transform.localPosition += (Vector3)(Vector2.right * (cultistSpeed * 0.001f));
            yield return new WaitForSeconds(0);
        }

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
            current.Object.transform.localPosition.x + (cultistSpacing * 5),
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
}