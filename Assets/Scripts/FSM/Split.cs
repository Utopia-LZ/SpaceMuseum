using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Split : IState
{
    public CameraController camera;
    private StateMachine machine;
    private float splitRate;

    public Split(StateMachine machine)
    {
        this.machine = machine;
        camera = machine.Camera;
    }

    public void OnEnter()
    {
        splitRate = camera.CurrentTarget.SplitRate;
        camera.MaxDistance *= splitRate;
        camera.MinDistance *= splitRate;
        Vector3 pos = camera.Forward(-splitRate);
        camera.SetDestination(pos, camera.transform.rotation);
        camera.CurrentTarget.Split();
    }

    public void OnExit()
    {
        camera.MaxDistance /= splitRate;
        camera.MinDistance /= splitRate;
        Vector3 pos = camera.Forward(splitRate);
        camera.SetDestination(pos, camera.transform.rotation);
        camera.CurrentTarget.Assemble();
    }

    public void OnUpdate()
    {
        if (camera.EscDown) machine.SwitchState(State.Revolve);
        else camera.RevolveMove();
        UIManager.Instance.ShowTip("Esc返回上一级");
    }
}
