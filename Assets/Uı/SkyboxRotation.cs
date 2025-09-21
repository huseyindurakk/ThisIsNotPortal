using UnityEngine;

public class SkyboxRotation : MonoBehaviour
{
    public float rotationSpeed = 1.0f; // Skybox'un dönüþ hýzý

    void Update()
    {
        // Skybox'un "_Rotation" özelliðini döndür
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotationSpeed);
    }
}
