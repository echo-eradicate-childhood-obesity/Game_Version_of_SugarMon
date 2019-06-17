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

        for (int i = 0 ; i < iconLocation.Count; i++) //Loop through the sugar groups
            CreateButtons(i);

        print("<color=green>Generation complete!</color>");

    }

    /// <summary>
    /// This function is used to clear all of the panel. Panels can be found under the SugarPanels GameObject
    /// </summary>
    [MenuItem("Tools/Generate UI/Clear All Buttons")]
    static void ClearAll()
    {
        print("Clearing...");

        List<GameObject> list = GameObject.FindGameObjectsWithTag("LevelButton").ToList<GameObject>();

        int size = list.Count;
        for (int i = 0; i < size; i++)
        {
            DestroyImmediate(list[i]);  
        }
        print("<color=green>Clearing all panels was successful!</color>");

    }

    /// <summary>
    /// This function is used to clear a panel given a specific sugar group index.
    /// <param name="index"></param>
    static void ClearPanel(int index)
    {
        print("Clearing...");
        string name = iconLocation[index].Substring(6) + "Panel";

        print(name);
        _currentPanel = GameObject.Find(name).transform.GetChild(1).GetChild(0);
        int size = _currentPanel.childCount;

        for (int i = size - 1; i >= 0; i--)
        {
            DestroyImmediate(_currentPanel.GetChild(i).gameObject);
        }

        print("<color=green>Clearing of the <" + name + "> panel was successful!</color>");
    }

    /// <summary>
    /// The function is used to clear a certain panel. The panel must be given in the parameters
    /// </summary>
    /// <param name="index"></param>
    static void ClearPanel(Transform panel)
    {
        _currentPanel = panel;
        int size = _currentPanel.childCount;

        for (int i = size - 1; i >= 0; i--)
        {
            DestroyImmediate(_currentPanel.GetChild(i).gameObject);
        }
    }

    /*
     * The following functions are used to gather all of the sugars in a particular group, then 
     * generate buttons based on those icons. 
    */
    #region INDIVIDUAL_GENERATION
    [MenuItem("Tools/Generate UI/Indv. Generation/Cane Buttons")]
    static void GenerateCane(){ CreateButtons(0); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Concentrate Buttons")]
    static void GenerateConcentrate(){ CreateButtons(1); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Dextrin Buttons")]
    static void GenerateDextrin(){ CreateButtons(2); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Obvious Buttons")]
    static void GenerateObvious(){ CreateButtons(3); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/OSE Buttons")]
    static void GenerateOSE(){ CreateButtons(4); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Strange Buttons")]
    static void GenerateStrange(){ CreateButtons(5); print("<color=green>Done!</color>"); }

    [MenuItem("Tools/Generate UI/Indv. Generation/Syrup Buttons")]
    static void GenerateSyrup(){ CreateButtons(6); print("<color=green>Done!</color>"); }
    #endregion

    /*
    * The following functions are used clear individual panels within the SugarPanels panel
   */
    #region CLEAR_INDIVIDUAL_PANELS
    [MenuItem("Tools/Generate UI/Indv. Clear/Cane Panel")]
    static void ClearCane() { ClearPanel(0); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Concentrate Buttons")]
    static void ClearConcentrate() { ClearPanel(1);}

    [MenuItem("Tools/Generate UI/Indv. Clear/Dextrin Buttons")]
    static void ClearDextrin() { ClearPanel(2); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Obvious Buttons")]
    static void ClearObvious() { ClearPanel(3); }

    [MenuItem("Tools/Generate UI/Indv. Clear/OSE Buttons")]
    static void ClearOSE() { ClearPanel(4); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Strange Buttons")]
    static void ClearStrange() { ClearPanel(5); }

    [MenuItem("Tools/Generate UI/Indv. Clear/Syrup Buttons")]
    static void ClearSyrup() { ClearPanel(6);}
    #endregion

    /// <summary>
    /// This function takes each of the sprite in the given list and creates a button with that icon and that name. 
    /// </summary>
    static void generate(int index, Transform panel = null)
    {
        sprites = Resources.LoadAll(iconLocation[index], typeof(Sprite)).Cast<Sprite>().ToList(); //Load all the sprites from the folder

        GameObject _buttonPrefab = Resources.Load("UI/Button") as GameObject; //Get the prefab
        //_currentPanel = _panel.GetChild(index); //Set the current panel to the correct sugar panel
        _currentPanel = panel;

        //Loop through each of the sprites from the folder and make button out of them 
        foreach (Sprite sprite in sprites)
        {
            GameObject button = Instantiate(_buttonPrefab, _currentPanel);
            button.GetComponent<Image>().sprite = sprite;
            button.name = Regex.Replace(sprite.name, @"(^\w)|(\s\w)", m => m.Value.ToUpper()) + " Button"; // Each words starts with an uppercase
            button.tag = "LevelButton";
        }
        _currentPanel.GetChild(0).GetComponent<Button>().interactable = true; //Set the first button to be interactable

    }

    /// <summary>
    /// This function is used to create the panels and buttons of sugar groups. 
    /// This function will create GameObjects, Panels, Text, etc. to generate the 
    /// proper heirarchy.
    /// </summary>
    /// <param name="index"></param>
    static void CreateButtons(int index) {
        string name = iconLocation[index].Substring(6) + "Panel"; //Remove the "Icon/" from the name and add "Panel" to the end

        GameObject sugarPanels = GameObject.Find("SugarPanels");

        GameObject obj = GameObject.Find(name);
        GameObject panel = null;

        sprites = Resources.LoadAll(iconLocation[index], typeof(Sprite)).Cast<Sprite>().ToList(); //Load all the sprites from the folder

        if (obj == null) //If the correct sugar panel doesn't exist, then make one
        {
            panel = CreateGroup(name);
            obj = panel.transform.parent.parent.gameObject;
        }
        else // If it does exist, then clear all of the buttons in that panel 
        {
            panel = obj.transform.GetChild(1).GetChild(0).gameObject;
            ClearPanel(panel.transform);
        }

        //Generate the buttons
        generate(index, panel.transform);

        //If the SugarPanel does not exist, then make one
        if (sugarPanels == null)
        { 
            sugarPanels = new GameObject("SugarPanels");
            sugarPanels.transform.SetParent(GameObject.Find("Canvas").transform, false);
        }

        //Set the parent to the SugarPanel
        obj.transform.SetParent(sugarPanels.transform, false);
        
    }

    /// <summary>
    /// This function is used to create the panel for each sugar group. The panel 
    /// will automatically resize depending on the number of added sugars icons.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static GameObject CreateGroup(string name)
    {
        // Set the inital size of the panel. Probably will need to change
        GameObject obj = CreatePanel(name, new Vector2(600, 500), GameObject.Find("Canvas").transform); 

        obj.GetComponent<Image>().color = Color.black;
        CreateText(name + " - 0/0", obj.transform); //Create the header for each of the buttons

        //Create the scroll size and the button panel 
        Vector3 size = new Vector3(600, Mathf.CeilToInt(Mathf.Max( sprites.Count,26) / 6.0f) * 100);
        GameObject scroll = CreateScroller(obj.transform, new Vector2(600, 450));
        GameObject buttonPanel = CreatePanel("Btns - " + name, size, scroll.transform, true);

        //Set the scroll contents and position
        buttonPanel.GetComponent<RectTransform>().position -= new Vector3(0, size.y / 6);
        scroll.GetComponent<ScrollRect>().content = buttonPanel.GetComponent<RectTransform>();
        return buttonPanel;
    }

    /// <summary>
    /// This function is used to create the scroller GameObject
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    static GameObject CreateScroller(Transform transform, Vector3 size)
    {
        //create the gameobject
        GameObject scroll = new GameObject("Scroll");
        scroll.AddComponent<CanvasRenderer>();
        scroll.AddComponent<RectTransform>();
        scroll.AddComponent<Image>();
        scroll.AddComponent<Mask>();

        //Init the size and position. Might need to change in the future 
        scroll.GetComponent<RectTransform>().sizeDelta = size;
        scroll.GetComponent<RectTransform>().position -= new Vector3(0, 25, 0);
        
        //Apply scroller settings
        ScrollRect rect = scroll.AddComponent<ScrollRect>();
        rect.movementType = ScrollRect.MovementType.Clamped;
        rect.scrollSensitivity = 30.0f;

        //Set the parent
        scroll.transform.SetParent(transform, false);
        return scroll;
    }

    /// <summary>
    /// This function is used to create a panel for the sugar buttons to be attached to.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="size"></param>
    /// <param name="transform"></param>
    /// <param name="isGrid"></param>
    /// <returns></returns>
    static GameObject CreatePanel(string name, Vector3 size, Transform transform, bool isGrid = false)
    {
        //Create the panel
        GameObject panel = new GameObject(name);
        panel.AddComponent<CanvasRenderer>();
        panel.AddComponent<RectTransform>();
        panel.AddComponent<Image>();

        //Set the size
        panel.GetComponent<RectTransform>().sizeDelta = size;

        //Add a grid component if needed
        if (isGrid)
            panel.AddComponent<GridLayoutGroup>();
        
       //Set the parent
        panel.transform.SetParent(transform, false);

        return panel;
    }

    /// <summary>
    /// This function is used to create the text for the header of each of the sugar groups
    /// </summary>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    static void CreateText(string name, Transform parent)
    {
        //Create the object
        GameObject text = new GameObject("Text");
        text.AddComponent<Text>();
        text.transform.SetParent(parent, false);

        //Set the text settings
        Text textSettings = text.GetComponent<Text>();
        textSettings.text = name;
        textSettings.fontSize = 27;
        textSettings.alignment = TextAnchor.MiddleLeft;
        textSettings.horizontalOverflow = HorizontalWrapMode.Overflow;

        //Set the position and size settings
        RectTransform rectSettings = text.GetComponent<RectTransform>();
        rectSettings.position = new Vector3(-200, 225, 0);
        rectSettings.anchoredPosition = new Vector3(-200, 225, 0);
        rectSettings.sizeDelta = new Vector2(160, 30);
    }

    private void Start()
    {
        Debug.LogError("This script should not be attached to any object! Current Object =  <" + gameObject.name + ">");
    }
}
