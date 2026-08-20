using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Movement;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class FadeOutCanvas : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;
    public GameObject completionUI; // Your "room complete" message
    public float messageDuration = 15f; // How long to show before quitting
    public bool quitAfterMessage = true;

    public GameObject playerRig; // Assign your XR Rig root here

    public void StartFade()
    {
        Debug.Log("FadeOutCanvas: StartFade() called.");
        StartCoroutine(FadeToBlack());
    }

    private IEnumerator FadeToBlack()
    {
        float t = 0;
        Color color = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
        Debug.Log("FadeOutCanvas: Fade complete. Room experience is done.");

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);
        Debug.Log("FadeOutCanvas: Fade complete.");

        if (completionUI != null)
        {
            fadeImage.gameObject.SetActive(false); 
            completionUI.SetActive(true);

            if (playerRig != null)
            {
                var moveProvider = playerRig.GetComponent<ContinuousMoveProvider>();
                if (moveProvider != null)
                {
                    moveProvider.enabled = false; // disables movement input
                    Debug.Log("FadeOutCanvas: Player movement disabled.");
                }
            }

        }

        //if (quitAfterMessage)
        //{
        //    yield return new WaitForSeconds(messageDuration);
        //    Debug.Log("FadeOutCanvas: Quitting application...");
        //    Application.Quit();

        //    #if UNITY_EDITOR
        //    EditorApplication.isPlaying = false;
        //    #endif
        //}

        float timer = 0f;

        Debug.Log("FadeOutCanvas: Starting message timer...");

        while (timer < messageDuration)
        {
            timer += Time.deltaTime;
            Debug.Log($"[FadeOutCanvas] Time elapsed: {timer:F2}s / {messageDuration}s");
            yield return null;
        }
        
        Debug.Log("FadeOutCanvas: Time complete. Quitting application...");

        Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif


    }

}
