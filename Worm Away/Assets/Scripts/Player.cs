using System;
using UnityEngine;

public class Player : MonoBehaviour
{  
    [SerializeField] private GameObject sprite;
    private Rigidbody2D rb;
    private Vector2 direction;

    [Header("Translational Movement")]
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

    public float maxSpeed;
    public float baseSpeed;
    public float accelaration;
    public float decelaration;
    public float movement;
    public float vel;
    public float curVel;
    public bool hello;
    private void Movement() {


        Vector2 targetSpeed = sprite.transform.up * (direction.y == 1 ? maxSpeed : baseSpeed);
        Vector2 curDirectionalSpeed = rb.velocity.normalized * ScalarProjection(rb.velocity, sprite.transform.up);
        Vector2 speedDif = targetSpeed - curDirectionalSpeed;
        rb.AddForce(speedDif);
        
        
        rb.AddForce(rb.velocity * -frictionAmount); // constant drag so i can more easily change it in code
        vel = rb.velocity.magnitude;
        curVel = curDirectionalSpeed.magnitude;
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
