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
    public AudioClip sonidoMovimientoMonitor;

    [Header("Fax")]
    public FaxHojaSpawner faxHojaSpawner;

    private FirebaseTextLoader firebaseTextLoader;
    private bool monitorListo = false;

    public UIManager uiManager;
    void Start()
    {
        firebaseTextLoader = FindObjectOfType<FirebaseTextLoader>();
        StartCoroutine(CrearNuevoMonitorAsync());
    }


    public void EvaluarRespuesta(PatronEnganoso patronSeleccionado)
    {
        if (!monitorListo || monitorActual == null) return;

        if (patronSeleccionado == monitorActual.patronAsignado)
        {
            Debug.Log("¡Respuesta Correcta!");
            if (audioSource && sonidoCorrecto) audioSource.PlayOneShot(sonidoCorrecto);

            if (uiManager != null)
            {
                uiManager.SumarMonitorCorrecto(); // ← Suma al contador
            }
        }
        else
        {
            Debug.Log("Respuesta Incorrecta");
            if (audioSource && sonidoIncorrecto) audioSource.PlayOneShot(sonidoIncorrecto);
            if (faxHojaSpawner != null)
            {
                faxHojaSpawner.DispararHojaError();
            }
            uiManager.SumarError();
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
        monitorListo = false;

        // Elegir prefab aleatorio
        GameObject prefab = monitoresEstilos[Random.Range(0, monitoresEstilos.Length)];
        GameObject nuevoMonitor = Instantiate(prefab, entryPoint.position, Quaternion.identity);
        nuevoMonitor.SetActive(true);

        Monitor monitorScript = nuevoMonitor.GetComponent<Monitor>();
        PatronEnganoso patron = monitorScript.patronAsignado;
        string variante = monitorScript.varianteSeleccionada;

        // Obtener textos desde Firebase según patrón y variante seleccionada
        var textoTask = firebaseTextLoader.GetRandomSubvarianteFor(patron, variante);
        while (!textoTask.IsCompleted)
            yield return null;

        TextoMonitor textos = textoTask.Result;

        if (textos != null)
        {
            monitorScript.SetTextos(textos);
        }
        else
        {
            Debug.LogError("No se pudieron cargar textos para el patrón: " + patron + ", variante: " + variante);
        }

        monitorActual = monitorScript;
        yield return StartCoroutine(MoverSuavemente(nuevoMonitor, centerPoint.position));
        monitorListo = true;
    }

    IEnumerator MoverSuavemente(GameObject monitor, Vector3 destino, float duracion = 1f)
    {
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
