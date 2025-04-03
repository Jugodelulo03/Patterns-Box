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

    public async Task<TextoMonitor> GetRandomSubvarianteFor(PatronEnganoso patron, string varianteNombre)
    {
        string patronKey = patron.ToString();
        string path = $"patronesEnganosos/{patronKey}/variantes/{varianteNombre}/subvariantes";

        var snapshot = await dbReference.Child(path).GetValueAsync();

        if (!snapshot.Exists)
        {
            Debug.LogWarning($"No se encontraron subvariantes en Firebase para {patronKey} > {varianteNombre}");
            return null;
        }

        List<DataSnapshot> subvariantes = new List<DataSnapshot>();
        foreach (var sub in snapshot.Children)
        {
            subvariantes.Add(sub);
        }

        if (subvariantes.Count == 0)
        {
            Debug.LogWarning($"Subvariantes vacías para {patronKey} > {varianteNombre}");
            return null;
        }

        var seleccionada = subvariantes[Random.Range(0, subvariantes.Count)];

        TextoMonitor texto = new TextoMonitor
        {
            texto1 = seleccionada.Child("texto1").Value?.ToString(),
            texto2 = seleccionada.Child("texto2").Value?.ToString(),
            texto3 = seleccionada.Child("texto3").Value?.ToString(),
            texto4 = seleccionada.Child("texto4").Value?.ToString(),
            texto5 = seleccionada.Child("texto5").Value?.ToString()
        };


        return texto;
    }
}
