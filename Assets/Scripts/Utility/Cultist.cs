﻿using UnityEngine;

public class Cultist
{
    public GameObject Object { get; }
    public Animator Animator { get; }
    public bool IsTraitor { get; }
    public bool IsChopped { get; set; }

    public Cultist(GameObject obj, bool traitor, bool isChopped = false)
    {
        Object = obj;
        Animator = Object.GetComponent<Animator>();
        IsTraitor = traitor;
        IsChopped = isChopped;
    }
}