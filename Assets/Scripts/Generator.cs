using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public int Row = 2;
    public int Col = 20;
    public float RowInterval = 10f;
    public float ColInterval = 5f;

    public List<GameObject> Prefabs;
    public GameObject PanelPrefab;
    public GameObject TablePrefab;
    public Transform WorldRoot;

    private void Start()
    {
        Init();
        Generate();
    }

    private void Init()
    {
    }

    private void Generate()
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                GameObject go = Instantiate(TablePrefab);
                go.transform.position = new Vector3(i * RowInterval, 0.5f, j * ColInterval);

                go = Instantiate(Prefabs[(i * Col + j) % Prefabs.Count]);
                MeshRenderer[] mesh = go.GetComponentsInChildren<MeshRenderer>();
                float maxRadius = 0;
                for (int k = 0; k < mesh.Length; k++)
                {
                    Vector3 size = mesh[k].bounds.size;
                    maxRadius = Mathf.Max(maxRadius, size.x, size.y, size.z);
                }
                if (maxRadius != 0) go.transform.localScale = 1f / maxRadius * Vector3.one;
                SphereCollider collider = go.AddComponent<SphereCollider>();
                //go.AddComponent<Model>();
                collider.radius *= maxRadius;
                go.transform.position = new Vector3(i * RowInterval, 2f, j * ColInterval);

                go = Instantiate(PanelPrefab, WorldRoot);
                UIManager.Instance.WorldPanels.Add(go.GetComponent<WorldPanel>());
                go.transform.position = new Vector3(i * RowInterval, 5f, j * ColInterval);
            }
        }
    }
}
