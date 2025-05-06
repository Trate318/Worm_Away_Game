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

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    void Update()
    {    
        Movement();
        FollowMouse();
    }

    private void Movement()
    {
        rb.velocity = sprite.transform.up * speed;
    }

    private void FollowMouse()
    {
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        float currentAngle = sprite.transform.eulerAngles.z;
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);
        
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);

    }
}
