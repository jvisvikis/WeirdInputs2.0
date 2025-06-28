using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginWindow : Window
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private string username;
    [SerializeField] private string password;
    public override bool CheckAnswer()
    {
        return usernameField.text.Equals(username) && passwordField.text.Equals(password);
    }
}
