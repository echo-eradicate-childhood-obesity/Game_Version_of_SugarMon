//This file was created by Zakir Chaudry on June 28th, 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This file is designed to fix any issues with the sizes of objects in the scene
/// </summary>
public class ScreenHelper : MonoBehaviour
{
    /// <summary>
    /// The canvas containing the buttons
    /// </summary>
    [Tooltip("The canvas containing the buttons")]
    public GameObject buttonContainer;
    /// <summary>
    /// The main canvas for the scene
    /// </summary>
    [Tooltip("The main canvas for the scene.")]
    public GameObject mainCanvas;
    /// <summary>
    /// The first button
    /// </summary>
    public Button button1;
    /// <summary>
    /// The second button
    /// </summary>
    public Button button2;
    /// <summary>
    /// The third button
    /// </summary>
    public Button button3;
    /// <summary>
    /// The fourth button
    /// </summary>
    public Button button4;
    /// <summary>
    /// Which button is selected
    /// </summary>
    public Button selectedButton;
    /// <summary>
    /// List of buttons
    /// </summary>
    Button[] buttons;

    // Start is called before the first frame update
    void Start()
    {
        buttonContainer = this.transform.gameObject;
        mainCanvas = this.transform.parent.gameObject;
        buttons = new Button[] { button1, button2, button3, button4 };
        selectedButton = buttons[0];
        SelectButton1();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateButtons()
    {
        selectedButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
        Vector3 pos = selectedButton.GetComponent<RectTransform>().position;
        selectedButton.GetComponent<RectTransform>().position = new Vector3(pos.x + 20, pos.y, pos.z);
        foreach (Button button in buttons)
        {
            if (button != selectedButton)
            {
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
            }
        }
    }

    void SelectButton1()
    {
        button1.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
        Vector3 pos = button1.GetComponent<RectTransform>().position;
        button1.GetComponent<RectTransform>().position = new Vector3(pos.x + 20, pos.y, pos.z);
        int i = 0;
        foreach (Button button in buttons)
        {
            if (button != button1)
            {
                button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
                pos = button.GetComponent<RectTransform>().position;
                button.GetComponent<RectTransform>().position = new Vector3(pos.x + 20, pos.y, pos.z);
            }
        }
    }

}
