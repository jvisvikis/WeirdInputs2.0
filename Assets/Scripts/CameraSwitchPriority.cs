using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitchPriority : MonoBehaviour
{
    [SerializeField] private PlayerInput inputActions;
    [SerializeField] private CinemachineVirtualCamera vcam1;
    [SerializeField] private CinemachineVirtualCamera vcam2;

    public bool vcam1Active = true;

    public static event Action<CameraSwitchPriority> OnSwitch;

    void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
        inputActions.Player.SwitchCamera.performed += SwitchCamera;
    }

    private void Update()
    {
        if(GameManager.Instance.gameWon)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void SwitchCamera(InputAction.CallbackContext ctx)
    {

        vcam1Active = !vcam1Active;
        if (vcam1Active)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        if (OnSwitch != null)
            OnSwitch(this);
    }

    
}
