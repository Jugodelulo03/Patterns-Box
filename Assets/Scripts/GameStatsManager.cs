using UnityEngine;
using Firebase.Database;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameStatsManager : MonoBehaviour
{
    public static GameStatsManager Instance;

    public int numeroNivel;

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

    public bool inicio = false;

    // Configuración de usuario
    public float volumen = 1f;
    public float sensibilidad = 1f;
    public int calidadGrafica = 2;
    public bool pantallaCompleta = true;

    void Start()
    {
        if (inicio)
        {
            if(PlayerPrefs.GetInt("numeroNivel") >= 0) //Cuando ya jugaste
            {
                numeroNivel = PlayerPrefs.GetInt("numeroNivel");
            }
            else //Primera vez
            {
                numeroNivel = 0;
                PlayerPrefs.SetInt("numeroNivel", numeroNivel);
            }
        }
        else
        {
            PlayerPrefs.SetInt("numeroNivel", numeroNivel);
            inicio = false;
            if (PlayerPrefs.HasKey("numeroNivel"))
                {
                    Debug.Log("Nivel guardado: " + PlayerPrefs.GetInt("numeroNivel"));
                }
                else
                {
                    Debug.Log("No hay nivel guardado.");
                }

        }

    }

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

    void Awake()
    {
        // Singleton básico
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

            // Cargar configuraciones guardadas
            volumen = PlayerPrefs.GetFloat("Volume", 1f);
            sensibilidad = PlayerPrefs.GetFloat("Sensitivity", 1f);
            pantallaCompleta = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
            calidadGrafica = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());

            // APLICAR volumen inmediatamente
            AudioListener.volume = volumen;
            Screen.fullScreen = pantallaCompleta;
            QualitySettings.SetQualityLevel(calidadGrafica);
        }
    }


    public void SetNumeroNivel(int nuevoNivel)
    {
        numeroNivel = nuevoNivel;
        PlayerPrefs.SetInt("numeroNivel", numeroNivel);
        PlayerPrefs.Save(); // Guarda los cambios inmediatamente
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

    // Guarda el nivel que se pasa como argumento
    public void GuardarNivel(int nivelAGuardar)
    {
        numeroNivel = nivelAGuardar;
        PlayerPrefs.SetInt("numeroNivel", numeroNivel);
        PlayerPrefs.Save();
        Debug.Log("Nivel guardado nuevo: " + numeroNivel);
    }


    // Carga el nivel guardado
    public string ObtenerEscenaActual()
    {
        if (PlayerPrefs.HasKey("numeroNivel"))
        {
            numeroNivel = PlayerPrefs.GetInt("numeroNivel");
        }
        else
        {
            numeroNivel = 0;
            PlayerPrefs.SetInt("numeroNivel", numeroNivel);
            PlayerPrefs.Save();
        }

        return "Nivel" + numeroNivel;
    }


    // Elimina el dato guardado del nivel
    public void ResetearNivel()
    {
        PlayerPrefs.DeleteKey("numeroNivel");
        numeroNivel = 0;
        Debug.Log("Nivel reseteado. Valor actual: " + numeroNivel);
    }

    public void IrANivel1()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Nivel1");
    }

    
    

    public void CargarConfiguraciones()
    {
        volumen = PlayerPrefs.GetFloat("Volume", 1f);
        sensibilidad = PlayerPrefs.GetFloat("Sensitivity", 1f);
        calidadGrafica = PlayerPrefs.GetInt("QualityLevel", 2);
        pantallaCompleta = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        // Aplicar directamente
        AudioListener.volume = volumen;
        QualitySettings.SetQualityLevel(calidadGrafica);
        Screen.fullScreen = pantallaCompleta;

        Debug.Log("Configuraciones cargadas y aplicadas");
    }

    public void GuardarConfiguraciones()
    {
        PlayerPrefs.SetFloat("Volume", volumen);
        PlayerPrefs.SetFloat("Sensitivity", sensibilidad);
        PlayerPrefs.SetInt("QualityLevel", calidadGrafica);
        Debug.Log("Seguardo la calidad: " + calidadGrafica);
        PlayerPrefs.SetInt("Fullscreen", pantallaCompleta ? 1 : 0);
        PlayerPrefs.Save();

        Debug.Log("Configuraciones guardadas");
    }



}