using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    [SerializeField] private int length; // How many vertices tail will have
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3[] segmentPoses;
    private Vector3[] segmentV;

    [SerializeField] private Transform targetDir; // Starting Position of Tail
    [SerializeField] private float targetDist;
    [SerializeField] private float smoothSpeed;
    void Start()
    {
        lineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentV = new Vector3[length];

        for (int i = 1; i < segmentPoses.Length; i++) {
            segmentPoses[i] = segmentPoses[i - 1] + targetDir.up * targetDist;
        }
    }

    void Update()
    {
        segmentPoses[0] = targetDir.transform.position;
        for (int i = 1; i<segmentPoses.Length; i++) {
            Vector3 targetPos = segmentPoses[i - 1] + (segmentPoses[i] - segmentPoses[i - 1]).normalized * targetDist;
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], targetPos, ref segmentV[i], smoothSpeed);
        }
        lineRenderer.SetPositions(segmentPoses);
    }
}
