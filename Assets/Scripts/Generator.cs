using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : MonoBehaviour
{
    public int Row = 2;
    public int Col = 20;
    public float RowInterval = 10f;
    public float ColInterval = 5f;
    public bool FixGenerate = true;

    public List<GameObject> Prefabs;
    public GameObject PanelPrefab;
    public GameObject TablePrefab;
    public Transform WorldRoot;
    public Transform ModelRoot;

    public List<GameObject> ModelList = new List<GameObject>();
    public List<GameObject> TableList = new List<GameObject>();
    public List<GameObject> PanelList = new List<GameObject>();

    private void Start()
    {
        Init();
        if(FixGenerate) FixedGenerate();
    }

    private void Init()
    {
        foreach(GameObject go in PanelList)
            UIManager.Instance.WorldPanels.Add(go.GetComponent<WorldPanel>());
    }

    private void FixedGenerate()
    {
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Col; j++)
            {
                Generate(i, 0, j, i * Col + j, true);
            }
        }
    }

    public void Generate(Vector3 pos, int idx)
    {
        Generate(pos.x, pos.y, pos.z, idx);
    }

    public void Generate(float x, float y, float z, int idx, bool fixedPos = false)
    {
        GameObject table = Instantiate(TablePrefab, ModelRoot);
        TableList.Add(table);

        GameObject model = Instantiate(Prefabs[idx % Prefabs.Count], ModelRoot);
        MeshRenderer[] mesh = model.GetComponentsInChildren<MeshRenderer>();
        float maxRadius = 0;
        for (int k = 0; k < mesh.Length; k++)
        {
            Vector3 size = mesh[k].bounds.size;
            maxRadius = Mathf.Max(maxRadius, size.x, size.y, size.z);
        }
        SkinnedMeshRenderer[] skin = model.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int k = 0; k < skin.Length; k++)
        {
            Vector3 size = skin[k].bounds.size;
            maxRadius = Mathf.Max(maxRadius, size.x, size.y, size.z);
        }
        if (maxRadius != 0) model.transform.localScale = 1f / maxRadius * Vector3.one;
        SphereCollider collider = model.AddComponent<SphereCollider>();
        collider.radius *= maxRadius;
        ModelList.Add(model);

        GameObject panel = Instantiate(PanelPrefab, WorldRoot);
        PanelList.Add(panel);
        Vector3 pos = new Vector3(x, y, z);
        if (fixedPos) pos = Mul(pos, new Vector3(RowInterval, 1f, ColInterval));
        table.transform.position = pos + Vector3.up * 1f;
        model.transform.position = pos + Vector3.up * 2.5f;
        panel.transform.position = pos + Vector3.up * 5.5f;

        string str = "Content/" + model.GetComponent<Model>().Name + "_0";
        str = Resources.Load<TextAsset>(str).text;
        panel.GetComponent<WorldPanel>().SetContent(str);
    }

    public void Clear()
    {
        foreach(GameObject go in ModelList) DestroyImmediate(go);
        foreach(GameObject go in TableList) DestroyImmediate(go);
        foreach(GameObject go in PanelList) DestroyImmediate(go);
        ModelList.Clear();
        TableList.Clear();
        PanelList.Clear();
    }

    private Vector3 Mul(Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
}
