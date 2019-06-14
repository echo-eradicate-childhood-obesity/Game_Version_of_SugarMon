﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// The background for the bar based health system
/// </summary>
public class HealthBar : MonoBehaviour
{
    /// <summary>
    /// Position of health bar
    /// </summary>
    [Tooltip("Position of health bar")]
    public Vector2 position = new Vector2(20, 40);
    /// <summary>
    /// Size of health bar
    /// </summary>
    [Tooltip("Size of health bar")]
    public Vector2 size = new Vector2(60, 20);
    /// <summary>
    /// Percent of total health player has
    /// </summary>
    [Range(0,1), Tooltip("Percent of total health player has")]
    public float healthPercent;
    /// <summary>
    /// Material for background of health bar
    /// </summary>
    [Tooltip("Material for background of health bar")]
    public Material emptyBar;
    /// <summary>
    /// Material for filled part of health bar
    /// </summary>
    [Tooltip("Material for filled part of health bar")]
    public Material filledBar;
    ///// <summary>
    ///// How long it takes for health bar to update
    ///// </summary>
    //[Tooltip("How long it takes for health bar to update")]
    //public float timeToChangeHealth = 1f;



    // Start is called before the first frame update
    void Start()
    {
        healthPercent = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(position, size));
            GUI.Box(new Rect(new Vector2(0, 0), size), emptyBar.mainTexture);

            GUI.BeginGroup(new Rect(0, 0, size.x * healthPercent, size.y));
                GUI.Box(new Rect(new Vector2(0, 0), size), filledBar.mainTexture);
            GUI.EndGroup();

        GUI.EndGroup();

    }
    /// <summary>
    /// Updates health for health bar based on current player health percentage
    /// </summary>
    public void UpdateHealth(float newHealthPercent)
    {
        healthPercent = newHealthPercent;
    }
}
