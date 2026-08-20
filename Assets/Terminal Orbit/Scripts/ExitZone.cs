using UnityEngine;

public class ExitZone : MonoBehaviour
{
    public string playerTag = "Player"; // Tag on XR Rig or Camera
    //public GameObject messageUI; // Optional
    public FadeOutCanvas fadeCanvas; // Reference to your fade script

    private bool hasExited = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasExited) return; // Prevent duplicate triggers

        if (other.CompareTag(playerTag))
        {
            hasExited = true;

            Debug.Log("ExitZone: Player exited the VR room!");

            //if (messageUI != null)
            //    messageUI.SetActive(true);

            if (fadeCanvas != null)
            {
                fadeCanvas.StartFade();
            }
            else
            {
                Debug.LogWarning("ExitZone: No FadeOutCanvas assigned.");
            }

        }
    }
}
