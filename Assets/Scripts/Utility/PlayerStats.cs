using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Stats stats;

    void Start()
    {
        stats = new Stats();
    }
}