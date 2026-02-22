using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState.Exit();

        currentState = newState;

        currentState.Enter();
    }

    void Update()
    {
        currentState?.Update();
    }
}
