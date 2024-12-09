using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed; // Horizontal movement speed
    [SerializeField] private float jump; // Jump force

    [Header("Air Jump")]
    [SerializeField] private float airTime; // Time the player can jump after leaving the ground
    private float airCounter; // Tracks remaining air jump time

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps; // Number of additional jumps allowed
    private int jumpCounter;

    [Header("Wall Jumps")]
    [SerializeField] private float wallJumpX; // Wall jump horizontal force
    [SerializeField] private float wallJumpY; // Wall jump vertical force

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; // Layer for ground detection
    [SerializeField] private LayerMask wallLayer; // Layer for wall detection
    [SerializeField] private LayerMask trapLayer; // Layer for traps

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    public bool canMove = true; // Allows toggling movement

    private Rigidbody2D body;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    private Health playerHealth;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        playerHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (!canMove) return;

        horizontalInput = Input.GetAxis("Horizontal");

        // Flip character based on direction
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);

        // Update animations
        animator.SetBool("run", horizontalInput != 0);
        animator.SetBool("grounded", isGrounded());

        // Handle Jump
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }

        // Adjustable jump height
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W)) && body.velocity.y > 0)
        {
            body.velocity = new Vector2(body.velocity.x, body.velocity.y / 2);
        }

        if (wallJumpCooldown > 0)
        {
            wallJumpCooldown -= Time.deltaTime;
            Debug.Log("Wall jump cooldown active: " + wallJumpCooldown);
        }
        else if (hasWall() && !isGrounded())
        {
            Debug.Log("Wall detected. Player sticking to the wall.");
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        }

        if (isGrounded())
        {
            airCounter = airTime; // Reset air jump timer
            jumpCounter = extraJumps; // Reset extra jumps
        }
        else
        {
            airCounter -= Time.deltaTime; // Decrease air time
        }
    }

    private void Jump()
    {
        if (airCounter <= 0 && !hasWall() && jumpCounter <= 0)
        {
            Debug.Log("Cannot jump: No air time or extra jumps remaining.");
            return;
        }

        SoundManager.instance.playSound(jumpSound);

        if (hasWall() && wallJumpCooldown <= 0)
        {
            WallJump();
            return;
        }

        if (isGrounded() || airCounter > 0)
        {
            Debug.Log("Performing a ground or air jump.");
            body.velocity = new Vector2(body.velocity.x, jump);
        }
        else if (jumpCounter > 0)
        {
            Debug.Log("Performing an extra jump.");
            body.velocity = new Vector2(body.velocity.x, jump);
            jumpCounter--;
        }

        airCounter = 0; // Reset air counter to prevent double jumps
    }

    private void WallJump()
    {
        Debug.Log("WallJump triggered!");
        body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY);
        wallJumpCooldown = 0.2f; // Small delay to prevent instant re-sticking to the wall
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            boxCollider.bounds.center,
            boxCollider.bounds.size,
            0,
            Vector2.down,
            0.1f,
            groundLayer | trapLayer);

        Debug.DrawRay(boxCollider.bounds.center, Vector2.down * 0.1f, Color.green);
        return raycastHit.collider != null;
    }

    private bool hasWall()
    { 
        Vector2 direction = new Vector2(transform.localScale.x, 0); // Check direction the player is facing
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, direction, 0.1f, wallLayer);

        // Debugging BoxCast visualization
        return raycastHit.collider != null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death"))
        {
            Debug.Log("Player collided with Death object.");
            playerHealth.damageTaken(99);
        }
    }

    public bool canAttack()
    {
        return isGrounded() && !hasWall();
    }
}
