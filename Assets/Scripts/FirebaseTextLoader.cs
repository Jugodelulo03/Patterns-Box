using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using System.Threading.Tasks;

public class FirebaseTextLoader : MonoBehaviour
{
    private DatabaseReference dbReference;

    void Awake()
    {
        var app = Firebase.FirebaseApp.DefaultInstance;
        dbReference = FirebaseDatabase.GetInstance(app, "https://patternsbox-a0af9-default-rtdb.firebaseio.com/").RootReference;
    }

    public async Task<TextoMonitor> GetRandomEnganoVisualTextAsync()
    {
        var snapshot = await dbReference
            .Child("patronesEnganosos")
            .Child("EnganoVisual")
            .Child("variantes")
            .GetValueAsync();

        if (snapshot.Exists)
        {
            List<TextoMonitor> variantes = new List<TextoMonitor>();

            foreach (var child in snapshot.Children)
            {
                string rawJson = child.GetRawJsonValue();
                Debug.Log("Raw JSON recibido: " + rawJson);

                TextoMonitor textos = JsonUtility.FromJson<TextoMonitor>(rawJson);
                variantes.Add(textos);
            }

            if (variantes.Count > 0)
            {
                int randomIndex = Random.Range(0, variantes.Count);
                TextoMonitor varianteElegida = variantes[randomIndex];

                Debug.Log("Textos seleccionados de Firebase:");
                Debug.Log("Texto 1: " + varianteElegida.texto1);
                Debug.Log("Texto 2: " + varianteElegida.texto2);
                Debug.Log("Texto 3: " + varianteElegida.texto3);
                Debug.Log("Texto 4: " + varianteElegida.texto4);
                Debug.Log("Texto 5: " + varianteElegida.texto5);

                return varianteElegida;
            }
        }

        Debug.LogWarning("No se encontraron variantes en Firebase.");
        return null;
    }
}

