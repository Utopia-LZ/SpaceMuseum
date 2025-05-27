using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("漫游参数")]
    public float MouseXSpeed = 1f;
    public float MouseYSpeed = 1f;
    public float HorizontalMoveSpeed = 3f;
    public float HorizontalRunSpeed = 5f;
    public float VerticalMoveSpeed = 1f;
    public float MaxInteractionDistance = 10f;
    public Transform VerticalNode;
    public LayerMask ModelLayer;
    public Vector3 StartPosition;

    [Header("环绕参数")]
    public float MaxDistance = 1.2f;
    public float MinDistance = 0.6f;
    public float RevolveSpeed = 1f;
    public float ScaleSpeed = 0.5f;
    public LayerMask TargetLayer;
    public GameObject TargetCamera;

    [Header("详情参数")]
    public float BackwardDistance = 1.5f;
    public float DeflectionAngle = 25f;

    [Header("其他")]
    public Model CurrentTarget;
    public bool EscDown;
    public bool LeftShift;
    public bool InteractionA;
    public bool InteractionB;
    public bool SwitchHelp;
    public bool Quit;
    public Vector3 RevolvePos;
    public Quaternion RevolveRot;
    private float MoveX;
    private float MoveY;
    private float MoveV;
    private float MouseX;
    private float MouseY;
    private float Scroll;
    private float CurrentSpeed;
    private Vector3 TargetPos;
    private StateMachine sm;
    private Vector3 desPosition;
    private Quaternion desRotation;
    private Vector3 oldPosition;
    private Quaternion oldRotation;
    private bool isLerping = false;
    private float lerpTime = 0f;

    private void Awake()
    {
        sm = new StateMachine(this);
        Cursor.visible = false;
    }

    void Update()
    {
        GetInputs();

        if (isLerping) Lerping();
        else sm.Update();
    }

    private void GetInputs()
    {
        Quit = Input.GetKeyDown(KeyCode.Q);
        SwitchHelp = Input.GetKeyDown(KeyCode.H);
        MoveX = Input.GetAxis("Horizontal");
        MoveY = Input.GetAxis("Vertical");
        MoveV = Input.GetKey(KeyCode.Space) ? VerticalMoveSpeed : 0;
        MoveV += Input.GetKey(KeyCode.LeftControl) ? -VerticalMoveSpeed : 0;
        LeftShift = Input.GetKey(KeyCode.LeftShift);
        CurrentSpeed = LeftShift ? HorizontalRunSpeed : HorizontalMoveSpeed;
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");
        Scroll = Input.GetAxis("Mouse ScrollWheel");
        EscDown = Input.GetKeyDown(KeyCode.Escape);
        InteractionA = Input.GetMouseButtonDown(0);
        InteractionB = Input.GetMouseButtonDown(1);
    }

    public void RoamMove()
    {
        transform.position += CurrentSpeed * Time.deltaTime * (MoveX * transform.right + MoveY * transform.forward);
        transform.position += VerticalMoveSpeed * MoveV * Time.deltaTime * Vector3.up;
        transform.Rotate(Vector3.up, MouseX * MouseXSpeed);
        VerticalNode.Rotate(Vector3.left, MouseY * MouseXSpeed);
    }

    public void RevolveMove()
    {
        transform.RotateAround(TargetPos, Vector3.up, MouseX * RevolveSpeed);
        transform.RotateAround(TargetPos, -transform.right, MouseY * RevolveSpeed);

        float dis = Vector3.Distance(transform.position, TargetPos);
        if (Scroll < -0.01f && dis < MaxDistance || Scroll > 0.01f && dis > MinDistance)
        {
            transform.position += Scroll * ScaleSpeed * transform.forward;
        }
    }

    public void SetDestination(Vector3 position, Quaternion rotation)
    {
        oldPosition = transform.position;
        oldRotation = transform.rotation;
        desPosition = position;
        desRotation = rotation;
        lerpTime = 0;
        isLerping = true;
    }

    private void Lerping()
    {
        lerpTime += Time.deltaTime;
        transform.position = Vector3.Lerp(oldPosition, desPosition, lerpTime);
        transform.rotation = Quaternion.Lerp(oldRotation, desRotation, lerpTime);
        if (lerpTime >= 1f)
        {
            isLerping = false;
            lerpTime = 0;
        }
    }

    public void DetectTarget()
    {
        if (Physics.Raycast(transform.position, VerticalNode.forward, out RaycastHit hit, MaxInteractionDistance, ModelLayer, QueryTriggerInteraction.Collide))
        {
            UIManager.Instance.ShowTip("点击左键观察模型");
            if (InteractionA)
            {
                TargetPos = hit.transform.position;
                Vector3 dir = (transform.position - TargetPos).normalized;
                RevolvePos = TargetPos + (MaxDistance + MinDistance) / 2f * dir;
                RevolveRot = Quaternion.LookRotation(-dir);
                CurrentTarget = hit.collider.GetComponent<Model>();
            }
        }
    }

    public Vector3 Forward(float dis)
    {
        return transform.position + transform.forward * dis;
    }

    public Vector3 Deflection(float angle)
    {
        return transform.eulerAngles - transform.up * angle;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + VerticalNode.forward * MaxInteractionDistance);
    }
}
