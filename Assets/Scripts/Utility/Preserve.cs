using System;
using UnityEngine;

public class Preserve : MonoBehaviour
{
    void Start()
    {
        Debug.Log("<b>PRESERVE:</b> " + gameObject.name);
        
        DontDestroyOnLoad(gameObject);
    }
}