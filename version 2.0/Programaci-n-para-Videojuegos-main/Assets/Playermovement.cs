using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Velocidad del personaje
    public float speed = 5f;

    Rigidbody rb;
    Vector3 direccion;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // evita que se incline o gire al chocar
        rb.interpolation = RigidbodyInterpolation.Interpolate; // movimiento suave
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // no atraviesa paredes
    }

    void Update()
    {
        // Lee las teclas W, A, S, D o las flechas
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        direccion = new Vector3(moveX, 0f, moveZ);

        // Evita que en diagonal vaya más rápido
        if (direccion.magnitude > 1f)
        {
            direccion.Normalize();
        }
    }

    void FixedUpdate()
    {
        // Mueve con física, conservando la caída por gravedad
        Vector3 velocidad = direccion * speed;
        velocidad.y = rb.linearVelocity.y;
        rb.linearVelocity = velocidad;
    }
}