using UnityEngine;

public class Detail : IState
{
    public CameraController camera;
    private StateMachine machine;

    public Detail(StateMachine machine)
    {
        this.machine = machine;
        camera = machine.Camera;
    }

    public void OnEnter()
    {
        Vector3 pos = camera.Forward(-camera.BackwardDistance);
        Vector3 angle = camera.Deflection(camera.DeflectionAngle);
        Quaternion rotation = Quaternion.Euler(angle);
        camera.SetDestination(pos, rotation);
        UIManager.Instance.DetailPanel.Open();
        UIManager.Instance.DetailPanel.SetContent(camera.CurrentTarget.Content);
    }

    public void OnExit()
    {
        Vector3 pos = camera.Forward(camera.BackwardDistance);
        Vector3 angle = camera.Deflection(camera.DeflectionAngle);
        Quaternion rotation = Quaternion.Euler(angle);
        camera.SetDestination(pos, rotation);
        UIManager.Instance.DetailPanel.Close();
    }

    public void OnUpdate()
    {
        if (camera.EscDown) machine.SwitchState(State.Revolve);
        else UIManager.Instance.ShowTip("Esc返回上一级");
        camera.RevolveMove();
    }
}
