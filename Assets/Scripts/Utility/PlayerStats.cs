using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stats stats;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        stats = new Stats(1);
    }
}