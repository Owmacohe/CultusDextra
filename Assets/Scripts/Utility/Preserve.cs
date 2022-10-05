using System;
using UnityEngine;

public class Preserve : MonoBehaviour
{
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}