using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour
{
    // 라인렌더러
    private LineRenderer myLineRenderer;
    // 직선 끝 transform
    public Transform destinationObject;

    private void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        myLineRenderer.startWidth = 0.05f;
        myLineRenderer.endWidth   = 0.05f;
    }
    
    // 라인 그리기
    private void Update()
    {
        myLineRenderer.SetPosition(0, transform.position);
        myLineRenderer.SetPosition(1, destinationObject.position);
    }
}
