using UnityEngine;

public class ReaparecerSiCae : MonoBehaviour
{
    public float alturaMinima = -10f; // si baja de esta altura, reaparece

    Vector3 posicionInicial;
    Quaternion rotacionInicial;
    Rigidbody rb;
    CharacterController cc;

    void Start()
    {
        posicionInicial = transform.position;
        rotacionInicial = transform.rotation;
        rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (transform.position.y < alturaMinima)
        {
            Reaparecer();
        }
    }

    void Reaparecer()
    {
        if (cc != null) cc.enabled = false;

        transform.position = posicionInicial;
        transform.rotation = rotacionInicial;

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (cc != null) cc.enabled = true;
    }
}