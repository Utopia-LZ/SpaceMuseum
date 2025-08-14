using UnityEngine;

public class EarthRotation : MonoBehaviour
{
    [Tooltip("地球自转速度（度/秒）")]
    public float rotationSpeed = 15.0f; // 默认速度15度/秒

    void Update()
    {
        // 绕自身Y轴（北极指向方向）旋转
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
    }
}