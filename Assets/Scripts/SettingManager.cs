using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public GameStatsManager GameStatsManager;
    public CalidadGrafica calidadGrafica;

    [Header("Referencias de Entrada")]
    

    public float MinSensi, MaxSensi;

    [Header("Volumen")]
    public Slider volumeSlider;
    public TMP_Text volumeText;

    [Header("Sensibilidad")]
    public Slider sensitivitySlider;
    public TMP_Text sensitivityText;

    [Header("Pantalla Completa")]
    public Image fullscreenButtonImage;
    public Sprite fullscreenSprite;
    public Sprite windowedSprite;


    private bool isFullscreen;

    void Start()
    {
        Debug.Log("¿Sliders iguales?: " + (volumeSlider == sensitivitySlider));

        // Limitar rango de sensibilidad manualmente
        sensitivitySlider.minValue = MinSensi; // o el mínimo que prefieras
        sensitivitySlider.maxValue = MaxSensi; // equivale a 500%

        // Cargar valores guardados
        isFullscreen = Screen.fullScreen;
        fullscreenButtonImage.sprite = isFullscreen ? fullscreenSprite : windowedSprite;

        volumeSlider.value = PlayerPrefs.GetFloat("Volume", AudioListener.volume);

        // Clampear el valor por si venía de un PlayerPrefs viejo
        float sensibilidadGuardada = Mathf.Clamp(PlayerPrefs.GetFloat("Sensitivity", 1f), sensitivitySlider.minValue, sensitivitySlider.maxValue);
        sensitivitySlider.value = sensibilidadGuardada;

        UpdateVolumeUI(volumeSlider.value);
        UpdateSensitivityUI(sensitivitySlider.value);

        int calidadGuardada = PlayerPrefs.GetInt("QualityLevel", QualitySettings.GetQualityLevel());
        calidadGrafica.AplicarCalidadGuardada(calidadGuardada);
    }


    void OnDisable()
    {
        if (GameStatsManager.Instance != null)
        {
            volumeSlider.value = GameStatsManager.Instance.volumen;
            sensitivitySlider.value = Mathf.Clamp(GameStatsManager.Instance.sensibilidad, sensitivitySlider.minValue, sensitivitySlider.maxValue);

            isFullscreen = GameStatsManager.Instance.pantallaCompleta;
            fullscreenButtonImage.sprite = isFullscreen ? fullscreenSprite : windowedSprite;

            // <- ESTE CAMBIO FALTABA
            Screen.fullScreen = isFullscreen;

            UpdateVolumeUI(volumeSlider.value);
            UpdateSensitivityUI(sensitivitySlider.value);
        }
    }



    public void OnVolumeChanged(float value)
    {
        //Debug.Log("cambiando volumen a: " + value);
        UpdateVolumeUI(value);
    }

    void UpdateVolumeUI(float value)
    {
        AudioListener.volume = value;
        volumeText.text = Mathf.RoundToInt(value * 100f) + "%";
    }

    public void OnSensitivityChanged(float value)
    {
        //Debug.Log("cambiando sensibilidad a: " + value);
        UpdateSensitivityUI(value);
    }

    void UpdateSensitivityUI(float value)
    {
        sensitivityText.text = Mathf.RoundToInt(value * 100f) + "%";
        // Aquí podrías aplicar directamente a tu sistema de control
    }

    public void ToggleFullscreen()
    {
        isFullscreen = !isFullscreen;
        fullscreenButtonImage.sprite = isFullscreen ? fullscreenSprite : windowedSprite;
        // NO cambies Screen.fullScreen aquí
    }


    public void OnQualityChanged(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void ApplySettings()
    {
        if (GameStatsManager.Instance != null)
        {
            GameStatsManager.Instance.volumen = volumeSlider.value;
            GameStatsManager.Instance.sensibilidad = sensitivitySlider.value;
            GameStatsManager.Instance.pantallaCompleta = isFullscreen;
            GameStatsManager.Instance.calidadGrafica = calidadGrafica.ObtenerCalidadActual();

            // Aquí sí aplicas los cambios de pantalla
            Screen.fullScreen = isFullscreen;

            GameStatsManager.Instance.GuardarConfiguraciones();
        }
    }




}
