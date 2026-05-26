using UnityEngine;

public class GoombaController : MonoBehaviour
{
    // --- Variables de movimiento ---
    public float speed = 2f;
    public bool moveRight = false;

    // --- Variables de muerte por aplastamiento ---
    private bool isCrushed = false;
    public float offset = 0.2f;          // Tu altura ajustada a 0.2f para Triggers

    // --- Componentes ---
    private Rigidbody2D rb;
    private Animator animator;

    // Control de tiempo para evitar atascos
    private float tiempoUltimoChoque = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        if (isCrushed)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return;
        }

        if (moveRight)
        {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocity.y);
        }
    }

    // Usamos únicamente OnTriggerEnter2D porque desactivaste el choque físico en la matriz
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // CORREGIDO: "Mario1" con M mayúscula idéntico a tu Unity
        if (collision.gameObject.CompareTag("Mario1"))
        {
            if (collision.transform.position.y > transform.position.y + offset)
            {
                Die();
            }
            else
            {
                Debug.Log("Goomba hirió a Mario!");
            }
        }

        // Detecta rebotes con tubos o enemigos
        if (collision.gameObject.CompareTag("Tube") || collision.gameObject.CompareTag("enemies"))
        {
            if (Time.time - tiempoUltimoChoque > 0.1f)
            {
                moveRight = !moveRight;
                tiempoUltimoChoque = Time.time;
            }
        }
    }

    void Die()
    {
        isCrushed = true;

        if (animator != null)
        {
            animator.SetBool("isCrushed", true);
        }

        // Apaga colisionadores al morir
        Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
        foreach (Collider2D col in colliders)
        {
            col.enabled = false;
        }

        Destroy(gameObject, 1f);
    }
}