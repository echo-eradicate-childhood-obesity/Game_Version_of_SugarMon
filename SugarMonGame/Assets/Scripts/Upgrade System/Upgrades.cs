//This file was created by Zakir Chaudry on June 17th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides structural background to upgrade system
/// </summary>
public class Upgrades : MonoBehaviour
{
    /// <summary>
    /// The different upgrade levels each item can achieve
    /// </summary>
    [Tooltip("The different upgrade levels an item can achieve")]
    public readonly string[] levels = { "Basic", "Bronze", "Silver", "Gold", "Platinum" };

    /// <summary>
    /// The different items available to upgrade
    /// </summary>
    [Tooltip("The different items available for upgrade")]
    public readonly string[] items = { "Gun", "Health", "Gold" };

    /// <summary>
    /// The different costs for each level
    /// </summary>
    readonly int[] costs = { 50, 100, 150, 200 };

    /// <summary>
    /// The filenames/locations for each item image
    /// </summary>
    readonly string[] fileLocations = { "Bazooka", "Heart", "Gold" };

    /// <summary>
    /// The descriptions for each item
    /// </summary>
    readonly string[] itemDescriptions = { "Increase how much damage you do", "Increase your total health", "Increase the amount of gold you get per monster" };

    /// <summary>
    /// Dictionary of Levels to Costs 
    /// </summary>
    [Tooltip("Dictionary of levels to costs")]
    public Dictionary<string, int> levelToCost = new Dictionary<string, int>();

    /// <summary>
    /// Dictionary of Items to Item Descriptions
    /// </summary>
    [Tooltip("Dictionary of items to item descriptions")]
    public Dictionary<string, string> itemToDescription = new Dictionary<string, string>();
    
    /// <summary>
    /// Dictionary of Items to File Locations
    /// </summary>
    [Tooltip("Dictionary of items to their file locations")]
    public Dictionary<string, string> itemToFile =  new Dictionary<string, string>();

    /// <summary>
    /// Dictionary of upgrade levels to their respective index in levels
    /// </summary>
    [Tooltip("Dictionary of upgrade levels to their respective index in levels")]
    public Dictionary<string, int> levelToIndex = new Dictionary<string, int>();

    /// <summary>
    /// Current Selected Item
    /// </summary>
    [Tooltip("Curently selected item")]
    public string curr;

    /// <summary>
    /// Index of current item in items
    /// </summary>
    int currIndex;

    // Start is called before the first frame update
    void Awake()
    {
        levelToIndex.Add(levels[0], 0);
        for (int i = 1; i < levels.Length; i++) {
            levelToCost.Add(levels[i], costs[i-1]);
            levelToIndex.Add(levels[i], i);
        }
        for (int i = 0; i < items.Length; i++)
        {
            itemToDescription.Add(items[i], itemDescriptions[i]);
            itemToFile.Add(items[i], fileLocations[i]);
        }

        currIndex = 0;
        curr = items[currIndex];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Updates anything that is affected by the upgrades backend
    /// </summary>
    void UpdateUpgrades()
    {
        this.transform.GetChild(0).gameObject.GetComponent<LevelUpgradeScreen>().UpdateScreen();
    }

    /// <summary>
    /// Changes current item to next item, and updates previous and next item accordingly
    /// </summary>
    public void GoToNext()
    {
        currIndex = (currIndex + 1) % items.Length;
        curr = items[currIndex];
        UpdateUpgrades();
    }

    /// <summary>
    /// Changes current item to previous item, and updates previous and next item accordingly
    /// </summary>
    public void GoToPrev()
    {
        currIndex = (currIndex - 1) % items.Length;
        curr = items[currIndex];
        UpdateUpgrades();
    }
}
