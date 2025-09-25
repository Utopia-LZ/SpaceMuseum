using UnityEngine;

public class Test : MonoBehaviour
{
    public LayerMask Layer;
    // Update is called once per frame
    void Update()
    {
        if(Physics.OverlapSphere(transform.position, 0.5f,Layer,QueryTriggerInteraction.Collide).Length > 0 )
        {
            Debug.Log("Hit something");
        }
    }
}
