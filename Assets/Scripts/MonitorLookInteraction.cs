using UnityEngine;

public class MonitorLookInteraction : MonoBehaviour
{
    public LayerMask uiLayer;
    public float detectionDistance = 10f;

    public Camera playerCamera;
    public Camera monitorCamera;

    private bool uiActive = false;

    public MonoBehaviour playerController; // Asigna el script de MFPC en el Inspector
    public MonoBehaviour playerController2; // Asigna el script de MFPC en el Inspector


    void Start()
    {
        // Asegurarse de que al inicio solo la cámara del jugador esté activa
        if (playerCamera) playerCamera.enabled = true;
        if (monitorCamera) monitorCamera.enabled = false;
    }

    void Update()
    {
        if (uiActive) return;

        if (playerCamera != null && PauseMenu.GameIsPaused == false)
        {
            
            Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, detectionDistance, uiLayer))
            {
                //Debug.Log("Detectado monitor: " + hit.collider.gameObject.name + " | Layer: " + LayerMask.LayerToName(hit.collider.gameObject.layer));

                if (Input.GetMouseButtonDown(0))
                {
                    ActivateUIInteraction();
                }
            }
        }
    }

    void ActivateUIInteraction()
    {
        PauseMenu.IsInteracting = true;
        uiActive = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerController.enabled = false; // Deshabilita el movimiento
        playerController2.enabled = false; // Deshabilita el movimiento

        // Cambiar cámaras
        if (playerCamera) playerCamera.enabled = false;
        if (monitorCamera) monitorCamera.enabled = true;
    }

    public void ExitUIInteraction()
    {
        uiActive = false;
        PauseMenu.IsInteracting = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playerController.enabled = true; // Deshabilita el movimiento
        playerController2.enabled = true; // Deshabilita el movimiento

        // Restaurar cámaras
        if (playerCamera) playerCamera.enabled = true;
        if (monitorCamera) monitorCamera.enabled = false;
    }
}

