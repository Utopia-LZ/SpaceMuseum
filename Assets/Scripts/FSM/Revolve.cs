using UnityEngine;

public class Revolve : IState
{
    public CameraController camera;
    private StateMachine machine;
    private GameObject[] targets;
    private int targetLayer;

    public Revolve(StateMachine machine)
    {
        this.machine = machine;
        camera = machine.Camera;
        targetLayer = (int)Mathf.Log(camera.TargetLayer, 2);
    }

    public void OnEnter()
    {
        camera.TargetCamera.SetActive(true);
        camera.CurrentTarget.SetLayer(targetLayer);
        camera.SetDestination(camera.RevolvePos, camera.RevolveRot);
    }

    public void OnExit()
    {
    }

    public void OnUpdate()
    {
        if (camera.CurrentTarget.CanSplit)
        {
            if(camera.InteractionB) machine.SwitchState(State.Split);
            else UIManager.Instance.ShowTip("����Ҽ����");
        }
        if (camera.EscDown) machine.SwitchState(State.Roam);
        else if(camera.InteractionA) machine.SwitchState(State.Detail);
        else
        {
            camera.RevolveMove();
            UIManager.Instance.ShowTip("Esc������һ��");
            UIManager.Instance.ShowTip("�������鿴����");
        }
    }
}
