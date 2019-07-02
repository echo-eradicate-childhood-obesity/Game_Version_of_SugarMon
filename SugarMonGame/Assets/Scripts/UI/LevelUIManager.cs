//This file was create by Mark Botaish on June 12th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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

        public void init(GameObject panel, int index)
        {
            _name = panel.name.Remove(panel.name.Length - 3);
            _buttonCount = panel.transform.childCount;
            _currentButtonIndex = index;
            _currentGroup = panel;            
            _currentButton = panel.transform.GetChild(index).gameObject.GetComponent<Button>();
        }
    }

    [System.Serializable]
    public struct Powers
    {
        public Button _powerup;
        public int _levelToUnlock;
    }
    #endregion

    #region PUBLIC_VARS
    public List<Powers> powers;
    public TextMeshProUGUI tempCoins;
    public TextMeshProUGUI tempLevel;
    #endregion

    #region PRIVATE_VARS
    private List<SugarButton> buttonGroups = new List<SugarButton>();   // This is the list of SugarButtons, which contains all the information needed for the sugar group
    private List<GameObject> _panelOrder = new List<GameObject>();
    private GameObject _allPanels;                                      // This is a reference to the Head of the sugar panels
    private GameObject _backButton;                                     // This is a reference to the back button in the scene
    private GameObject _currentLevels;                                  // This is a reference to the current active panel 
    private GameObject _buttonPanel;                                    // This is a reference to the back button in the scene
    private GameObject _selectionPanel;                                 // This is a reference to the selection panel in the scene
    private GameObject _skillsPanel;
    private GameObject _mainMenuPanel;

    int _currentSelection = 0;                                          // This is a current button index that is showing on screen
    float _selectionOffset;                                             // This is the offset position of the first button
    float _targetLocation;                                              // This is the current target position       
    Coroutine _selectionAnim = null;                                    // This is a reference to the current coroutine running the animation

    private PlayerInfoScript info;
    #endregion

    private void Start()
    {
        //Get all of the references in the game
        _buttonPanel = GameObject.Find("Buttons");
        _allPanels = GameObject.Find("SugarPanels");
        _backButton = GameObject.Find("BackButton");
        _selectionPanel = GameObject.Find("Selection");
        _skillsPanel = GameObject.Find("SkillsPanel");
        _mainMenuPanel = GameObject.Find("MainMenu");
       
        _allPanels.SetActive(false);   
        _selectionPanel.SetActive(false);        
        _skillsPanel.SetActive(false);   

        info = PlayerInfoScript.instance;

        _currentLevels = _mainMenuPanel;

        int size = _allPanels.transform.childCount;

        //Loop through the sugar groups panels
        for(int i = 0; i < size; i++)
        {
            SugarButton group = new SugarButton();
            Transform panel = _allPanels.transform.GetChild(i).GetChild(0);

            _allPanels.transform.GetChild(i).GetComponent<ScrollRect>().verticalNormalizedPosition = 1; //Automatically scroll to the top
            group.init(panel.gameObject,0);//Init the sugar group
            group._currentButton.onClick.AddListener(() => Unlock(panel.parent.GetSiblingIndex())); //Set the current button to be active
            group._currentButton.interactable = true;
            buttonGroups.Add(group);

            //Update the buttons to be at the location last saved (Future: From the save file)
            UpdateButtons(buttonGroups[i]);

            //Update the titles
            Transform child = panel.parent.Find("Text");
            child.GetComponent<Text>().text = buttonGroups[i]._name + " - " + (buttonGroups[i]._currentButtonIndex) + "/" + buttonGroups[i]._buttonCount;

            panel.parent.gameObject.SetActive(false);
        }

        //Settings for the "map" selection animatoin s
        _selectionOffset = -_selectionPanel.transform.GetChild(0).GetComponent<RectTransform>().sizeDelta.x / buttonGroups.Count;
        _targetLocation = -_selectionPanel.transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().anchoredPosition.x;

    }
    
    #region  PUBLIC FUNCTIONS

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
    /// This function is used to activate the correct sugar group panel 
    /// </summary>
    /// <param name="num"></param>
    public void ActivatePanel(int num)
    {
        _panelOrder.Add(_currentLevels);
        _currentLevels = _allPanels.transform.GetChild(num).gameObject;
        _currentLevels.transform.parent.gameObject.SetActive(true);
        _currentLevels.SetActive(true);
        ToggleButtons();
    }

    /// <summary>
    /// This function is used to go back to the sugar group buttons and disable
    /// the current sugar buttons panel
    /// </summary>
    public void GoBack()
    {
        _currentLevels.SetActive(false);
        if (_panelOrder.Count > 0)
        {
            _currentLevels = _panelOrder[_panelOrder.Count - 1];
            _panelOrder.RemoveAt(_panelOrder.Count - 1);
        }           
        else
            _currentLevels = _mainMenuPanel;
        
        _currentLevels.SetActive(true);
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
        _currentSelection = Mathf.Clamp(_currentSelection + dir, 0, buttonGroups.Count - 1); //Change selection but clamp the values

        if (prev != _currentSelection) //If the two selections are different move the panel
        {
            _targetLocation += dir * _selectionOffset;
            if (_selectionAnim != null)
                StopCoroutine(_selectionAnim);
            _selectionAnim = StartCoroutine(MoveSelection(_selectionPanel.transform.GetChild(0).GetComponent<RectTransform>()));
        }

    }

    public void IncreasePowerLevel(TextMeshProUGUI text)
    {
        string name = text.gameObject.transform.parent.name;
        int coins = info.BuyLevel(name);

        if(coins > 0)
        {
            text.text = "Level " + info.GetPowerLevel(name) + " \n(<color=green>"+ info.GetBuyAmmount(name) + "</color>)";
            tempCoins.text = info.GetCoinCount().ToString("00000000");
        }
        else
        {
            print("NO ENOUGH COINS");
        }

    }

    public void GoToLevelSelectScreen()
    {
        _currentLevels.SetActive(false);
        _currentLevels = _selectionPanel;
        _currentLevels.SetActive(true);
    }

    public void GoToSkillsScreen()
    {
        _currentLevels.SetActive(false);
        _currentLevels = _skillsPanel;
        _currentLevels.SetActive(true);
    }

    #endregion

    #region PRIVATE FUNCTIONS

    private void UpdatePowers(int level)
    {
        for(int i = 0; i < powers.Count; i++)
        {
            if(level >= powers[i]._levelToUnlock)
            {
                string name = powers[i]._powerup.name;
                powers[i]._powerup.interactable = true;
                powers[i]._powerup.transform.Find("Level").GetComponent<TextMeshProUGUI>().text = "Level 0" + " \n(<color=green>" + info.GetBuyAmmount(name) + "</color>)";
                powers.RemoveAt(i);
                i--;
            }
        }
    }

    /// <summary>
    /// This function is used to turn all of the buttons green before the current buttons. 
    /// This will visually show that the levels have been completed in the past. 
    /// This should be used in conjunction with a saved file.
    /// </summary>
    /// <param name="group"></param>
    private void UpdateButtons(SugarButton group)
    {
        Transform trans = group._currentGroup.transform;
        int size = group._currentButtonIndex;
        for (int i = 0; i < size; i++)
        {
            Button but = trans.GetChild(i).GetComponent<Button>();

            ColorBlock block = but.colors;
            block.disabledColor = Color.green;
            but.colors = block;
        }
    }

    /// <summary>
    /// This function is used to toggle the Sugar Group button panels
    /// </summary>
    private void ToggleButtons() { _selectionPanel.SetActive(!_selectionPanel.activeSelf); }

    /// <summary>
    /// This fucntion is to lerp between the two postions to create a smooth animation 
    /// when transitioning between buttons.
    /// </summary>
    /// <param name="panel"></param>
    /// <returns></returns>
    private IEnumerator MoveSelection(RectTransform panel)
    {
        float timer = 0;
        float timeToComplete = 1;

        while (panel.anchoredPosition.x != _targetLocation)
        {
            float x = Mathf.Lerp(panel.anchoredPosition.x, _targetLocation, timer / timeToComplete);
            panel.anchoredPosition = new Vector2(x, panel.anchoredPosition.y);
            timer += Time.deltaTime;

            yield return null;
        }
        _selectionAnim = null;
    }

    private void IncreaseXP(int xp)
    {
        int prevLevel = info.GetLevel();
        info.AddXp(xp);
        int currentLevel = info.GetLevel();

        tempLevel.text = "" + currentLevel;

        if (powers.Count > 0 && prevLevel < currentLevel)
            UpdatePowers(currentLevel);

    }

    private void IncreaseCoinCount(int coins)
    {
        info.AddCoins(coins);
        tempCoins.text = info.GetCoinCount().ToString("00000000");
    }

    #endregion

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("ADDING COINS");
            IncreaseCoinCount(100);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            print("ADDING XP");
            IncreaseXP(100);
        }
    }

}
