using UnityEngine;

public class Roam : IState
{
    private CameraController camera;
    private StateMachine machine;
    private Vector3 lastPosition;
    private Quaternion lastRotation = Quaternion.identity;
    private LayerMask modelLayer;

    public Roam(StateMachine machine, Vector3 startPos)
    {
        this.machine = machine;
        lastPosition = startPos;
        camera = machine.Camera;
        modelLayer = (int)Mathf.Log(camera.ModelLayer, 2);
    }

    public void OnEnter()
    {
        camera.TargetCamera.SetActive(false);
        camera.CurrentTarget?.SetLayer(modelLayer);
        camera.CurrentTarget = null;
        camera.SetDestination(lastPosition, lastRotation);
    }

    public void OnExit()
    {
        lastPosition = camera.transform.position;
        lastRotation = camera.transform.rotation;
    }

    public void OnUpdate()
    {
        if (camera.CurrentTarget != null) machine.SwitchState(State.Revolve);

        camera.RoamMove();
        camera.DetectTarget();
    }
}
