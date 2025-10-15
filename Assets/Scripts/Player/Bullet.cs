using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 2f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // Destruir la bala después de un tiempo
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Aquí puedes agregar lógica para lo que sucede cuando la bala golpea algo
        Destroy(gameObject);
    }
}
