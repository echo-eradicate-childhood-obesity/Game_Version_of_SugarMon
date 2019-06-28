using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoScript : MonoBehaviour
{
    public static PlayerInfoScript instance;

    #region PRIVATE_VARS

    private int _coins = 0;
    private int _level = 0;
    private int _xp = 0;
    private int _xpForNextLevel = 100;
    private float _nextLevelXPMultiplier = 1.3f;

    //Damage
    private float _damage = 20.0f;
    private float _damageIncreasePerLevel = 20.0f;
    private float _startDamage = 20.0f;
    private int _damageLevel = 0;

    //Armour
    private float _armour = 0f;
    private float _armourIncreasePerLevel = 50.0f;
    private float _startArmour = 0.0f;
    private int _armourLevel = 0;

    //Health 
    private float _health = 100f;
    private float _healthIncreasePerLevel = 50;
    private float _startHealth = 100.0f;
    private int _healthLevel = 0;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetLevel(int level) { _level = level; _xpForNextLevel = (int)(_xpForNextLevel * Mathf.Pow(_nextLevelXPMultiplier,_level)); }
    public void SetXP(int xp) { _xp = xp; }
    public void SetCoins(int coins) { _coins = coins; }
    public void SetDamage(float damage) { _damage = damage; }
    public void SetHealth(float health) { _health = health; }
    public void SetArmour(float armour) { _armour = armour; }
    public void SetHealthLevel(int level) { _healthLevel = level; _health = ((_healthIncreasePerLevel * _healthLevel) + _startHealth); }
    public void SetDamageLevel(int level) { _damageLevel = level; _damage = ((_damageIncreasePerLevel * _damageLevel) + _startDamage); }
    public void SetArmourLevel(int level) { _armourLevel = level; _armour = ((_armourIncreasePerLevel * _armourLevel) + _startArmour); }

    public void AddCoins(int coins) { _coins += coins; }
    public void AddXp(int xp)
    {
        _xp += xp;
        if(xp >= _xpForNextLevel)
        {
            _level++;
            _xpForNextLevel = (int)(_xpForNextLevel * _nextLevelXPMultiplier);
        }
    }
    public void AddHealth(float health) { _health += health; }
    public void AddDamage(float damage) { _damage += damage; }
    public void AddArmour(float armour) { _armour += armour; }
    public void AddHealthLevel(int level = 1) { _healthLevel++; _health += _healthIncreasePerLevel; }
    public void AddDamageLevel(int level = 1) { _damageLevel++; _damage += _damageIncreasePerLevel; }
    public void AddArmourLevel(int level = 1) { _armourLevel++; _armour += _armourIncreasePerLevel; }

    public int GetHealthLevel() { return _healthLevel; }
    public int GetDamageLevel() { return _damageLevel; }
    public int GetArmourLevel() { return _armourLevel; }
    public float GetHealth() { return _health; }
    public float GetDamage() { return _damage; }
    public float GetArmour() { return _armour; }
    
}
