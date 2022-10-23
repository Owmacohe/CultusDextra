using System;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField]
    float speed = 2f;
    [SerializeField]
    float offset = 0.3f;
    [SerializeField]
    float amplitude = 0.2f;
    
    Image img;
    SpriteRenderer rend;

    void Start()
    {
        img = GetComponent<Image>();

        if (img == null)
        {
            rend = GetComponent<SpriteRenderer>();
        }
    }

    void FixedUpdate()
    {
        Color col;
        float opacity = Mathf.Sin(Time.time * speed) * amplitude;

        if (img != null)
        {
            col = img.color;
            img.color = new Color(col.r, col.g, col.b, opacity + offset);
        }
        else
        {
            col = rend.color;
            rend.color = new Color(col.r, col.g, col.b, opacity + offset);
        }
    }
}