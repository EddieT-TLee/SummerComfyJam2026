using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// I AM GOING TO EXPLODE YOU IF DON'T UNDERSTAND THIS CODE
/// </summary>
public class Keypad : MonoBehaviour, IInteractable
{
    public GameObject keypad;
    public TMP_Text displayText;
    public Button[] numberButtons;
    public Button enterButton;
    
    private bool isKeypadActive;
    private string currentCombination;
    private int correctAttempts;


    void Start()
    {
        foreach (Button btn in numberButtons)
        {
            btn.onClick.AddListener(() => OnNumberPressed(btn));
        }
        enterButton.onClick.AddListener(() => OnEnterPressed());
        displayText.text = "";
    }
    public bool CanInteract()
    {
        return !isKeypadActive;
    }

    public void Interact()
    {
       keypad.SetActive(true);
    }

    public void CloseKeypad()
    {
        isKeypadActive = false;
        currentCombination = "";
        keypad.SetActive(false);
    }

    public void OnNumberPressed(Button btn)
    {
        if (displayText.text.Length < 4)
        {
            displayText.text += btn.name;
            currentCombination += btn.name;
        }
    }
    
    void OnEnterPressed()
    {
        if (currentCombination == "0000" && correctAttempts < 6)
        {
            displayText.text = "TRY AGAIN";
            correctAttempts++;
            StartCoroutine(ClearScreen());
        }
        else if (correctAttempts == 6)
        {
            displayText.text = "CORRECT";
        }
        else
        {
            displayText.text = "INCORRECT";
            StartCoroutine(ClearScreen());
        }
    }
    
    IEnumerator ClearScreen() 
    {
        yield return new WaitForSeconds(0.7f);
        displayText.text = "";
        currentCombination = "";
    }
}
