using System.Collections;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public LineRenderer lineRenderer;  // Reference to the LineRenderer component
    public float maxDistance = 50f;  // Max distance the laser can reach

    public Material normalMaterial;  // Default laser material
    public Material highlightMaterial;  // Glowing laser when hitting Leo

    private bool isHitting = false;  // Flag check if laser is hitting Leo
    private bool isLaserLocked = false;  // Flag check if laser is locked
    public bool isLaserActive = false;  // Flag Check if Laser is on

    private Renderer leoRenderer;  // Reference to Leo
    private Color originalColour;  // Store original colour
    private Vector3 lockedDirection; // Store locked direction 

    //private void Start()
    //{
    //    lineRenderer.enabled = false;  // Ensure laser starts off
    //}

    private void Update()
    {
        if (!isLaserActive || lineRenderer == null) return;  // Laser does nothing if not active or found

        Vector3 endPoint;

        if (isLaserLocked)
        {
            // If locked, maintain the last locked direction
            endPoint = transform.position + lockedDirection * maxDistance;
        }
        else
        {
            // Normal behavior, update laser direction dynamically
            RaycastHit hit;
            endPoint = transform.position + transform.forward * maxDistance;

            bool hitLeo = false;
            if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance))
            {
                endPoint = hit.point;  // Stop at first hit
                if (hit.collider.gameObject.name == "LeoTarget")
                {
                    hitLeo = true;
                    if (!isHitting) ActivateLeoEffect(hit.collider.gameObject);
                }
            }

            if (!hitLeo && isHitting)
            {
                DeactivateLeoEffect();
            }

            if (hitLeo != isHitting)
            {
                isHitting = hitLeo;
                if (normalMaterial != null && highlightMaterial != null)
                {
                    lineRenderer.material = isHitting ? highlightMaterial : normalMaterial;
                }

                if (isHitting)
                {
                    StartCoroutine(LockLaserAfterDelay());  // Start countdown to lock laser
                }
            }
        }

        // Update the Line Renderer
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint);
    }

    private void ActivateLeoEffect(GameObject leo)
    {
        if (leoRenderer == null)
        {
            leoRenderer = leo.GetComponent<Renderer>();
            if (leoRenderer != null)
            {
                originalColour = leoRenderer.material.color;  // Store the original color
            }
        }

        if (leoRenderer != null)
        {
            leoRenderer.material.color = Color.yellow;  // Laser turns Yellow when hitting Leo
        }
    }

    private void DeactivateLeoEffect()
    {
        if (leoRenderer != null)
        {
            leoRenderer.material.color = originalColour;  // Reset Leo Colour
        }
    }

    public void ActivateLaser()
    {
        isLaserActive = true;
        lineRenderer.enabled = true; // Show the laser
        Debug.Log("Laser Activated!");
    }

    private IEnumerator LockLaserAfterDelay()
    {
        float timer = 0f;
        Vector3 finalDirection = transform.forward;

        Debug.Log("[Laser] Starting lock timer...");

        while (timer < 3f)
        {
            timer += Time.deltaTime;
            Debug.Log($"[Laser] Time elapsed: {timer:F2}s / 3s");
            yield return null;
        }

        if (isHitting)
        {
            isLaserLocked = true;
            lockedDirection = finalDirection;
            Debug.Log("[Laser] Laser locked after 5 seconds.");

            FindAnyObjectByType<DoorController>()?.SetStarPuzzle();
        }
        else
        {
            Debug.Log("[Laser] Laser did not remain hitting during the full timer.");
        }
    }

}
