//This file was created by Zakir Chaudry on June 14th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealthNumbers : HealthNumbers
{
    // Start is called before the first frame update
    public override void Start()
    {
        totalHealth = this.transform.parent.gameObject.GetComponent<MonsterHealthOptions>().startingHealth; //Gets starting health from HealthOptions
        currentHealth = totalHealth;
        UpdateHealth();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
