using System;
using UnityEngine;

public class Player : MonoBehaviour
{  
    [SerializeField] private GameObject sprite;
    private Rigidbody2D rb;
    private Vector2 direction;

    [Header("Translational Movement")]
    public float maxSpeed;
    public float baseSpeed;
    public float normalAccel;
    public float fastAccel;
    public float fastDecel;
    public float frictionAmount;
    [Header("Rotational Movement")]
    [SerializeField] private float LerpRotSpeed;
    [SerializeField] private float stepRotSpeed;
    private float angle;

    void Start() {
        rb = gameObject.GetComponent<Rigidbody2D>(); 
    }

    void Update() {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");
        

        if (Input.GetKeyDown(KeyCode.Space)) { // boost
            rb.AddForce(sprite.transform.up * 1000);
        }

        Rotation();
    }
    void FixedUpdate() {
            Movement();
    }

    
    public float vel;
    private void Movement() {


        Vector2 targetSpeed = sprite.transform.up * (direction.y == 1 ? maxSpeed : baseSpeed);
        float forwardVelocity = ScalarProjection(rb.velocity, sprite.transform.up);
        Vector2 speedDif = targetSpeed - rb.velocity.normalized * forwardVelocity;
        float accelRate = direction.y == 0 ? normalAccel : direction.y == 1 ? fastAccel : fastDecel;
        rb.AddForce(speedDif * accelRate);
        
        if (forwardVelocity < 0) {
        Vector2 counterForce = -rb.velocity.normalized * frictionAmount;
        rb.AddForce(counterForce);
        }
        vel = rb.velocity.magnitude;
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
