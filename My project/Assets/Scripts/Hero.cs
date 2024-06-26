using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float speed = 4f; // скорость движения
    [SerializeField] private int lives = 5; // количество жизней
    [SerializeField] private float jumpForce = 30f; // сила прыжка  
    [SerializeField] private float airControlFactor = 0.6f; // Контроль в воздухе
    private bool isGrounded = false;


    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;
    private bool jumpRequest;


    [SerializeField] private Transform groundCheck; // Точка для проверки, на земле ли персонаж
    [SerializeField] private float checkRadius = 0.2f; // Радиус проверки
    [SerializeField] private LayerMask whatIsGround; // Слой, представляющий землю

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        Move();
        CheckGround();
        if (jumpRequest)
        {
            Jump();
            jumpRequest = false;
        }
    }

    private void Update()
    {
        if (isGrounded) State = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }

    private void Run()
    {
        if (isGrounded) State = States.run;
        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        if (isGrounded)
        {
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(moveInput * speed * airControlFactor, rb.velocity.y);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;
        if (!isGrounded) State = States.jump;
    }
}

public enum States
{
    idle,
    run,
    jump
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 pos;

    private void Awake()
    {
        if (!player)
        {
           player = FindObjectOfType<Hero>().transform;
        }
    }
    private void Update()
    {
        pos = player.position;

        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime);
    }
}