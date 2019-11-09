﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyAdder : MonoBehaviour
{
    public float energyBonus;
    [HideInInspector]
    public bool booster = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            booster = true;
    }
}