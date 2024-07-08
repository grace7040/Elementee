using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private BaseState currentState;

    public void ChangeState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }
}