using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Options : MonoBehaviour
{
    public int startingHealth = 100;
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

    void TakeDamage(int damage)
    {
        if (health - damage < 0)
        {
            health = 0;
        } else
        {
            health -= damage;
        }
    }

    [ContextMenu("Take 1 Damage")]
    void TakeOneDamage()
    {
        TakeDamage(1);
    }

    [ContextMenu("Take 10 Damage")]
    void TakeTenDamage()
    {
        TakeDamage(10);
    }

    [ContextMenu("Take 30 Damage")]
    void TakeThirtyDamage()
    {
        TakeDamage(30);
    }

    [ContextMenu("Restore Full Health")]
    void RestoreFullHealth()
    {
        health = startingHealth;
    }
}
