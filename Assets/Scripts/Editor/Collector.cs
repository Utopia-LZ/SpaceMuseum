using UnityEditor;
using UnityEngine;

public class Collector : EditorWindow 
{
    private SpawnPoint[] Points;
    private Generator generator;
    private Transform ModelRoot;

    [MenuItem("Tools/Collector")]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(Collector));
        window.Show();
    }

    private void OnEnable()
    {
        generator = FindObjectOfType<Generator>();
        ModelRoot = GameObject.Find("ModelRoot").transform;
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Generate"))
        {
            Collect();
            Collect();
            Generate();
        }
        GUILayout.Space(10);
        if(GUILayout.Button("Clear models"))
        {
            ClearModels();
        }
    }

    private void Collect()
    {
        Points = FindObjectsOfType<SpawnPoint>();
        Debug.Log(Points.Length + " points in total.");
    }

    private void Generate()
    {
        foreach (SpawnPoint sp in Points)
        {
            generator.Generate(sp.transform.position, sp.Index);
        }
    }

    private void ClearModels()
    {
        generator.Clear();
    }
}
