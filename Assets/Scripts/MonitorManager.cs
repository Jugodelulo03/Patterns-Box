using UnityEngine;
using System.Collections;

public class MonitorManager : MonoBehaviour
{
    public Transform entryPoint;     // Punto de entrada (inicio cinta)
    public Transform centerPoint;    // Punto central (zona interactiva)
    public Transform exitPoint;      // Punto de salida (fin cinta)

    public GameObject[] monitors;
    private int currentMonitorIndex = 0;

    [Header("Monitor Actual")]
    public Monitor monitorActual;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    void Start()
    {
        // Desactivar todos excepto el primero
        for (int i = 0; i < monitors.Length; i++)
        {
            monitors[i].SetActive(i == 0);
            if (i == 0)
            {
                monitors[i].transform.position = entryPoint.position;
                StartCoroutine(MoveMonitorSmooth(monitors[i], centerPoint.position));
            }
        }
    }

    // Llamado desde los botones
    public void EvaluarRespuesta(PatronEnganoso patronSeleccionado)
    {
        if (monitorActual == null) return;

        if (patronSeleccionado == monitorActual.patronAsignado)
        {
            Debug.Log("¡Respuesta Correcta!");
            if (audioSource && sonidoCorrecto) audioSource.PlayOneShot(sonidoCorrecto);
        }
        else
        {
            Debug.Log("Respuesta Incorrecta");
            if (audioSource && sonidoIncorrecto) audioSource.PlayOneShot(sonidoIncorrecto);
        }

        MoveToNextMonitor();
    }

    public void MoveToNextMonitor()
    {
        if (currentMonitorIndex < monitors.Length)
        {
            // Mover monitor actual a la salida
            GameObject oldMonitor = monitors[currentMonitorIndex];
            StartCoroutine(MoveMonitorSmooth(oldMonitor, exitPoint.position));
            StartCoroutine(DeactivateAfterDelay(oldMonitor, 1f));

            currentMonitorIndex++;

            // Activar y mover el siguiente
            if (currentMonitorIndex < monitors.Length)
            {
                GameObject newMonitor = monitors[currentMonitorIndex];
                newMonitor.SetActive(true);
                newMonitor.transform.position = entryPoint.position;
                StartCoroutine(MoveMonitorSmooth(newMonitor, centerPoint.position, 1f, true));
            }
            else
            {
                monitorActual = null; // No hay más monitores
            }
        }
    }

    IEnumerator MoveMonitorSmooth(GameObject monitor, Vector3 targetPos, float duration = 1f, bool setAsCurrent = false)
    {
        float time = 0f;
        Vector3 startPos = monitor.transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            monitor.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        monitor.transform.position = targetPos;

        // ✅ Si es el movimiento hacia el centro, lo marcamos como monitor actual
        if (setAsCurrent)
        {
            monitorActual = monitor.GetComponent<Monitor>();
        }
    }

    IEnumerator DeactivateAfterDelay(GameObject monitor, float delay)
    {
        yield return new WaitForSeconds(delay);
        monitor.SetActive(false);
    }
}
