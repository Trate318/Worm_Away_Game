using System;
using UnityEngine;

public class Player : MonoBehaviour
{  
    [SerializeField] private GameObject sprite;
    private Rigidbody2D rb;
    private float rbMass;
    private Vector2 direction;

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
    private float angle;


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

        Rotation();
    }
    void FixedUpdate() {
        if (!Input.GetMouseButton(0))
            SandMovement();

        if (Input.GetMouseButton(1))
            rb.gravityScale = 3;
        else 
            rb.gravityScale = 0;
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

    private void Rotation() {
        float currentAngle = sprite.transform.eulerAngles.z;
        float targetAngle = currentAngle - direction.x * stepRotSpeed;
        angle = Mathf.LerpAngle(currentAngle, targetAngle, LerpRotSpeed * Time.deltaTime);
        sprite.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private float ScalarProjection(Vector2 projectedVector, Vector2 baseVector) {
        return Vector2.Dot(projectedVector, baseVector) / baseVector.magnitude;
    }
}
