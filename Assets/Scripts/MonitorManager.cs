using UnityEngine;
using System.Collections;

public class MonitorManager : MonoBehaviour
{
    [Header("Puntos de Movimiento")]
    public Transform entryPoint;
    public Transform centerPoint;
    public Transform exitPoint;

    [Header("Prefabs de monitores")]
    public GameObject[] monitoresEstilos; // Diferentes estilos de monitor

    [Header("Monitor Actual")]
    public Monitor monitorActual;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;

    private FirebaseTextLoader firebaseTextLoader;

    void Start()
    {
        firebaseTextLoader = FindObjectOfType<FirebaseTextLoader>();
        StartCoroutine(CrearNuevoMonitorAsync()); // Llamamos a la corrutina para crear el primero
    }

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

        MoverMonitorActualYCrearOtro();
    }

    void MoverMonitorActualYCrearOtro()
    {
        if (monitorActual != null)
        {
            StartCoroutine(MoverYDesactivar(monitorActual.gameObject, exitPoint.position));
        }

        StartCoroutine(CrearNuevoMonitorAsync());
    }

    IEnumerator CrearNuevoMonitorAsync()
    {
        // 1. Esperar a que Firebase cargue los textos
        var textoTask = firebaseTextLoader.GetRandomEnganoVisualTextAsync();

        while (!textoTask.IsCompleted)
            yield return null;

        TextoMonitor variante = textoTask.Result;

        if (variante == null)
        {
            Debug.LogError("No se pudo obtener textos desde Firebase. No se creará monitor.");
            yield break;
        }

        // 2. Elegir prefab aleatorio
        GameObject prefab = monitoresEstilos[Random.Range(0, monitoresEstilos.Length)];

        // 3. Instanciar monitor
        GameObject nuevoMonitor = Instantiate(prefab, entryPoint.position, Quaternion.identity);
        nuevoMonitor.SetActive(true);

        // 4. Asignar datos del patrón
        Monitor monitorScript = nuevoMonitor.GetComponent<Monitor>();
        monitorScript.patronAsignado = PatronEnganoso.EnganoVisual;

        // 5. Asignar los textos al Monitor
        monitorScript.SetTextos(variante);

        // 6. Mover al centro
        StartCoroutine(MoverSuavemente(nuevoMonitor, centerPoint.position));

        // 7. Asignar como actual
        monitorActual = monitorScript;
    }

    IEnumerator MoverSuavemente(GameObject monitor, Vector3 destino, float duracion = 1f)
    {
        float tiempo = 0f;
        Vector3 origen = monitor.transform.position;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            monitor.transform.position = Vector3.Lerp(origen, destino, tiempo / duracion);
            yield return null;
        }

        monitor.transform.position = destino;
    }

    IEnumerator MoverYDesactivar(GameObject monitor, Vector3 destino, float duracion = 1f)
    {
        yield return StartCoroutine(MoverSuavemente(monitor, destino, duracion));
        yield return new WaitForSeconds(0.5f);
        Destroy(monitor);
    }
}

