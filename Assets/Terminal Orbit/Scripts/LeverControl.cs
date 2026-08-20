using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class LeverControl : MonoBehaviour
{
    public XRGrabInteractable interactable;  // The XRGrabInteractable component

    public Transform laserTransform;  // The transform of the laser
    public Transform leverHandle;

    public float maxAngle = 45f;  // Max rotation angle the lever can rotate
    public float rotationSpeed = 2.5f;  // Speed of the rotation
    public float laserRotationSpeed = 3f;  // Speed of laser rotation
    //public Vector3 rotationAxis = Vector3.right; // Default: lever moves up/down

    private Quaternion initialRotation; 
    //private Quaternion currentRotation;

    Vector3 initialForward;

    private void Start()
    {
        if (interactable == null || leverHandle == null)
        {
            Debug.LogError("LeverControl: Missing reference(s)!");
            return;
        }

        //initialRotation = interactable.transform.localRotation;
        initialRotation = leverHandle.localRotation;
        initialForward = leverHandle.forward;
        //interactable = GetComponent<XRGrabInteractable>();

        // Event Listeners
        interactable.selectExited.AddListener(OnReleased);
    }

    private void Update()
    {

        // Update only when grabbed
        if (interactable.isSelected)
        {
            // Calculate signed angles from lever's local movement

            //Vector3 localForward = interactable.transform.localRotation * Vector3.forward;

            //float angleX = Vector3.SignedAngle(Vector3.forward, localForward, Vector3.right);   // Up/down
            //float angleZ = Vector3.SignedAngle(Vector3.forward, localForward, Vector3.up);      // Left/right


            // Calculate signed angles based on world rotation of the parent GameObject
            Vector3 worldForward = leverHandle.forward;  // Forward direction in world space
            Vector3 worldRight = leverHandle.right;  // Right direction in world space
            Vector3 worldUp = leverHandle.up;  // Up direction in world space

            // Note Forward Last Z option

            float angleX = Vector3.SignedAngle(initialForward, worldForward, worldRight);  // Up/down
            float angleZ = Vector3.SignedAngle(initialForward, worldForward, worldUp);  // Left/right


            // Clamp angles
            angleX = -Mathf.Clamp(angleX, -maxAngle, maxAngle);
            angleZ = Mathf.Clamp(angleZ, -maxAngle, maxAngle);

            leverHandle.localRotation = Quaternion.Euler(angleX, 0, angleZ);

            // Rotate the laser based on the lever angle
            if (laserTransform != null)
            {
                //laserTransform.localRotation = Quaternion.Euler(angleX, 0, angleZ);

                Quaternion targetLaserRotation = Quaternion.AngleAxis(angleX, Vector3.right) * Quaternion.AngleAxis(angleZ, Vector3.up);
                //Quaternion targetLaserRotation = Quaternion.Euler(angleX, 0, angleZ);
                //laserTransform.localRotation = Quaternion.Slerp(laserTransform.localRotation, targetLaserRotation, Time.deltaTime * laserRotationSpeed);

                laserTransform.localRotation = Quaternion.RotateTowards(laserTransform.localRotation, targetLaserRotation, laserRotationSpeed * Time.deltaTime);

            }

            Debug.Log("Laser: Lever is grabbed and moving...");
            Debug.Log("Laser: Lever Angle X = ");
            Debug.Log($"Laser: Lever Angle X = {angleX} Z = {angleZ}");

            Debug.DrawRay(leverHandle.position, leverHandle.forward * 0.5f, Color.green);
            Debug.DrawRay(leverHandle.position, leverHandle.up * 0.5f, Color.blue);
            Debug.DrawRay(leverHandle.position, leverHandle.right * 0.5f, Color.red);

        }
    }
    private void OnReleased(SelectExitEventArgs args)
    {
        // Reset the lever to its initial rotation
        //if (Quaternion.Angle(interactable.transform.localRotation, initialRotation) > 1f)
        //{
        //    StartCoroutine(ResetLever());
        //}

        //if (Quaternion.Angle(transform.parent.localRotation, initialRotation) > 1f)
        //{
        //    StartCoroutine(ResetLever());
        //}

        // Reset the lever to its initial rotation when released
        if (!Mathf.Approximately(Quaternion.Angle(leverHandle.localRotation, initialRotation), 0f))
        {
            StartCoroutine(ResetLever());
        }

    }

    private System.Collections.IEnumerator ResetLever()
    {
        //Quaternion startRotation = interactable.transform.localRotation;

        Quaternion startRotation = leverHandle.localRotation;

        Quaternion endRotation = initialRotation;

        float time = 0;

        while (time < 1)
        {
            time += Time.deltaTime * rotationSpeed;
            //interactable.transform.localRotation = Quaternion.Lerp(startRotation, initialRotation, time);

            leverHandle.localRotation = Quaternion.Lerp(startRotation, initialRotation, time);
            yield return null;
        }

        //interactable.transform.localRotation = initialRotation;

        leverHandle.localRotation = initialRotation;
    }

    //private void OnDrawGizmosSelected()
    //{
    //    if (leverHandle != null)
    //    {
    //        Gizmos.color = Color.green;
    //        Vector3 origin = leverHandle.position;
    //        Vector3 axis = rotationAxis.normalized;
    //        Vector3 forward = leverHandle.forward;

    //        Vector3 start = Quaternion.AngleAxis(-maxAngle, axis) * forward;
    //        Vector3 end = Quaternion.AngleAxis(maxAngle, axis) * forward;

    //        Gizmos.DrawLine(origin, origin + start * 1f); // Make these longer
    //        Gizmos.DrawLine(origin, origin + end * 1f);
    //        Gizmos.DrawWireSphere(origin, 0.05f); // Make the sphere more noticeable

    //        // Optional: Draw arc dots
    //        for (float a = -maxAngle; a <= maxAngle; a += 5f)
    //        {
    //            Vector3 dir = Quaternion.AngleAxis(a, axis) * forward;
    //            Gizmos.DrawSphere(origin + dir * 0.5f, 0.015f);
    //        }
    //    }
    //}


}
