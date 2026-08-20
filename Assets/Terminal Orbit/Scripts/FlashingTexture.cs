using UnityEngine;
using System.Collections;

public class FlashingTexture : MonoBehaviour
{
    public Renderer quadRenderer;  // Assign the Quad Renderer
    
    public Texture texture1;  // First UFO image
    public Texture texture2;  // Second UFO image
    public float flashInterval = 0.5f;  // Time between flashes
    
    private Material quadMaterial;  // Assign the material applied to the Quad
    
    private void Start()
    {
        if (quadRenderer == null)
        {
            quadRenderer = GetComponent<Renderer>(); // Auto-assign Renderer
        }

        Debug.Log("Flashing Texture Starting...");

        quadMaterial = quadRenderer.material;
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        Debug.Log("Flash Started");

        while (true)
        {
            quadMaterial.SetTexture("_BaseMap", texture1); // Change texture
            yield return new WaitForSeconds(flashInterval);
            quadMaterial.SetTexture("_BaseMap", texture2);
            yield return new WaitForSeconds(flashInterval);
        }
    }

}
