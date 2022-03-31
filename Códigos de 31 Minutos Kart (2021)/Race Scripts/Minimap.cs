using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject trackPath;

    public float grosorDeLinea;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        trackPath = this.gameObject;

        int numOfPath = trackPath.transform.childCount;
        lineRenderer.positionCount = numOfPath + 1;

        for (int i = 0; i < numOfPath; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(trackPath.transform.GetChild(i).transform.position.x,
                4, trackPath.transform.GetChild(i).transform.position.z));
        }
        lineRenderer.SetPosition(numOfPath, lineRenderer.GetPosition(0));

        lineRenderer.startWidth = grosorDeLinea;
        lineRenderer.endWidth = grosorDeLinea;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
