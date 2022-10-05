using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tremble : MonoBehaviour
{
    [SerializeField]
    float xSpeed = 10;
    [SerializeField]
    float xAmplitude = 1;
    [SerializeField]
    float ySpeed = 15;
    [SerializeField]
    float yAmplitude = 0.1f;
    [SerializeField, Range(0, 1)]
    float randomRange = 0.5f;

    void FixedUpdate()
    {
        transform.localPosition = new Vector2(
            transform.localPosition.x + (Mathf.Sin(Time.time * xSpeed) * xAmplitude * Random.Range(1f - randomRange, 1f + randomRange)),
            transform.localPosition.y + (Mathf.Sin(Time.time * ySpeed) * yAmplitude * Random.Range(1f - randomRange, 1f + randomRange))
        );
    }
}