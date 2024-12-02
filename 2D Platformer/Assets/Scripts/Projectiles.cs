using UnityEngine;

public class Projectiles : MonoBehaviour
{
    [SerializeField] private float speed;
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
        if (projectileLife > 3) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        boxCollider.enabled = false;
        animator.SetTrigger("explode");
    }

    public void SetDirection(float paraDirection, bool status)
    {
        projectileLife = 0;
        direction = paraDirection;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = status;

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
