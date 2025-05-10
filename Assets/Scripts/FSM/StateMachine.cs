using System.Collections.Generic;

public interface IState
{
    public void OnEnter();
    public void OnUpdate();
    public void OnExit();
}

public enum State
{
    None, Roam, Revolve, Split, Detail
}

public class StateMachine
{
    public Dictionary<State, IState> States = new();
    public State curStateType;
    public IState CurrentState;
    public CameraController Camera;

    public StateMachine(CameraController Camera)
    {
        this.Camera = Camera;
        States[State.Roam] = new Roam(this);
        States[State.Revolve] = new Revolve(this);
        States[State.Detail] = new Detail(this);
        States[State.Split] = new Split(this);
        SwitchState(State.Roam);
    }

    public void Update()
    {
        CurrentState.OnUpdate();
    }

    public void SwitchState(State state)
    {
        CurrentState?.OnExit();
        CurrentState = States[state];
        CurrentState.OnEnter();
        curStateType = state;
    }
}