using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float playerJumpImpulse;
    [SerializeField] private Transform feet;

    private Vector2 dir = Vector2.zero;

    private Rigidbody2D rb;

  void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

  void Update()
  {
    Debug.DrawLine(feet.position,feet.position+Vector3.down*1f,Color.red);
  }

  void FixedUpdate()
    {
        rb.linearVelocityX = dir.x * playerSpeed * Time.deltaTime;
    }

  public void OnMove(InputValue value)
    {
        var v = value.Get<Vector2>();

        dir.x = v.x;
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded())
        {
            rb.linearVelocityY = playerJumpImpulse;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(feet.position,Vector2.down,.1f,LayerMask.GetMask("Default"));
        return hit;
    }
}
