﻿//This file was create by Mark Botaish on June 12th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUIManager : MonoBehaviour
{
    #region   STRUCTS/CLASSES
    //The infomation needed for each sugar group
    [System.Serializable]
    public class SugarButton
    {
        public Button _currentButton;
        public GameObject _currentGroup;
        public int _currentButtonIndex;
        public int _buttonCount;
        public string _name;

        public void init(GameObject panel, int size, string name, int index = 0)
        {
            _name = name;
            _buttonCount = size;
            _currentButtonIndex = index;
            _currentGroup = panel;
            _currentButton = panel.transform.GetChild(index).gameObject.GetComponent<Button>();
        }
    }
    #endregion

    #region PRIVATE_VARS
    private List<SugarButton> buttonGroups = new List<SugarButton>();   // This is the list of SugarButtons, which contains all the information needed for the sugar group
    private GameObject _allPanels;                                      // This is a reference to the Head of the sugar panels
    private GameObject _backButton;                                     // This is a reference to the back button in the scene
    private GameObject _currentLevels;                                  // This is a reference to the current active panel 
    private GameObject _buttonPanel;                                    // This is a reference to the back button in the scene
    private GameObject _selectionPanel;                                 // This is a reference to the selection panel in the scene

    int _currentSelection = 0;                                          // This is a current button index that is showing on screen
    float _selectionOffset;                                             // This is the offset position of the first button
    float _targetLocation;                                              // This is the current target position       
    Coroutine _selectionAnim = null;                                    // This is a reference to the current coroutine running the animation 
    #endregion

    private void Start()
    {
        //Get all of the references in the game
        _buttonPanel = GameObject.Find("Buttons");
        _allPanels = GameObject.Find("SugarPanels");
        _backButton = GameObject.Find("BackButton");
        _selectionPanel = GameObject.Find("Selection");       
   
        _currentLevels = null;

        int size = _allPanels.transform.childCount;

        //Loop through the sugar groups
        for(int i = 0; i < size; i++)
        {
            SugarButton group = new SugarButton();
            Transform panel = _allPanels.transform.GetChild(i).GetChild(0);
            group.init(panel.gameObject, panel.childCount, panel.name.Remove(panel.name.Length - 3)); //Init the sugar group
            group._currentButton.onClick.AddListener(() => Unlock(panel.parent.GetSiblingIndex())); //Set the current button to be active
            buttonGroups.Add(group);

            //Update the titles
            Transform child = panel.parent.Find("Text");
            child.GetComponent<Text>().text = buttonGroups[i]._name + " - " + (buttonGroups[i]._currentButtonIndex) + "/" + buttonGroups[i]._buttonCount;
            //_allPanels.transform.GetChild(i).gameObject.SetActive(false);
        }
        _selectionOffset = -_selectionPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x / buttonGroups.Count;
        _targetLocation = -_selectionPanel.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.x;

    }

    /// <summary>
    /// This function is called when a button is clicked. This changes the current button to have a green check mark on it
    /// along with have the icon turn gold. It takes the next button an activates it and places a red square around the icon.
    /// Index is the sugar group index as each group has it's own progression.
    /// </summary>
    /// <param name="index"></param>
    public void Unlock(int index)
    {
        buttonGroups[index]._currentButton.interactable = false; //Disables the button

        //Updates the titles of the sugar groups
        Transform child = buttonGroups[index]._currentGroup.transform.parent.Find("Text");
        child.GetComponent<Text>().text = buttonGroups[index]._name + " - " + (buttonGroups[index]._currentButtonIndex + 1) + "/" + buttonGroups[index]._buttonCount; 

        //Changes the tint of the button to gold
        ColorBlock block = buttonGroups[index]._currentButton.colors; 
        block.disabledColor = Color.green;
        buttonGroups[index]._currentButton.colors = block;

        //If the button was not the last button in the group update the next button
        if (buttonGroups[index]._currentButtonIndex < buttonGroups[index]._currentGroup.transform.childCount - 1)
        {
            buttonGroups[index]._currentButtonIndex++; 
            GameObject button = buttonGroups[index]._currentGroup.transform.GetChild(buttonGroups[index]._currentButtonIndex).gameObject; //Gets the next button
            button.GetComponent<Button>().interactable = true; //Enables the button
            button.GetComponent<Button>().onClick.AddListener(() => Unlock(index)); //Set the OnClick functionality
            buttonGroups[index]._currentButton = button.GetComponent<Button>();
        }       
    }

    /// <summary>
    /// This function is used to toggle the Sugar Group button panels
    /// </summary>
    public void ToggleButtons(){_buttonPanel.SetActive(!_buttonPanel.activeSelf);}

    /// <summary>
    /// This function is used to activate the correct sugar group panel 
    /// </summary>
    /// <param name="num"></param>
    public void ActivatePanel(int num)
    {
        _currentLevels = _allPanels.transform.GetChild(num).gameObject;
        _currentLevels.SetActive(true);
        ToggleButtons();
    }

    /// <summary>
    /// This function is used to go back to the sugar group buttons and disable
    /// the current sugar buttons panel
    /// </summary>
    public void GoBack()
    {
        if(_currentLevels != null)
        {
            _currentLevels.SetActive(false);
            _currentLevels = null;
            ToggleButtons();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    /// <summary>
    /// This function is used to move the selection screen to the correct button. This function should be called
    /// by the arrows in the scene.
    /// </summary>
    /// <param name="dir"></param>
    public void MoveSelectionScreen(int dir)
    {
        dir = (dir / Mathf.Abs(dir)); //Ensure that die is either -1 or 1
        int prev = _currentSelection; //Get the currently selected
        _currentSelection = Mathf.Clamp(_currentSelection + dir,0 , buttonGroups.Count -1); //Change selection but clamp the values

        if (prev != _currentSelection) //If the two selections are different move the panel
        {
            _targetLocation += dir * _selectionOffset;
            if (_selectionAnim != null)
                StopCoroutine(_selectionAnim);
            _selectionAnim = StartCoroutine(MoveSelection(_selectionPanel.transform.GetChild(0).GetComponent<RectTransform>()));
        }

    }


    /// <summary>
    /// This fucntion is to lerp between the two postions to create a smooth animation 
    /// when transitioning between buttons.
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    IEnumerator MoveSelection(RectTransform panel)
    {
        float timer = 0;
        float timeToComplete = 1;

        while (panel.anchoredPosition.x != _targetLocation)
        {
            float x = Mathf.Lerp(panel.anchoredPosition.x, _targetLocation, timer/ timeToComplete);
            panel.anchoredPosition = new Vector2(x, panel.anchoredPosition.y);
            timer += Time.deltaTime;            

            yield return null;
        }
        _selectionAnim = null;
    }
}
