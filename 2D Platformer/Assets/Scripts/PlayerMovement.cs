using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed; // Horizontal movement speed
    [SerializeField] private float jump; // Jump 
    [SerializeField] private LayerMask groundLayer; // Ground detection layer
    [SerializeField] private LayerMask wallLayer; // Wall detection layer

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip character based on movement direction
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);

        // Update animations
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        // Manage wall jump cooldown
        wallJumpCooldown += Time.deltaTime;

        // Horizontal movement
        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            // Wall sliding logic
            if (hasWall() && !isGrounded())
            {
                body.gravityScale = 0; // Stop gravity
                body.velocity = new Vector2(0, -1); // Slow downward slide
            }
            else
            {
                body.gravityScale = 7; // Reset gravity
            }

            // Jump logic
            if ((Input.GetKeyDown(KeyCode.Space) || 
                 Input.GetKeyDown(KeyCode.UpArrow) || 
                 Input.GetKeyDown(KeyCode.W)) && (isGrounded() || hasWall()))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        if (isGrounded())
        {
            // Normal jump
            body.velocity = new Vector2(body.velocity.x, jump);
            animator.SetTrigger("jump");
        }
        else if (hasWall() && !isGrounded())
        {
            // Wall jump
            body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * speed, 15);
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x) * 0.5f, 0.5f, 1); // Flip direction
            wallJumpCooldown = 0; // Reset cooldown
        }
    }

    private bool isGrounded()
    {
        // Check if the player is on the ground using a BoxCast
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool hasWall()
    {
        // Check if the player is touching a wall using a BoxCast
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !hasWall();
    }
}
