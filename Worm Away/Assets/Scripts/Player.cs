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
    private Vector2 direction;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float maxSpeedChange;
    [SerializeField] private float accelSpeed;
    [SerializeField] private float maxBoostSpeed;
    [SerializeField] private float boostSpeed;
    private bool isBoosting;
    [SerializeField] private float LerpRotSpeed;
    [SerializeField] private float stepRotSpeed;
    private float angle;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    void Update() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        
        Rotation();

        if (Input.GetKeyDown(KeyCode.G)) {
            rb.AddForce(Vector2.down* 10);
        }
    }
    void FixedUpdate() {
        if (isBoosting) {
            ;
        }   
        else {
            Movement();
        }
    }

    private void Movement() {
        Vector2 targetSpeed = sprite.transform.up * speed ;
        Vector2 speedDif = targetSpeed - rb.velocity;
        rb.AddForce(movement);
    }

    private void Rotation() {
        float currentAngle = sprite.transform.eulerAngles.z;
        float targetAngle = currentAngle - direction.x * stepRotSpeed;
        angle = Mathf.LerpAngle(currentAngle, targetAngle, LerpRotSpeed * Time.deltaTime);
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
