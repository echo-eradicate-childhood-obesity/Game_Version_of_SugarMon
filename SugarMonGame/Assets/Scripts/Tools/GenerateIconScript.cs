//This file was created by Mark Botaish on June 12th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;

public class GenerateIconScript : MonoBehaviour
{
    static Transform _currentPanel;     //The current sugar group panel
    static Transform _panel;            //The SugarPanels object. Its the panel that contains all the other sugar group panels
    static List<Sprite> sprites;        //The list of sprite icons in the folder

    static List<string> iconLocation = new List<string>(new string[] {  //This a list of all of the sugar icon locations
        "Icons/Cane",
        "Icons/Concentrate",
        "Icons/Dextrin",
        "Icons/Obvious",
        "Icons/OSE",
        "Icons/Strange",
        "Icons/Syrup"
    });

    /// <summary>
    /// This function is used to generate all of the buttons in all of the sugar groups
    /// </summary>
    [MenuItem("Tools/Generate UI/All Buttons")]
    static void GenerateAll()
    {
        print("Generating...");
        if (!CheckScene())
            return;        

        int size = _panel.childCount - 1;
        for (int i = 0 ; i < size; i++) //Loop through the sugar groups
            Create(i, false);

        print("<color=green>Generation complete!</color>");

    }

    /// <summary>
    /// This function is used to clear all of the panel that at children of SugarPanels
    /// </summary>
    [MenuItem("Tools/Generate UI/Clear All Buttons")]
    static void ClearAll()
    {
        print("Clearing...");
        if (!CheckScene())
        {
            return;
        }

        int size = _panel.childCount - 1;
        for (int i = 0; i < size; i++)
        {
            if(_panel.GetChild(i).childCount > 0) //Are there buttons that already exists?
                ClearPanel(i);  
        }
        print("<color=green>Clearing all panels was successful!</color>");

    }

    /// <summary>
    /// This function is used to Create each button in a sugar group. First it clears the panel then 
    /// creates the respective buttons.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="shouldCheckScene"></param>
    static void Create(int index, bool shouldCheckScene = true)
    {
        if (shouldCheckScene && !CheckScene())
            return;

        if (_panel.GetChild(index).childCount > 0)
            ClearPanel(index);

        generate(index);
    }

    /// <summary>
    /// This function is used to clear all of the panels. The function itself does not clear
    /// all panels. This function does not require to check the scene first
    /// </summary>
    /// <param name="index"></param>
    static void ClearPanel(int index)
    {
        _currentPanel = _panel.GetChild(index);
        int size = _currentPanel.childCount;

        for (int i = size - 1; i >= 0; i--)
        {
            DestroyImmediate(_currentPanel.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// This function is used to clear an individual panel. It is different ClearPanel() as it checks the scene beforehand
    /// </summary>
    /// <param name="index"></param>
    static void ClearIndividualPanel(int index)
    {
        print("Clearing...");
        if (!CheckScene())
            return;

        ClearPanel(index);
        print("<color=green>Clearing the <" + _panel.GetChild(index).name + "> panel was successful!</color>");
    }

    /*
     * The following functions are used to gather all of the sugars in a particular group, then 
     * generate buttons based on those icons. 
    */
    #region INDIVIDUAL_GENERATION
    [MenuItem("Tools/Generate UI/Indv. Generation/Cane Buttons")]
    static void GenerateCane(){ Create(0); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Concentrate Buttons")]
    static void GenerateConcentrate(){ Create(1); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Dextrin Buttons")]
    static void GenerateDextrin(){ Create(2); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Obvious Buttons")]
    static void GenerateObvious(){ Create(3); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/OSE Buttons")]
    static void GenerateOSE(){ Create(4); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Strange Buttons")]
    static void GenerateStrange(){ Create(5); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Syrup Buttons")]
    static void GenerateSyrup(){ Create(6); print("<color=green>Done!</color>"); }
    #endregion

    /*
    * The following functions are used clear individual panels within the SugarPanels panel
   */
    #region CLEAR_INDIVIDUAL_PANELS
    [MenuItem("Tools/Generate UI/Indv. Clear/Cane Panel")]
    static void ClearCane() { ClearIndividualPanel(0); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Concentrate Buttons")]
    static void ClearConcentrate() { ClearIndividualPanel(1);}

    [MenuItem("Tools/Generate UI/Indv. Clear/Dextrin Buttons")]
    static void ClearDextrin() { ClearIndividualPanel(2); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Obvious Buttons")]
    static void ClearObvious() { ClearIndividualPanel(3); }

    [MenuItem("Tools/Generate UI/Indv. Clear/OSE Buttons")]
    static void ClearOSE() { ClearIndividualPanel(4); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Strange Buttons")]
    static void ClearStrange() { ClearIndividualPanel(5); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Syrup Buttons")]
    static void ClearSyrup() { ClearIndividualPanel(6);}
    #endregion

    /// <summary>
    /// This function takes each of the sprite in the given list and creates a button with that icon and that name. 
    /// </summary>
    static void generate(int index)
    {
        sprites = Resources.LoadAll(iconLocation[index], typeof(Sprite)).Cast<Sprite>().ToList(); //Load all the sprites from the folder

        GameObject _buttonPrefab = Resources.Load("UI/Button") as GameObject; //Get the prefab
        _currentPanel = _panel.GetChild(index); //Set the current panel to the correct sugar panel


        //Loop through each of the sprites from the folder and make button out of them 
        foreach (Sprite sprite in sprites)
        {
            GameObject button = Instantiate(_buttonPrefab, _currentPanel);
            button.GetComponent<Image>().sprite = sprite;
            button.name = Regex.Replace(sprite.name, @"(^\w)|(\s\w)", m => m.Value.ToUpper()) + " Button"; // Each words starts with an uppercase
        }
        _currentPanel.GetChild(0).GetComponent<Button>().interactable = true; //Set the first button to be interactable

    }
    
    /// <summary>
    /// Checks to see if the SugarPanels object exists and has at least 7 children.
    /// </summary>
    static bool CheckScene()
    {
        GameObject obj = GameObject.Find("SugarPanels");
        if (obj == null) //Check to see if the object exists
        {
            Debug.LogError("Could not find the SugarPanels object");
            return false;
        }
        if (obj.transform.childCount < 7) //Check to see if all of the sugar group panels exist
        {
            Debug.LogError("SugarPanels does not have at least 7 children attached!");
            return false;
        }

        print("<color=green>Scene has passed the checks!</color>");
        _panel = obj.transform;
       
       return true;
    }

    private void Start()
    {
        Debug.LogError("This script should not be attached to any object! Current Object =  <" + gameObject.name + ">" );
    }
}
