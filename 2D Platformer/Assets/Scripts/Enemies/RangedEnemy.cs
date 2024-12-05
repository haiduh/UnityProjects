using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;

    [Header("Collider")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;

    [Header("Sounds")]
    [SerializeField] private AudioClip fireBallSound;

    private float cooldownTimer = Mathf.Infinity;
    private Animator animator;
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
            // Attack only when player is in sight
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                animator.SetTrigger("rangedattack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !playerVisible();
    }

    private void rangedAttack()
    {
        SoundManager.instance.playSound(fireBallSound);
        cooldownTimer = 0;
        int fireballIndex = findFireballs();

        if (fireballIndex != -1)
        {
            fireballs[fireballIndex].transform.position = firePoint.position;
            fireballs[fireballIndex].GetComponent<EnemyProjectiles>().ActivateProjectile();
        }
    }

    private int findFireballs()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy)
                return i; // Return the index of an inactive fireball
        }
        return -1; // No inactive fireballs available
    }

    private bool playerVisible()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(
            boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }
}
