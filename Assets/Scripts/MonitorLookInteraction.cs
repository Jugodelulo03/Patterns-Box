using UnityEngine;

public class MonitorLookInteraction : MonoBehaviour
{
    public LayerMask uiLayer;
    public float detectionDistance = 10f;

    public Camera playerCamera;
    public Camera monitorCamera;

    private bool uiActive = false;

    void Start()
    {
        // Asegurarse de que al inicio solo la cámara del jugador esté activa
        if (playerCamera) playerCamera.enabled = true;
        if (monitorCamera) monitorCamera.enabled = false;
    }

    void Update()
    {
        if (uiActive) return;

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, detectionDistance, uiLayer))
        {
            //Debug.Log("Detectado monitor: " + hit.collider.gameObject.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

            if (Input.GetKeyDown(KeyCode.E))
            {
                ActivateUIInteraction();
            }
        }
    }

    void ActivateUIInteraction()
    {
        uiActive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Cambiar cámaras
        if (playerCamera) playerCamera.enabled = false;
        if (monitorCamera) monitorCamera.enabled = true;
    }

    public void ExitUIInteraction()
    {
        uiActive = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Restaurar cámaras
        if (playerCamera) playerCamera.enabled = true;
        if (monitorCamera) monitorCamera.enabled = false;
    }
}

