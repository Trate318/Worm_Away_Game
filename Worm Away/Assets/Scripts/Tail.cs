using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] private int length;
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3[] segmentPoses;
    private Vector3[] segmentV;
    [SerializeField] private Transform targetDir;
    [SerializeField] private float targetDist;
    [SerializeField] private float smoothSpeed;
    void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];
    }

    void Update()
    {
        segmentPoses[0] = targetDir.transform.position;
        for (int i = 1; i<segmentPoses.Length; i++) {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i-1] + targetDir.right * targetDist, ref segmentV[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}
