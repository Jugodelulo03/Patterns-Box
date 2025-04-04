using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public int numeroNivel = 1;

    private int monitoresCorrectos = 0;
    private int monitoresIncorrectos = 0;
    private float tiempoTotalNivel = 0f;
    private List<float> tiemposPorMonitor = new List<float>();
    private int erroresConsecutivos = 0;
    private int maxErroresConsecutivos = 0;
    private Dictionary<string, int> patronesFallados = new Dictionary<string, int>();

    private float tiempoInicioMonitor;

    public int GetMonitoresCorrectos() => monitoresCorrectos;
    public int GetMonitoresIncorrectos() => monitoresIncorrectos;
    public int GetTotalRevisados() => monitoresCorrectos + monitoresIncorrectos;
    public float GetTiempoPromedioPorMonitor() => tiemposPorMonitor.Count > 0 ? tiempoTotalNivel / tiemposPorMonitor.Count : 0f;
    public float GetPorcentajeAciertos() => GetTotalRevisados() > 0 ? (monitoresCorrectos * 100f) / GetTotalRevisados() : 0f;
    public int GetMaxErroresConsecutivos() => maxErroresConsecutivos;

    public string GetPatronMasFallado()
    {
        string patron = "";
        int max = 0;
        foreach (var entry in patronesFallados)
        {
            if (entry.Value > max)
            {
                max = entry.Value;
                patron = entry.Key;
            }
        }
        return patron;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        tiempoTotalNivel += Time.deltaTime;
    }

    public void IniciarTiempoMonitor()
    {
        tiempoInicioMonitor = Time.time;
        //Debug.Log("Tiempo de resolución iniciado en: " + tiempoInicioMonitor);
    }

    public void RegistrarMonitor(bool esCorrecto, string patronEnganoso)
    {
        float tiempoResolucion = Time.time - tiempoInicioMonitor;
        tiemposPorMonitor.Add(tiempoResolucion);

        //Debug.Log("Tiempo de resolución del monitor: " + tiempoResolucion + " segundos");

        if (esCorrecto)
        {
            monitoresCorrectos++;
            erroresConsecutivos = 0;
            //Debug.Log("Monitor correcto. Total: " + monitoresCorrectos);
        }
        else
        {
            monitoresIncorrectos++;
            erroresConsecutivos++;
            //Debug.Log("Monitor incorrecto. Total: " + monitoresIncorrectos);

            if (erroresConsecutivos > maxErroresConsecutivos)
            {
                maxErroresConsecutivos = erroresConsecutivos;
            }

            if (!patronesFallados.ContainsKey(patronEnganoso))
            {
                patronesFallados[patronEnganoso] = 0;
            }
            patronesFallados[patronEnganoso]++;

            //Debug.Log("Patrón engañoso fallado: " + patronEnganoso + ", Total fallos: " + patronesFallados[patronEnganoso]);
        }
    }

    public void EnviarEstadisticasAFirebase()
    {
        float promedioResolucion = tiemposPorMonitor.Count > 0 ? tiempoTotalNivel / tiemposPorMonitor.Count : 0f;
        float porcentajeAciertos = (monitoresCorrectos + monitoresIncorrectos) > 0 ? (monitoresCorrectos * 100f) / (monitoresCorrectos + monitoresIncorrectos) : 0f;

        string patronMasFallado = "";
        int maxFallas = 0;
        foreach (var entry in patronesFallados)
        {
            if (entry.Value > maxFallas)
            {
                maxFallas = entry.Value;
                patronMasFallado = entry.Key;
            }
        }

        //Debug.Log("Enviando estadísticas a Firebase...");
        //Debug.Log("Monitores correctos: " + monitoresCorrectos);
        //Debug.Log("Monitores incorrectos: " + monitoresIncorrectos);
        //Debug.Log("Tiempo total del nivel: " + tiempoTotalNivel);
        //Debug.Log("Tiempo promedio por monitor: " + promedioResolucion);
        //Debug.Log("Porcentaje de aciertos: " + porcentajeAciertos);
        //Debug.Log("Errores consecutivos máximos: " + maxErroresConsecutivos);
        //Debug.Log("Patrón más fallado: " + patronMasFallado);

        DatabaseReference dbRef = FirebaseDatabase.DefaultInstance.GetReference("estadisticas");
        string userId = System.Guid.NewGuid().ToString();

        Dictionary<string, object> datos = new Dictionary<string, object>
        {
            { "nivel", numeroNivel },
            { "totalMonitoresRevisados", monitoresCorrectos + monitoresIncorrectos },
            { "monitoresCorrectos", monitoresCorrectos },
            { "monitoresIncorrectos", monitoresIncorrectos },
            { "porcentajeAciertos", porcentajeAciertos.ToString("F2") },
            { "tiempoPromedioResolucion", promedioResolucion.ToString("F1") },
            { "tiempoTotalNivel", tiempoTotalNivel.ToString("F1") },
            { "patronMasFallado", patronMasFallado },
            { "maxErroresConsecutivos", maxErroresConsecutivos }
        };

        dbRef.Child(userId).SetValueAsync(datos).ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("Estadísticas enviadas correctamente a Firebase.");
            }
            else
            {
                Debug.LogError("Error al enviar estadísticas a Firebase: " + task.Exception);
            }
        });
    }
}