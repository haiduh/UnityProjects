using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed; // Horizontal movement speed
    [SerializeField] private float jump; // Jump 

    [Header("Air Jump")]
    [SerializeField] private float airTime; //How much time the player has to jump after the player has left the box collider
    private float airCounter; //How much time has passed since the player ran off the edge

    [Header("Multiple Jumps")]
    [SerializeField] private int extraJumps;
    private int jumpCounter;

    [Header("Wall Jumps")]
    [SerializeField] private float wallJumpX;
    [SerializeField] private float wallJumpY;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer; // Ground detection layer
    [SerializeField] private LayerMask wallLayer; // Wall detection layer
    [SerializeField] private LayerMask trapLayer; // Trap detection layer

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound; 

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

        //Jump
        if ((Input.GetKeyDown(KeyCode.Space) ||
             Input.GetKeyDown(KeyCode.UpArrow) ||
             Input.GetKeyDown(KeyCode.W)))
        {
            Jump();
        }

        //Adjustable Jump Height
        if((Input.GetKeyUp(KeyCode.Space) ||
            Input.GetKeyUp(KeyCode.UpArrow) ||
            Input.GetKeyUp(KeyCode.W)) && 
            body.velocity.y > 0)
            body.velocity = new Vector2 (body.velocity.x, body.velocity.y/2);

        if (hasWall())
        {
            body.gravityScale = 0;
            body.velocity = Vector2.zero;
        }
        else
        {
            body.gravityScale = 7;
            body.velocity = new Vector2 (horizontalInput * speed, body.velocity.y);

            if (isGrounded())
            {
                airCounter = airTime; //Resets the counter
                jumpCounter = extraJumps; //Resets the counter
            }
            else 
                airCounter -= Time.deltaTime;
        }
    }

    private void Jump()
    {
        if (airCounter <= 0 && !hasWall() && jumpCounter <= 0) return;

        SoundManager.instance.playSound(jumpSound);

        if (hasWall())
            WallJump();
        else
        {
            if (isGrounded())
                body.velocity = new Vector2(body.velocity.x, jump);
            else 
            {
              
              //Allows players to jump even if off the ground for a certain period of time.
              if (airCounter > 0)
                    body.velocity = new Vector2(body.velocity.x, jump);
              else
                {
                    if (jumpCounter > 0) //Check the amount of jumps and decrease it accordingly
                    {
                        body.velocity = new Vector2(body.velocity.x, jump);
                        jumpCounter--;
                    }
                }
            }

           //Resets the counter to 0 to prevent double jumps
           airCounter = 0;
        }
    }

    private void WallJump()
    {
        body.AddForce(new Vector2(-Mathf.Sign(transform.localScale.x) * wallJumpX, wallJumpY));
        wallJumpCooldown = 0; 
    }

    private bool isGrounded()
    {
        // Check if the player is on the ground using a BoxCast
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer | trapLayer);
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
        return isGrounded() && !hasWall();
    }

    //cutscene manager
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Transition"))
                horizontalInput = 0;
                Debug.Log("Entered Cutscene");
        
    }
}