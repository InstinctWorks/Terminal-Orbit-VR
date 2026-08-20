using System.Collections;
using System.Drawing;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public GameObject leftDoor;
    public GameObject rightDoor;
    private bool mapPuzzle = false;  // Flag Check if map puzzle is complete
    private bool starPuzzle = false;  // Flag Check if star puzzle is complete

    public float openDistance = 2f;
    public float openDuration = 20f; // How long the door takes to open

    public void SetMapPuzzle()
    {
        mapPuzzle = true;
        CheckUnlock();
        Debug.Log("DoorController: Map Puzzle Completed!");
    }

    public void SetStarPuzzle()
    {
        starPuzzle = true;
        CheckUnlock();
        Debug.Log("DoorController: Star Puzzle Completed!");
    }

    private void CheckUnlock()
    {
        if (mapPuzzle && starPuzzle)
        {
            Debug.Log("DoorController: Map and Star Puzzle completed! Unlocking Door...");
            //OpenDoor();
            StartCoroutine(OpenDoors());
        }
    }

    private void OpenDoor()
    {
        Debug.Log("DoorController: Door Unlocked!");
        leftDoor.transform.position += new Vector3(-12, 0, 0);
        rightDoor.transform.position += new Vector3(12, 0, 0);

    }

    private IEnumerator OpenDoors()
    {
        Debug.Log("DoorController: Door Unlocking...");

        Vector3 leftStart = leftDoor.transform.position;
        Vector3 leftEnd = leftStart + new Vector3(-openDistance, 0, 0);

        Vector3 rightStart = rightDoor.transform.position;
        Vector3 rightEnd = rightStart + new Vector3(openDistance, 0, 0);

        float timeElapsed = 0f;
        float count = 0f;

        while (count < 3f)
        {
            count += Time.deltaTime;
            Debug.Log($"[DoorController] Time elapsed: {count:F2}s / 3s");
            yield return null;
        }

        while (timeElapsed < openDuration)
        {
            timeElapsed += Time.deltaTime;
            float t = timeElapsed / openDuration;

            leftDoor.transform.position = Vector3.Lerp(leftStart, leftEnd, t);
            rightDoor.transform.position = Vector3.Lerp(rightStart, rightEnd, t);

            yield return null;
        }

        // Ensure exact final position
        leftDoor.transform.position = leftEnd;
        rightDoor.transform.position = rightEnd;

        Debug.Log("DoorController: Doors fully opened.");
    }
}
