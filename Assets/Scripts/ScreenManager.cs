using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class ScreenManager : MonoBehaviour
{
    [SerializeField] private List<Window> windows;
    [SerializeField] private PlayerInput inputActions;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private int correctIterationsNum;
    [SerializeField] private int windowTimeLimit;

    private int windowIdx;
    private int currentIteration;
    private float timer;
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
    }

    private void Update()
    {
        if (windows[windowIdx].IsTimed())
        { 
            timerText.gameObject.SetActive(false);
            timer += Time.deltaTime;
        }
        else
            timerText.gameObject.SetActive(true);

        
        if(timer > windowTimeLimit )
        {
            Reset();
        }
        timerText.text = (windowTimeLimit-(int)timer).ToString();
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
        timer = 0;
        windows[windowIdx].gameObject.SetActive(false);
        windows[++windowIdx].gameObject.SetActive(true);
    }

    public void GoToWindow(int idx)
    {
        windows[windowIdx].gameObject.SetActive(false);
        windowIdx = idx;
        windows[windowIdx].gameObject.SetActive(true);
    }

    public void FinishLoad()
    {
        if(currentIteration < correctIterationsNum)
        {
            currentIteration++;
            NextWindow();
        }
        else
        {
            //Win
            GoToWindow(windows.Count - 1);
        }
    }

    public void LoadFirstWindow()
    {
        windows[windowIdx].gameObject.SetActive(true) ;
    }

    private void Reset()
    {
        currentIteration = 0;
        timer = 0;
        if(windowIdx < 3)
            GoToWindow(0);
        else
            GoToWindow(3);
    }
}
