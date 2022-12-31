using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByKey : PlayerMoving
{
    protected override void Update()
    {
        if (GameController.IsGameOver) return;
        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        base.Update();
    }

    protected override Vector2 GetInput()
    {
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    protected override bool GetRunning()
    {
        return Input.GetKey(KeyCode.LeftShift);
    }
}
