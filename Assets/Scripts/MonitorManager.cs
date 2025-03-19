using UnityEngine;
using System.Collections;

public class MonitorManager : MonoBehaviour
{
    [Header("Puntos de Movimiento")]
    public Transform entryPoint;
    public Transform centerPoint;
    public Transform exitPoint;

    [Header("Prefabs de monitores")]
    public GameObject[] monitoresEstilos;

    [Header("Monitor Actual")]
    public Monitor monitorActual;

    [Header("Sonidos")]
    public AudioSource audioSource;
    public AudioClip sonidoCorrecto;
    public AudioClip sonidoIncorrecto;
    public AudioClip sonidoMovimientoMonitor; // <- NUEVO

    [Header("Fax")]
    public FaxHojaSpawner faxHojaSpawner;

    private FirebaseTextLoader firebaseTextLoader;

    private bool monitorListo = false; // <- NUEVA BANDERA

    void Start()
    {
        firebaseTextLoader = FindObjectOfType<FirebaseTextLoader>();
        StartCoroutine(CrearNuevoMonitorAsync());
    }

    public void EvaluarRespuesta(PatronEnganoso patronSeleccionado)
    {
        // No se puede responder si aún no terminó la animación
        if (!monitorListo || monitorActual == null) return;

        if (patronSeleccionado == monitorActual.patronAsignado)
        {
            Debug.Log("¡Respuesta Correcta!");
            if (audioSource && sonidoCorrecto) audioSource.PlayOneShot(sonidoCorrecto);
        }
        else
        {
            Debug.Log("Respuesta Incorrecta");
            if (audioSource && sonidoIncorrecto) audioSource.PlayOneShot(sonidoIncorrecto);

            if (faxHojaSpawner != null)
            {
                faxHojaSpawner.DispararHojaError();
            }
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
        monitorListo = false; // <- Bloquear interacción durante la animación

        var textoTask = firebaseTextLoader.GetRandomEnganoVisualTextAsync();

        while (!textoTask.IsCompleted)
            yield return null;

        TextoMonitor variante = textoTask.Result;

        if (variante == null)
        {
            Debug.LogError("No se pudo obtener textos desde Firebase.");
            yield break;
        }

        GameObject prefab = monitoresEstilos[Random.Range(0, monitoresEstilos.Length)];
        GameObject nuevoMonitor = Instantiate(prefab, entryPoint.position, Quaternion.identity);
        nuevoMonitor.SetActive(true);

        Monitor monitorScript = nuevoMonitor.GetComponent<Monitor>();
        monitorScript.patronAsignado = PatronEnganoso.EnganoVisual;
        monitorScript.SetTextos(variante);

        monitorActual = monitorScript;

        yield return StartCoroutine(MoverSuavemente(nuevoMonitor, centerPoint.position));

        monitorListo = true; // <- Ahora sí se puede interactuar
    }

    IEnumerator MoverSuavemente(GameObject monitor, Vector3 destino, float duracion = 1f)
    {
        // Sonido de movimiento
        if (audioSource && sonidoMovimientoMonitor)
        {
            audioSource.PlayOneShot(sonidoMovimientoMonitor);
        }

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
