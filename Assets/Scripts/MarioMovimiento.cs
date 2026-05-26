using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // Obligatorio para Unity 6

public class PlayerController : MonoBehaviour
{
    // --- Variables de movimiento horizontal ---
    public float speed = 5f;
    private float moveInput;
    private bool facingRight = true;

    // --- Variables de Salto ---
    public int jumpPower = 200;

    // --- Detección de Suelo ---
    private bool isGrounded;
    public Transform feetPos;
    public float checkRadius = 0.04f;
    public LayerMask whatIsGround;

    // --- Componentes ---
    private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Detección física del suelo
        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        // Sincronización exacta con los nombres de tu Animator (Speed e isGround)
        if (animator != null)
        {
            animator.SetBool("isGround", isGrounded);
            animator.SetFloat("Speed", Mathf.Abs(moveInput));
        }

        // Sistema de control moderno para Unity 6
        if (Keyboard.current != null)
        {
            float horizontal = 0f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
            {
                horizontal = 1f;
            }
            else if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
            {
                horizontal = -1f;
            }
            moveInput = horizontal;

            // Saltar: Solo si presiona espacio y está tocando el suelo
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                Jump();
            }
        }

        // Giro automático del personaje
        if (facingRight == false && moveInput > 0) { Flip(); }
        else if (facingRight == true && moveInput < 0) { Flip(); }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpPower);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }
}