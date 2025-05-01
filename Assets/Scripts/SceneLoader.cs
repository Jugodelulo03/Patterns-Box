// File: SceneLoader.cs
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    private GameObject panelVideoCarga;
    private VideoPlayer videoDeCarga;

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

    public void ConfigurarVideo(GameObject panel, VideoPlayer video)
    {
        panelVideoCarga = panel;
        videoDeCarga = video;
    }

    

    private IEnumerator CargarEscenaAsync(object escena)
    {
        if (panelVideoCarga != null) panelVideoCarga.SetActive(true);
        if (videoDeCarga != null) videoDeCarga.Play();

        yield return new WaitForSeconds(1f);

        AsyncOperation asyncLoad = escena is int index
            ? SceneManager.LoadSceneAsync(index)
            : SceneManager.LoadSceneAsync(escena as string);

        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1f);
                if (videoDeCarga != null) videoDeCarga.Stop();
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void CargarEscenaGuardada()
    {
        string escena = GameStatsManager.Instance.ObtenerEscenaActual();
        StartCoroutine(CargarEscenaAsync(escena));
    }


}
