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
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (playerVisible())
        {
            //Attack only when player in sight
            if (cooldownTimer >= attackCooldown && playerHealth.currentHealth > 0)
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
        RaycastHit2D hit = Physics2D.BoxCast
            (boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
             new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
             0,Vector2.left, 0, playerLayer);

        if(hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
 
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void damagePlayer()
    {
        if (playerVisible())
        {
            //Damage the player in range
            playerHealth.damageTaken(damage);
        }
    }
}
