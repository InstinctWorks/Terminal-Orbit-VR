using UnityEngine;

public class ResetFallenObjects : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fragment"))
        {
            other.transform.position = new Vector3(0, 2, 0);  // Reset to safe position
            other.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;  
        }
    }
}
