using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Skybox'un d�n�� h�z�

    void Update()
    {
        // Skybox'un "_Rotation" �zelli�ini d�nd�r
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
