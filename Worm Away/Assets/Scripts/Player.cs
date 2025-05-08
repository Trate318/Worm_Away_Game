using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{  
    [SerializeField] private GameObject sprite;
    private Rigidbody2D rb;
    private Vector2 movement;
    private float direction;

    [Header("Movement")]

    [SerializeField] private float speed;
    [SerializeField] private float boostSpeed;
    private bool isBoosting;
    [SerializeField] private float LerpRotSpeed;
    [SerializeField] private float stepRotSpeed;
    private float angle;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {    
        Movement();
        Rotation();
    }

    private void Movement()
    {
        rb.velocity = sprite.transform.up * (isBoosting ? boostSpeed : speed);    
    }

    private void Rotation()
    {
        direction = Input.GetAxisRaw("Horizontal");   
        float currentAngle = sprite.transform.eulerAngles.z;
        float targetAngle = currentAngle - direction * stepRotSpeed;
        angle = Mathf.LerpAngle(currentAngle, targetAngle, LerpRotSpeed * Time.deltaTime);
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
