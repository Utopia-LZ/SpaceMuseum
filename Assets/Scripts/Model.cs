using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    private List<Transform> total;

    public bool CanSplit = false;
    public float SplitRate = 2f;

    private void Start()
    {
        total = FindChildren(transform);
    }

    private List<Transform> FindChildren(Transform root)
    {
        List<Transform> res = new List<Transform> { root };
        foreach (Transform child in root)
        {
            res.AddRange(FindChildren(child));
        }
        return res;
    }

    public void SetLayer(int layer)
    {
        foreach (Transform tf in total) tf.gameObject.layer = layer;
    }

    public void Split()
    {
        foreach(Transform part in total)
        {
            //Debug.Log(transform.position + " " + part.position + " " + part.name);
            part.position += part.position - transform.position;
        }
    }

    public void Assemble()
    {
        foreach (Transform part in total)
        {
            part.position += transform.position;
            part.position /= 2;
        }
    }
}
