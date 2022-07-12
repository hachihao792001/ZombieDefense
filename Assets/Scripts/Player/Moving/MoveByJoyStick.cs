using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByJoyStick : PlayerMoving
{
    [SerializeField]
    private JoyStick _joyStick;

    [SerializeField]
    private ButtonHolding _runButton;

    protected override Vector2 GetInput()
    {
        return _joyStick.Input;
    }

    protected override bool GetRunning()
    {
        return _runButton.IsHolding;
    }

    public void JumpOnClick()
    {
        Jump();
    }
}
