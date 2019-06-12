﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Provides structural background for all health related mechanics.
/// </summary>
public class HealthOptions : MonoBehaviour
{
    /// <summary>
    /// How much health the player starts with
    /// </summary>
    [Tooltip("How much health the player starts with")]
    public int startingHealth = 100;
    /// <summary>
    /// How much health the player currently has
    /// </summary>
    [Range(0,100), Tooltip("How much health the player currently has")]
    public int health;

    // Start is called before the first frame update
    void Start()
    {
        health = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Player takes the specified amount of damage.
    /// </summary>
    /// <param name="damage">How much damage the player takes.</param>
    void TakeDamage(int damage)
    {
        if (health - damage < 0)
        {
            health = 0;
        } else
        {
            health -= damage;
        }
        UpdateHealth();
    }
    /// <summary>
    /// Player takes 1 damage.
    /// </summary>
    [ContextMenu("Take 1 Damage")]
    void TakeOneDamage()
    {
        TakeDamage(1);
    }
    /// <summary>
    /// Player takes 10 damage.
    /// </summary>
    [ContextMenu("Take 10 Damage")]
    void TakeTenDamage()
    {
        TakeDamage(10);
    }
    /// <summary>
    /// Player takes 30 damage.
    /// </summary>
    [ContextMenu("Take 30 Damage")]
    void TakeThirtyDamage()
    {
        TakeDamage(30);
    }
    /// <summary>
    /// Fully restores the player's health to the starting health amount.
    /// </summary>
    [ContextMenu("Restore Full Health")]
    void RestoreFullHealth()
    {
        health = startingHealth;
    }
    /// <summary>
    /// Updates anything that depends on the player health value
    /// </summary>
    [ContextMenu("Update Health Values")]
    void UpdateHealth()
    {
        float percentageHealth = (float)health / startingHealth;
        Debug.Log("Percentage Health is " + percentageHealth);
        this.transform.GetChild(0).gameObject.GetComponent<Hearts>().UpdateHealth(percentageHealth);
    }
}
