using UnityEngine;

public class StateMachine
{
    public enum states { SET, STARTED, ENDED}
    private states currentState;

    public void ChangeState(states state)
    {
        currentState = state;
    }

    public states GetCurrentState()
    {
        return currentState;
    }
}
