using UnityEngine;

public class SnapTarget : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;  // Green Outline for Snap Targets
        Gizmos.DrawSphere(transform.position, 0.2f);  
    }
}
