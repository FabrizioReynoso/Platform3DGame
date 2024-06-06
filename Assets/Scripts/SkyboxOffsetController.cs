using UnityEngine;

public class SkyboxOffsetController : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.1f; // Velocidad de desplazamiento del offset

    Material skyboxMaterial;

    void Start()
    {
        // Obtiene el material del skybox del RenderSettings
        skyboxMaterial = RenderSettings.skybox;

        // Si no hay material de skybox, se desactiva el script
        if (skyboxMaterial == null)
        {
            Debug.LogWarning("No se encontro material de skybox en RenderSettings.");
            enabled = false;
        }
    }

    void Update()
    {
        // Calcula el offset basado en el tiempo y la velocidad de desplazamiento
        float offset = Time.time * scrollSpeed;

        // Actualiza el offset en el material del skybox
        skyboxMaterial.SetFloat("_Rotation", offset);
    }
}