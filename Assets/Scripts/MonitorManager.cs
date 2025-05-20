using UnityEngine;
using System.Collections;

public class MonitorManager : MonoBehaviour
{
    [Header("AnimadorMesa")]
    public Animator anim;

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

    [Header("Botones")]
    public GameObject[] boton;
    public GameObject botonRED;

    void Start()
    {
        firebaseTextLoader = FindObjectOfType<FirebaseTextLoader>();
        StartCoroutine(CrearNuevoMonitorAsync(true));
    }

    public void EvaluarRespuesta(PatronEnganoso patronSeleccionado)
    {
        if (!monitorListo || monitorActual == null) return;

        int valorPatron = ObtenerValorPatron(patronSeleccionado);

        if (valorPatron == 7)
        {
            anim.SetInteger("DeskState", 1);
        }
        else
        {
            anim.SetInteger("BtnPress", valorPatron);
        }

        bool esCorrecto = patronSeleccionado == monitorActual.patronAsignado;
        string patronNombre = monitorActual.patronAsignado.ToString();

        if (esCorrecto)
        {
            Debug.Log("¡Respuesta Correcta!");
            if (audioSource && sonidoCorrecto) audioSource.PlayOneShot(sonidoCorrecto);
            uiManager?.SumarMonitorCorrecto();
        }
        else
        {
            Debug.Log("Respuesta Incorrecta");
            faxHojaSpawner?.DispararHojaError();
            if (audioSource && sonidoIncorrecto) audioSource.PlayOneShot(sonidoIncorrecto);
            uiManager?.SumarError(patronNombre);
        }

        // Registrar estadística
        //GameStatsManager.Instance.RegistrarMonitor(esCorrecto, patronNombre);

        MoverMonitorActualYCrearOtro(esCorrecto);

        foreach (GameObject b in boton)
        {
            b.layer = LayerMask.NameToLayer("Interactable");
        }
        botonRED.layer = LayerMask.NameToLayer("Interactable");
    }

    private int ObtenerValorPatron(PatronEnganoso patron)
    {
        switch (patron)
        {
            case PatronEnganoso.PresionPsicologica: return 1;
            case PatronEnganoso.EnganoVisual: return 2;
            case PatronEnganoso.RoboAtencion: return 3;
            case PatronEnganoso.RecoleccionAbusivaDatos: return 4;
            case PatronEnganoso.ObstruccionFriccion: return 5;
            case PatronEnganoso.FalsasPromesas: return 6;
            case PatronEnganoso.Ninguno: return 7;
            default: return 0;
        }
    }

    void MoverMonitorActualYCrearOtro(bool isCorrect)
    {
        if (monitorActual != null)
        {
            StartCoroutine(MoverYDesactivar(monitorActual.gameObject, exitPoint.position, 1));
        }
        StartCoroutine(CrearNuevoMonitorAsync(isCorrect));
    }

    IEnumerator CrearNuevoMonitorAsync(bool isCorrect)
    {
        monitorListo = false;

        // Iniciar tiempo para el nuevo monitor
        GameStatsManager.Instance.IniciarTiempoMonitor();

        GameObject prefab = monitoresEstilos[Random.Range(0, monitoresEstilos.Length)];
        GameObject nuevoMonitor = Instantiate(prefab, entryPoint.position, Quaternion.identity);
        nuevoMonitor.SetActive(true);

        Monitor monitorScript = nuevoMonitor.GetComponent<Monitor>();
        PatronEnganoso patron = monitorScript.patronAsignado;
        string variante = monitorScript.varianteSeleccionada;

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
            //Debug.LogError("No se pudieron cargar textos para el patrón: " + patron + ", variante: " + variante);
        }

        monitorActual = monitorScript;
        if (!isCorrect)
        {
            yield return new WaitForSeconds(2f);
        }
        anim.SetInteger("BtnPress", 0);
        yield return StartCoroutine(MoverSuavemente(nuevoMonitor, centerPoint.position, 1));
        monitorListo = true;

    }

    IEnumerator MoverSuavemente(GameObject monitor, Vector3 destino, float duracion)
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
        yield return new WaitForSeconds(0.8f);
        Destroy(monitor);
    }
}
