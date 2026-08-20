using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Fragment : MonoBehaviour
{

    private Transform snapTarget;  // The correct position for the fragment
    private XRGrabInteractable interactable;  // Reference to Interactable Object
    private Rigidbody body;

    private bool puzzleCompleted = false;  // Flag Check if Map Puzzle is completed 

    public GameObject ufoImage;  // Assign the UFO Image 

    private void Start()
    {
        // FInd the correct position for the fragment
        snapTarget = GameObject.Find(gameObject.name + "_SnapTarget")?.transform;  // Specific Snapping 
        //snapTarget = GameObject.Find("SnapTarget")?.transform;  // General Snapping 

        if (snapTarget == null)
        {
            Debug.LogError("Snap Target not found for " + gameObject.name);
        }

        interactable = GetComponent<XRGrabInteractable>();
        body = GetComponent<Rigidbody>();
        
        // Event Listeners
        interactable.selectEntered.AddListener(PickUp);
        interactable.selectExited.AddListener(Drop);

    }

    public void PickUp(SelectEnterEventArgs args)
    {
        body.isKinematic = false; // Allow Movement while holding object
    }


    public void Drop(SelectExitEventArgs args)
    {
        
        if (snapTarget != null)
        {
            float distance = Vector3.Distance(transform.position, snapTarget.position);
            Debug.Log("Distance to Snap Target: " + distance);

            if (distance < 1.5f)
            {
                SnapToPlace();
            }

        }

        else
        {
            Debug.LogWarning("Snap Target not found!");
        }

    }

    private void SnapToPlace()
    {
        // Snap Fragment to Snap Target
        transform.position = snapTarget.position;
        transform.rotation = snapTarget.rotation;

        body.isKinematic = true; // Stops physics movement
        interactable.enabled = false;  // Disable grabbing after placing

        Debug.Log("Fragment Snapped in place!");

        // Check if all fragments are placed
        CheckAllFragmentsPlaced();

    }

    private void CheckAllFragmentsPlaced()
    {
        
        if (puzzleCompleted) return;
        
        Fragment[] allFragments = FindObjectsByType<Fragment>(FindObjectsSortMode.None);

        foreach (Fragment fragment in allFragments)
        {
            XRGrabInteractable interactable = fragment.GetComponent<XRGrabInteractable>();
            if (interactable != null && interactable.enabled)
            {
                // If any fragment is still grabbable, puzzle is not complete
                return;
            }
        }

        // Mark Map Puzzle as completed 
        puzzleCompleted = true;  

        // If all fragments are placed, unlock the door
        Debug.Log("All Fragments Placed! First Puzzle Done...");
        FindAnyObjectByType<DoorController>()?.SetMapPuzzle();

        Laser laser = FindAnyObjectByType<Laser>();
        if (laser != null)
        {
            laser.isLaserActive = true;
            laser.ActivateLaser();
        }

        ShowUFOClue();
    }

    private void ShowUFOClue()
    {
        if (ufoImage != null && !ufoImage.activeSelf)  // Check if UFO Image is already active
        {
            ufoImage.SetActive(true);  // Make UFO Image visible
            Debug.Log("UFO Clue Revealed!");
        }
        else
        {
            Debug.Log("UFO Image not assigned");
        }
    }

}
