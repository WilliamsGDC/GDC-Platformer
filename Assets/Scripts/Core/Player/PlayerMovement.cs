using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private Rigidbody2D rb;
    private bool initialized;

    [SerializeField] private PlayerInput playerInput;

    public PlayerInput PlayerInput => playerInput;

    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float acceleration = 0.1f;

    [Header("Jump Settings")]
    public float jumpForce = 15f;
    public float coyoteTime = 0.2f;
    public float jumpBufferTime = 0.2f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;

    private bool hasDashed;
    private bool isDashing;
    private float dashTimeLeft;

    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    [HideInInspector] public float horizontalInput;
    private bool jumpHeld;
    private bool dashPressed;

    private float velocityXSmoothing;

    public void Initialize()
    {
        player = GetComponentInParent<Player>();
        rb = player.rb;
        initialized = true;
    }

    void Update()
    {
        if (!initialized) return;

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Reset dash on landing
        if (isGrounded && !isDashing)
            hasDashed = false;

        // Coyote time (jumping after walking off a ledge)
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        jumpBufferCounter -= Time.deltaTime;

        // Jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }

        // Dash
        if (dashPressed && !isGrounded && !hasDashed && !isDashing)
        {
            StartDash();
        }
        dashPressed = false;

        // Dash Timer
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0f)
            {
                EndDash();
            }
        }

        // Variable jump height
        if (rb.linearVelocity.y > 0f && !jumpHeld && !isDashing)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1f) * Time.deltaTime;
        }

        // Better fall
        if (rb.linearVelocity.y < 0f && !isDashing)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1f) * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (!initialized || isDashing) return;

        float targetVelocityX = horizontalInput * moveSpeed;
        float smoothedX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, acceleration);
        rb.linearVelocity = new Vector2(smoothedX, rb.linearVelocity.y);
    }

    private void StartDash()
    {
        isDashing = true;
        hasDashed = true;
        dashTimeLeft = dashDuration;

        // Keep horizontal direction, no vertical motion
        float dashDirection = horizontalInput != 0f ? horizontalInput : transform.localScale.x;
        rb.linearVelocity = new Vector2(dashDirection * dashSpeed, 0f); // no Y velocity to prevent falling
    }

    private void EndDash()
    {
        isDashing = false;
        // Optionally reset velocity to prevent excess speed carryover
        rb.linearVelocity = new Vector2(rb.linearVelocity.x * 0.5f, rb.linearVelocity.y);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    // ======================
    // Input System Callbacks
    // ======================

    public void OnMove(InputAction.CallbackContext ctx)
    {
        horizontalInput = ctx.ReadValue<Vector2>().x;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.started)
        {
            jumpHeld = true;
            jumpBufferCounter = jumpBufferTime;
        } else if (ctx.canceled)
        {
            jumpHeld = false;
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        dashPressed = true;
    }
}
