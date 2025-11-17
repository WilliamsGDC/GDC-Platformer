using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    private Player player;
    private bool initialized = false;

    [Header("Main Body Animation")]
    public Animator animator;

    [Header("Animation Settings")]
    public int animationSpeed = 10; // frames per second

    // input
    private float horizontalInput;
    private bool isMoving;
    private bool isFacingRight;

    public void Initialize()
    {   
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();

        animator.speed = animationSpeed;

        initialized = true;
    }
    
    void Update()
    {
        if (!initialized) return;

        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (!(horizontalInput == 0)) // moving
        {
            isMoving = true;
            
            if (horizontalInput >= 0)
            {
                isFacingRight = true;
            } else
            {
                isFacingRight = false;
            }
        
        } else // not moving
        {
            isMoving = false;
        }
        
        animator.SetBool("isMoving", isMoving);
        animator.SetBool("isFacingRight", isFacingRight);
    }
}
