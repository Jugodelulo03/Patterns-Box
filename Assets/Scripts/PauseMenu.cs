using UnityEngine;
using System.Collections; // Necesario para usar corrutinas

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Asigna el panel de pausa en el Inspector
    public MonoBehaviour playerController; // Asigna el script de MFPC en el Inspector
    private bool isPaused = false;
    public Animator animador; // Asigna el Animator en el Inspector
    public string triggerName = "SubirCarpeta";
    public string triggerName1 = "BajarCarpeta";

    void Start()
    {
        // Asegurar que el cursor esté bloqueado al inicio del juego
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Verifica que el Animator esté asignado
        if (animador == null)
        {
            Debug.LogError("⚠️ No se ha asignado un Animator al PauseMenu.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Presiona ESC para pausar/despausar
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (!isPaused)
        {
            // **PAUSA**
            isPaused = true;
            pausePanel.SetActive(true);
            Time.timeScale = 0f; // Detiene el juego
            playerController.enabled = false; // Deshabilita el movimiento

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (animador != null)
            {
                animador.SetBool("isPause", true);
            }
        }
        else
        {
            // **DESPAUSA CON RETRASO**
            StartCoroutine(ResumeWithDelay());
        }
    }

    IEnumerator ResumeWithDelay()
    {
        if (animador != null)
        {
            animador.SetBool("isPause", false);
        }

        yield return new WaitForSecondsRealtime(0.4f); // Espera 1 segundo sin depender de Time.timeScale

        isPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Reanuda el juego
        playerController.enabled = true; // Habilita el movimiento

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ResumeGame()
    {
        if (isPaused) TogglePause();
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // Asegurar que el tiempo vuelva a la normalidad
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
