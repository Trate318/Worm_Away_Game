using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Transform playerSprite;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private float maxLookAhead;
    [SerializeField] private float minLookAhead;
    [SerializeField] private float lookAheadDamping;
    [SerializeField] private float lerpSpeed;
    
    void Start()
    {
        transform.position = playerSprite.position;
    }

    void Update()
    {
        float curVel = playerRB.velocity.magnitude > minLookAhead ? playerRB.velocity.magnitude : 0;
        Vector3 lookAhead = playerRB.velocity.normalized * Mathf.Min(curVel, maxLookAhead) * lookAheadDamping;
        Vector3 targetPosition = new Vector3(0, 0, -10) + playerSprite.position + lookAhead;

        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
