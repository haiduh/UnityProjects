using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [Header("Collider")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip meleeAttackSound;

    private float cooldownTimer = Mathf.Infinity;
    private Animator animator;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();

        // Ensure boxCollider is assigned
        if (boxCollider == null)
        {
            boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Debug.LogError("BoxCollider2D is not assigned and couldn't be found on " + gameObject.name);
            }
        }
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (playerVisible())
        {
            // Attack only when the player is visible and playerHealth is not null
            if (cooldownTimer >= attackCooldown && playerHealth != null && playerHealth.currentHealth > 0)
            {
                cooldownTimer = 0;
                animator.SetTrigger("meleeattack");
                SoundManager.instance.playSound(meleeAttackSound);
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !playerVisible();
    }

    private bool playerVisible()
    {
        if (boxCollider == null) return false;  // Exit if boxCollider is not assigned

        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            // Ensure playerHealth is assigned only when a valid player object is detected
            playerHealth = hit.transform.GetComponent<Health>();

            // If the player object no longer has health, reset playerHealth to null
            if (playerHealth == null)
            {
                Debug.LogWarning("Player detected but no Health component found.");
            }
        }
        else
        {
            playerHealth = null;  // Reset playerHealth if player is not visible
        }

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return; // Exit if boxCollider is not assigned

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        );
    }

    private void damagePlayer()
    {
        if (playerVisible() && playerHealth != null)
        {
            // Damage the player if they are visible and playerHealth is assigned
            playerHealth.damageTaken(damage);
        }
    }
}
