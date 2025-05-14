using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{  
    [SerializeField] private GameObject sprite;
    private Rigidbody2D rb;
    private float rbMass;
    private Vector2 direction;

    public Terrain terrain;
    [Header("Translational Movement")]
    public float maxSpeed;
    public float baseSpeed;
    public float normalAccel;
    public float fastAccel;
    public float fastDecel;
    public float frictionAmount;
   

    [Header("Dash")]
    public bool canDash;
    public float dashForce;
    public float dashCooldown;
    public float dashCooldownTimer;
    [Header("Rotational Movement")]
    [SerializeField] private float LerpRotSpeed;
    [SerializeField] private float stepRotSpeed;

    public enum Terrain {
        Sand,
        Air
    }

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        dashCooldownTimer = dashCooldown;
    }

    void Update() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        vel = rb.velocity.magnitude; // debug help

        dashCooldownTimer += Time.deltaTime;
        if (dashCooldownTimer > dashCooldown)
        {
            canDash = true;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canDash) { // boost
            dashCooldownTimer = 0;
            canDash = false;
            // rb.velocity += (Vector2) sprite.transform.up * dashForce;
            rb.AddForce(sprite.transform.up * rb.mass * dashForce);
        }


        switch (terrain) {
            case Terrain.Sand:
                Debug.Log("you're in sand");
                SandRotation();
                rb.gravityScale = 0;
                rb.drag = 1;
                break;
            case Terrain.Air:
                Debug.Log("you're in the air");
                AirRotation();
            rb.gravityScale = 2;
            rb.drag = 0.2f;
            break;
        }
        // if (!Input.GetMouseButton(0)) {
            
        // }
        // else {
            
        // }

       
    }
    void FixedUpdate() {
        switch (terrain) {
            case Terrain.Sand:
                SandMovement();
                break;
            case Terrain.Air:
                break;
        }
    }

    
    public float vel;
    public float forwardVelocity;
    private void SandMovement() {
        

        Vector2 targetSpeed = sprite.transform.up * (direction.y == 1 ? maxSpeed : baseSpeed);
        forwardVelocity = ScalarProjection(rb.velocity, sprite.transform.up);
        Vector2 speedDif = targetSpeed - rb.velocity.normalized * forwardVelocity;
        float accelRate = direction.y == 0 ? normalAccel : direction.y == 1 ? fastAccel : fastDecel;
        
        rb.AddForce(speedDif * accelRate);

        Vector2 counterForce = -rb.velocity.normalized * frictionAmount;
        rb.drag = 1;
        // rb.AddForce(counterForce); // friction
        if (forwardVelocity < 0) // doubled if moving too fast or slow
            rb.AddForce(counterForce) ;
        
        
    }

    private void SandRotation() {
        float currentAngle = sprite.transform.eulerAngles.z;
        float targetAngle = currentAngle - direction.x * stepRotSpeed;
        float angle = Mathf.LerpAngle(currentAngle, targetAngle, LerpRotSpeed * Time.deltaTime);
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void AirRotation() {
        float currentAngle = sprite.transform.eulerAngles.z;
        float targetangle = Mathf.Atan2(-rb.velocity.x, rb.velocity.y) * Mathf.Rad2Deg;
        float angle = Mathf.LerpAngle(currentAngle, targetangle, LerpRotSpeed * Time.deltaTime);
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);

        
    }

    private float ScalarProjection(Vector2 projectedVector, Vector2 baseVector) {
        return Vector2.Dot(projectedVector, baseVector) / baseVector.magnitude;
    }


    private void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Sand")) {
            terrain = Terrain.Sand;
        }
        else if (other.CompareTag("Air")) {
            terrain = Terrain.Air;
        }
    }
}
