using UnityEngine;

public class EnemyProjectiles : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float resetTime;
    private float lifetime;
    private Animator animator;
    private BoxCollider2D boxCollider;

    private bool hit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    public void ActivateProjectile()
    {
        hit = false;
        lifetime = 0;
        gameObject.SetActive(true); // Re-enable the fireball
        Debug.Log("Fireball Activated: " + gameObject.name); // Log when fireball is activated
    }

    private void Update()
    {
        if (hit) { return; }

        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0); // Move the fireball

        lifetime += Time.deltaTime;

        // Log the lifetime and reset time of the fireball
        Debug.Log("Fireball Lifetime: " + lifetime + " | Reset Time: " + resetTime);

        // If the fireball has existed longer than its reset time, deactivate it
        if (lifetime > resetTime)
        {
            Deactivate();
            Debug.Log("Fireball Deactivated: " + gameObject.name); // Log when the fireball is deactivated
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hit) { return; }

        hit = true; // Mark the fireball as hit
        base.OnTriggerEnter2D(collision); // Handle damage from the parent script

        // Trigger explosion animation if available
        if (animator != null)
        {
            animator.SetTrigger("explode");
        }
        else
        {
            Deactivate(); // Deactivate immediately if no animator is found
        }

        Debug.Log("Fireball Hit: " + gameObject.name); // Log when the fireball hits something
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); // Deactivate the fireball
        Debug.Log("Fireball Deactivated from Deactivate method: " + gameObject.name); // Log when deactivation happens
    }
}
