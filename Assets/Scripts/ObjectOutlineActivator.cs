using UnityEngine;

public class ObjectOutlineActivator : MonoBehaviour
{
    public LayerMask outlineLayer;  // Define la capa que activará el Outline
    public float detectionDistance = 10f; // Distancia del Raycast

    private Outline lastOutline; // Último objeto resaltado

    void Update()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Raycast desde el centro de la pantalla
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance, outlineLayer))
        {
            Outline outline = hit.collider.GetComponent<Outline>();

            if (outline != null)
            {
                if (lastOutline && lastOutline != outline)
                {
                    lastOutline.enabled = false; // Desactivar el último objeto resaltado
                }

                outline.enabled = true; // Activar el nuevo objeto
                lastOutline = outline;
            }
        }
        else if (lastOutline)
        {
            lastOutline.enabled = false; // Desactivar el último objeto si no miramos ninguno
            lastOutline = null;
        }
    }
}
