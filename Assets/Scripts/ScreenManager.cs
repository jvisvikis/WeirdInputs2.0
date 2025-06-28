using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private List<Window> windows;
    [SerializeField] private PlayerInput inputActions;
    [SerializeField] private int correctIterationsNum;

    private int windowIdx;
    private int currentIteration;
    private void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
        inputActions.Player.Submit.performed += SubmitAnswer;
    }
    // Start is called before the first frame update
    void Start()
    {
         foreach(Window window in windows) 
            window.gameObject.SetActive(false);
        windows[0].gameObject.SetActive(true);
            
    }

    public void SubmitAnswer(InputAction.CallbackContext ctx)
    {
        SubmitAnswer();
    }

    public void SubmitAnswer()
    {
        if (windows[windowIdx].CheckAnswer())
            NextWindow();
        else
            Reset();
    }

    public void NextWindow()
    {
        windows[windowIdx].gameObject.SetActive(false);
        windows[++windowIdx].gameObject.SetActive(true);
    }

    public void GoToWindow(int idx)
    {
        windows[windowIdx].gameObject.SetActive(false);
        windowIdx = idx;
        windows[windowIdx].gameObject.SetActive(true);
    }

    private void Reset()
    {
        currentIteration = 0;
        if(windowIdx < 3)
            GoToWindow(0);
        else
            GoToWindow(3);
    }
}
