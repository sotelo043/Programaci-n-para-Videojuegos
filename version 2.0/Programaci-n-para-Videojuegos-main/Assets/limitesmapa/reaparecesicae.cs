using UnityEngine;

public class LimitesMapa : MonoBehaviour
{
    [Header("Opcional: si lo dejas vacío, busca el objeto llamado Plane")]
    public Transform suelo;

    public float altura = 20f;   // qué tan altas son las paredes
    public float grosor = 2f;    // qué tan gruesas son

    void Start()
    {
        if (suelo == null)
        {
            GameObject plano = GameObject.Find("Plane");
            if (plano != null) suelo = plano.transform;
        }

        if (suelo == null)
        {
            Debug.LogError("LimitesMapa: no se encontró el suelo. Arrástralo a la casilla Suelo.");
            return;
        }

        Collider col = suelo.GetComponent<Collider>();
        if (col == null)
        {
            Debug.LogError("LimitesMapa: el suelo no tiene Collider.");
            return;
        }

        Bounds b = col.bounds;
        float yCentro = b.max.y + altura / 2f;

        // Izquierda y derecha
        CrearPared("Pared_Izquierda",
            new Vector3(b.min.x - grosor / 2f, yCentro, b.center.z),
            new Vector3(grosor, altura, b.size.z + grosor * 2f));

        CrearPared("Pared_Derecha",
            new Vector3(b.max.x + grosor / 2f, yCentro, b.center.z),
            new Vector3(grosor, altura, b.size.z + grosor * 2f));

        // Atrás y adelante
        CrearPared("Pared_Atras",
            new Vector3(b.center.x, yCentro, b.min.z - grosor / 2f),
            new Vector3(b.size.x + grosor * 2f, altura, grosor));

        CrearPared("Pared_Adelante",
            new Vector3(b.center.x, yCentro, b.max.z + grosor / 2f),
            new Vector3(b.size.x + grosor * 2f, altura, grosor));
    }

    void CrearPared(string nombre, Vector3 posicion, Vector3 tamano)
    {
        GameObject pared = new GameObject(nombre);
        pared.transform.SetParent(transform);
        pared.transform.position = posicion;

        BoxCollider caja = pared.AddComponent<BoxCollider>();
        caja.size = tamano;
    }
}