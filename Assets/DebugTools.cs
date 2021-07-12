using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class DebugTools : MonoBehaviour
{

    public GameObject pre;
    int index = 0;
    public Camera cam;
    public GameObject go;
    public Vector3[] vectors;
    public float scale;

    // Start is called before the first frame update
    void Start()
    {
        // Assembly assembly = Assembly.GetExecutingAssembly();
        // assembly.GetTypes()./
        vectors = cam.GetSides(go.transform);
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     GameObject go = GameObject.Instantiate(pre, table.gameObject.transform);
        //     go.transform.localScale = Vector3.one;
        //     go.transform.localPosition = Vector3.one;
        //     go.name = index.ToString();
        //     index++;
        //     table.enabled = true;
        //     table.Reposition();
        // }
    }
    void OnDrawGizmos()
    {
        for(int i = 0;i<vectors.Length;i++)
        {
            Gizmos.DrawSphere(vectors[i],50);
        }
    }
}
