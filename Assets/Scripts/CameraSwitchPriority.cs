using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwitchPriority : MonoBehaviour
{
    [SerializeField] private PlayerInput inputActions;
    [SerializeField] private CinemachineVirtualCamera vcam1;
    [SerializeField] private CinemachineVirtualCamera vcam2;

    private bool vcam1Active = true;

    void Awake()
    {
        inputActions = new PlayerInput();
        inputActions.Enable();
        inputActions.Player.SwitchCamera.performed += SwitchCamera;
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
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        else
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
    }
}
