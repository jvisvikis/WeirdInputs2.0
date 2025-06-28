using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordCAPTCHA : Window
{
    [SerializeField] private string wordCapTCHA;
    [SerializeField] private TMP_InputField wordCapTCHAInputField;
    public override bool CheckAnswer()
    {
        bool correct = wordCapTCHAInputField.text.Equals(wordCapTCHA);
        wordCapTCHAInputField.text = "";
        return correct;
    }

}
