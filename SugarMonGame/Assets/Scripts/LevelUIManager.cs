//This file was create by Mark Botaish on June 12th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : MonoBehaviour
{
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

    private List<SugarButton> buttonGroups = new List<SugarButton>();  //This is the list of SugarButtons, which contains all the information needed for the sugar group
    private GameObject _allPanels;
    private GameObject _backButton;
    private GameObject _currentLevels;
    private GameObject _buttonPanel;

    private void Start()
    {
        _buttonPanel = GameObject.Find("Buttons");
        _allPanels = GameObject.Find("SugarPanels");
        _backButton = GameObject.Find("BackButton");
        _currentLevels = null;

        int size = _allPanels.transform.childCount;
        
        //Loop through the sugar groups
        for(int i = 0; i < size; i++)
        {
            SugarButton group = new SugarButton();
            Transform panel = _allPanels.transform.GetChild(i).Find("Scroll").GetChild(0);
            group.init(panel.gameObject, panel.childCount, panel.parent.parent.name.Remove(panel.parent.parent.name.Length - 5)); //Init the sugar group
            group._currentButton.onClick.AddListener(() => Unlock(panel.parent.parent.GetSiblingIndex())); //Set the current button to be active
            group._currentButton.gameObject.transform.GetChild(1).gameObject.SetActive(true); //Set the current button to have the red square
            buttonGroups.Add(group); 
            
            //Update the titles
            Transform child = panel.parent.parent.GetChild(0);
            child.GetComponent<Text>().text = buttonGroups[i]._name + " - " + (buttonGroups[i]._currentButtonIndex) + "/" + buttonGroups[i]._buttonCount;
            _allPanels.transform.GetChild(i).gameObject.SetActive(false);
        }     
        
    }

    /// <summary>
    /// This function is called when a button is clicked. This changes the current button to have a green check mark on it
    /// along with have the icon turn gold. It takes the next button an activates it and places a red square around the icon.
    /// Index is the sugar group index as each group has it's own progression.
    /// </summary>
    /// <param name="index"></param>
    public void Unlock(int index)
    {
        print(index);

        buttonGroups[index]._currentButton.interactable = false; //Disables the button
        buttonGroups[index]._currentButton.transform.GetChild(0).gameObject.SetActive(true); // Enables the check mark
        buttonGroups[index]._currentButton.transform.GetChild(1).gameObject.SetActive(false);  //Removes the red square

        //Updates the titles of the sugar groups
        Transform child = buttonGroups[index]._currentGroup.transform.parent.parent.GetChild(0);
        child.GetComponent<Text>().text = buttonGroups[index]._name + " - " + (buttonGroups[index]._currentButtonIndex + 1) + "/" + buttonGroups[index]._buttonCount; 

        //Changes the tint of the button to gold
        ColorBlock block = buttonGroups[index]._currentButton.colors; 
        block.disabledColor = Color.yellow;
        buttonGroups[index]._currentButton.colors = block;

        //If the button was not the last button in the group update the next button
        if (buttonGroups[index]._currentButtonIndex < buttonGroups[index]._currentGroup.transform.childCount - 1)
        {
            buttonGroups[index]._currentButtonIndex++; 
            GameObject button = buttonGroups[index]._currentGroup.transform.GetChild(buttonGroups[index]._currentButtonIndex).gameObject; //Gets the next button
            button.GetComponent<Button>().interactable = true; //Enables the button
            button.GetComponent<Button>().onClick.AddListener(() => Unlock(index)); //Set the OnClick functionality
            buttonGroups[index]._currentButton = button.GetComponent<Button>();
            buttonGroups[index]._currentButton.transform.GetChild(1).gameObject.SetActive(true); //Enables the red square
        }       
    }


    public void ToggleButtons()
    {
        _buttonPanel.SetActive(!_buttonPanel.activeSelf);
    }

    public void ActivatePanel(int num)
    {
        _currentLevels = _allPanels.transform.GetChild(num).gameObject;
        _currentLevels.SetActive(true);
        ToggleButtons();
    }

    public void GoBack()
    {
        if(_currentLevels != null)
        {
            _currentLevels.SetActive(false);
            _currentLevels = null;
            ToggleButtons();
        }
       
    }
}
