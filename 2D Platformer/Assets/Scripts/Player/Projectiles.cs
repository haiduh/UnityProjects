using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float enemyDamage;
    [SerializeField] private float bossDamage;
    private float direction;
    private bool hit;
    private float projectileLife;

    private Animator animator;
    private BoxCollider2D boxCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (hit) return;
        float movementSpeed = speed * Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        projectileLife += Time.deltaTime;
        if (projectileLife > 3) Deactivate();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Iceball") && !collision.CompareTag("Transition") && !collision.CompareTag("Trap") && !collision.CompareTag("Health"))
            { 
                hit = true;
                animator.SetTrigger("explode");
            }
        else
        {
            return;
        }

        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<Health>().damageTaken(enemyDamage);
        }

        if (collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossHealth>().TakeDamage(bossDamage);
        }
    }

    public void SetDirection(float paraDirection)
    {
        projectileLife = 0;
        direction = paraDirection;
        hit = false;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != paraDirection)
            localScaleX = -localScaleX;

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
