using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HumanoidLandInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }= Vector2.zero;
    public bool MoveIsPressed = false;
    public Vector2 LookInput { get; private set; }= Vector2.zero;
    public bool InvertMouseY { get; private set; } = true;
    public bool RunIsPressed = false;
    private InputActions _input = null;

    private void OnEnable()
    {
        _input = new InputActions();
        _input.HumanoidLand.Enable();

        _input.HumanoidLand.Move.performed += SetMove;
        _input.HumanoidLand.Move.canceled += SetMove;

        _input.HumanoidLand.Look.performed += SetLook;
        _input.HumanoidLand.Look.canceled += SetLook;
        
        _input.HumanoidLand.Run.performed += SetRun;
        _input.HumanoidLand.Run.canceled += SetRun;
        
    }
    
    private void OnDisable()
    {
        _input.HumanoidLand.Move.performed -= SetMove;
        _input.HumanoidLand.Move.canceled -= SetMove;

        _input.HumanoidLand.Look.performed -= SetLook;
        _input.HumanoidLand.Look.canceled -= SetLook;
        
        _input.HumanoidLand.Run.performed -= SetRun;
        _input.HumanoidLand.Run.canceled -= SetRun;
        
        _input.HumanoidLand.Disable();
    }

    private void SetMove(InputAction.CallbackContext ctx)
    {
        MoveInput = ctx.ReadValue<Vector2>();
        MoveIsPressed = !(MoveInput == Vector2.zero);
    }
    
    private void SetRun(InputAction.CallbackContext ctx)
    {
        MoveIsPressed = ctx.started;
    }
    
    private void SetLook(InputAction.CallbackContext ctx)
    {
        LookInput = ctx.ReadValue<Vector2>();
        
    }
    
}
