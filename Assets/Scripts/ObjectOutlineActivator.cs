using UnityEngine;

public class ObjectOutlineActivator : MonoBehaviour
{
    public LayerMask outlineLayers;
    public float detectionDistance = 10f;
    private Outline lastOutline;

    [Header("Hint de interacción (texto flotante opcional)")]
    public GameObject hintTextObject;

    [Header("Crosshair")]
    public GameObject crosshairObject; //Crosshair como GameObject

    private bool hintVisible = false;
    public Camera raycastCamera;

    void Update()
    {
        // Si la cámara está desactivada, apagamos todo y salimos
        if (raycastCamera == null || !raycastCamera.enabled)
        {
            if (lastOutline)
            {
                lastOutline.enabled = false;
                lastOutline = null;
            }

            ShowHint(false);
            ShowCrosshair(false); //Desactiva crosshair también
            return;
        }

        ShowCrosshair(true); // Mostrar crosshair si cámara está activa

        Ray ray = raycastCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance, outlineLayers))
        {
            Outline outline = hit.collider.GetComponent<Outline>();
            if (outline != null)
            {
                if (lastOutline && lastOutline != outline)
                {
                    lastOutline.enabled = false;
                }

                outline.enabled = true;
                lastOutline = outline;
            }

            ShowHint(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                // Acción opcional
            }
        }
        else
        {
            if (lastOutline)
            {
                lastOutline.enabled = false;
                lastOutline = null;
            }

            ShowHint(false);
        }
    }

    void ShowHint(bool show)
    {
        if (hintTextObject == null) return;

        if (hintTextObject.activeSelf != show)
        {
            hintTextObject.SetActive(show);
            hintVisible = show;
        }
    }

    void ShowCrosshair(bool show)
    {
        if (crosshairObject == null) return;

        if (crosshairObject.activeSelf != show)
        {
            crosshairObject.SetActive(show);
        }
    }
}

