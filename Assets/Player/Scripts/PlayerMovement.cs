using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Player player;
    private bool initialized = false;

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
    private bool hasDashed = false;
    private bool isDashing = false;
    private float dashTimeLeft;

    private Rigidbody2D rb;

    private float horizontalInput;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

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

        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

        // Reset dash on landing
        if (isGrounded && !isDashing)
        {
            hasDashed = false;
        }

        // Coyote time
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        // Jump buffer
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // Jump
        if (jumpBufferCounter > 0 && coyoteTimeCounter > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpBufferCounter = 0f;
        }

        // Dash
        if (Input.GetMouseButtonDown(1) && !isGrounded && !hasDashed && !isDashing)
        {
            StartDash();
        }

        // Dash Timer
        if (isDashing)
        {
            dashTimeLeft -= Time.deltaTime;
            if (dashTimeLeft <= 0)
            {
                EndDash();
            }
        }

        // Variable jump height
        if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump") && !isDashing)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Better fall
        if (rb.linearVelocity.y < 0 && !isDashing)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
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
        float dashDirection = horizontalInput != 0 ? horizontalInput : transform.localScale.x;
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
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
        }
    }
}
