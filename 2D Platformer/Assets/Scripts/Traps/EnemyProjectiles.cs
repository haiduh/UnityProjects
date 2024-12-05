using UnityEngine;

public class EnemyProjectiles : EnemyDamage //Inherting from Enemy damage script
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
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (hit) { return; }
        float movementSpeed = speed * Time.deltaTime;
        transform.Translate(movementSpeed, 0, 0);

        lifetime += Time.deltaTime;
        if (lifetime > resetTime)
            Deactivate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        hit = true;
        base.OnTriggerEnter2D(collision); //Execute logic from parent script 

        if (animator != null)
            animator.SetTrigger("explode");
        else
            Deactivate(); //When this hits any object deactivate 
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
