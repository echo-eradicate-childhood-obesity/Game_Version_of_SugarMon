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

        public void init(GameObject panel, int size, int index = 0)
        {
            _buttonCount = size;
            _currentButtonIndex = index;
            _currentGroup = panel;
            _currentButton = panel.transform.GetChild(index).gameObject.GetComponent<Button>();

        }
    }

    private List<SugarButton> buttonGroups = new List<SugarButton>();  //This is the list of SugarButtons, which contains all the information needed for the sugar group
    private Transform titleHead;                                       //The head of all the headers/text of the different sugar groups

    private void Start()
    {
        GameObject panel = GameObject.Find("SugarPanels");
        titleHead = GameObject.Find("Titles").transform;

        int size = panel.transform.childCount - 1;
        int index = 0;

        //Loop through the sugar groups
        for(int i = 0; i < size; i++)
        {
            SugarButton group = new SugarButton();
            group.init(panel.transform.GetChild(i).gameObject, panel.transform.GetChild(i).childCount); //Init the sugar group
            group._currentButton.onClick.AddListener(() => Unlock(index++)); //Set the current button to be active
            group._currentButton.gameObject.transform.GetChild(1).gameObject.SetActive(true); //Set the current button to have the red square
            buttonGroups.Add(group); 
            
            //Update the titles
            Transform child = titleHead.GetChild(i);
            child.GetComponent<Text>().text = child.name + " - " + (buttonGroups[i]._currentButtonIndex) + "/" + buttonGroups[i]._buttonCount;
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
        buttonGroups[index]._currentButton.interactable = false; //Disables the button
        buttonGroups[index]._currentButton.transform.GetChild(0).gameObject.SetActive(true); // Enables the check mark
        buttonGroups[index]._currentButton.transform.GetChild(1).gameObject.SetActive(false);  //Removes the red square

        Transform child = titleHead.GetChild(index);
        child.GetComponent<Text>().text = child.name + " - " + (buttonGroups[index]._currentButtonIndex + 1) + "/" + buttonGroups[index]._buttonCount; //Updates the title

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
}
