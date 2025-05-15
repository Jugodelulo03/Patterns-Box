using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel,Main,Ajustes;
    public MonoBehaviour playerController;
    public MonoBehaviour playerController2;
    public Animator animador;

    //variables estaticas
    public static bool GameIsPaused { get; private set; } = false;
    public static bool IsInteracting = false;

    //audioDialogo
    public AudioSource dialogueAudio; // arrástralo en el inspector
    public AudioSource MusicAudio;// por si quieren pausar la musica tambien

    public float delayBeforeResume = 0.4f; // Tiempo de espera para que termine la animación de salida

    void Start()
    {
        IsInteracting = false;
        GameIsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsInteracting==false)
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void PauseGame()
    {
        GameIsPaused = true;
        pausePanel.SetActive(true);
        Main.SetActive(true);
        Ajustes.SetActive(false);
        Time.timeScale = 0f;

        PauseResumeAudios(true);

        if (playerController != null) playerController.enabled = false;
        if (playerController2 != null) playerController2.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (animador != null) animador.SetBool("isPause", true);
    }


    public void PauseResumeAudios(bool Action)
    {
        if(Action == true)//pausar audios
        {
            //pausa dialogo
            if (dialogueAudio != null && dialogueAudio.isPlaying)
            {
                dialogueAudio.Pause();
            }

            //quitar los /**/ por si se quiere pausar la musica tambien

            /*
             if (MusicAudio != null)
            {
                MusicAudio.UnPause(); // o Play() si prefieres que se reinicie
            }
             */
        }
        else
        {
            //Resume Dialogo
            if (dialogueAudio != null)
            {
                dialogueAudio.UnPause(); // o Play() si prefieres que se reinicie
            }

            //no olvidar esta parte de codigo para que reanude la musica
            /*
            if (MusicAudio != null)
            {
                MusicAudio.UnPause(); // o Play() si prefieres que se reinicie
            }
            */
        }
    }
    public void ResumeGame()
    {
        StartCoroutine(ResumeAfterDelay());
    }

    IEnumerator ResumeAfterDelay()
    {
        if (animador != null) animador.SetBool("isPause", false);

        yield return new WaitForSecondsRealtime(delayBeforeResume); // Espera sin depender de Time.timeScale

        GameIsPaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1f;

        PauseResumeAudios(false);
        


        if (playerController != null) playerController.enabled = true;
        if (playerController2 != null) playerController2.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
