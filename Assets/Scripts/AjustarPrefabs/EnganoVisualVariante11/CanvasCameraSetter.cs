using UnityEngine;

public class CanvasCameraSetter : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        Camera camPantalla = GameObject.Find("CameraPantalla")?.GetComponent<Camera>();

        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            if (camPantalla != null)
            {
                canvas.worldCamera = camPantalla;
                Debug.Log("Canvas asignado a CameraPantalla correctamente en: " + gameObject.name);
            }
            else
            {
                Debug.LogWarning("No se encontró una cámara llamada 'CameraPantalla'");
            }
        }
    }
}
