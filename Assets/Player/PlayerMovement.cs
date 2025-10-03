using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerMovement : MonoBehaviour
{
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

    [Header("Sprite/Visuals")]
    public List<Sprite> rightSprites;
    public List<Sprite> leftSprites;
    public Color rightColor = Color.green;
    public Color leftColor = Color.red;
    public float animationFrameRate = 10f; // frames per second

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float horizontalInput;
    private bool isGrounded;
    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private float velocityXSmoothing;
    private Vector2 currentVelocity;

    private int currentFrame;
    private float animationTimer;
    private bool isFacingRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

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

        // Variable jump height
        if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        // Better fall
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }

        HandleSpriteAnimation();
    }

    private void FixedUpdate()
    {
        float targetVelocityX = horizontalInput * moveSpeed;
        float smoothedX = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, acceleration);
        rb.linearVelocity = new Vector2(smoothedX, rb.linearVelocity.y);
    }

    private void HandleSpriteAnimation()
    {
        // Only animate if moving
        if (horizontalInput != 0)
        {
            isFacingRight = horizontalInput > 0;
            animationTimer += Time.deltaTime;

            if (animationTimer >= 1f / animationFrameRate)
            {
                animationTimer = 0f;

                List<Sprite> currentList = isFacingRight ? rightSprites : leftSprites;
                if (currentList != null && currentList.Count > 0)
                {
                    currentFrame = (currentFrame + 1) % currentList.Count;
                    spriteRenderer.sprite = currentList[currentFrame];
                }
                else
                {
                    // Fallback to color change
                    spriteRenderer.color = isFacingRight ? rightColor : leftColor;
                }
            }
        }
        else
        {
            // for future people 
            currentFrame = 0;
        }
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
