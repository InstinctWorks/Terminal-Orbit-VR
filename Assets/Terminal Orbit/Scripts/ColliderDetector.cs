using UnityEngine;

public class ColliderDetector : MonoBehaviour
{
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collided with: " + hit.gameObject.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger: " + other.gameObject.name);
    }
}
