using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerSystem
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpImpulse;
    [SerializeField] private Transform feet;
    
    private Vector2 dir = Vector2.zero;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // sprite animation
        if(dir.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if(dir.x < 0)
        {
            spriteRenderer.flipX = false;
        }

        rb.linearVelocityX = dir.x * playerSpeed * Time.deltaTime;
    }

    void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();

        dir.x = v.x;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded())
        {
            rb.linearVelocityY = playerJumpImpulse;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.position, Vector2.down, .1f, LayerMask.GetMask("Default"));
        return hit;
    }
}
