using System.Collections.Generic;
using UnityEngine;

public class TreeRopeRenderer : MonoBehaviour
{
    public List<Transform> points;
    private LineRenderer _lineRenderer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = points.Count;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < points.Count; i++)
        {
            _lineRenderer.SetPosition(i, points[i].transform.position);
        }
    }
}
