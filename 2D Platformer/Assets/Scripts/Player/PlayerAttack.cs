using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] iceBalls;
    [SerializeField] private AudioClip iceBallSound;


    private Animator animator;
    private PlayerMovement playerMovement;
    private float attackTime = Mathf.Infinity;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && attackTime > attackCooldown && playerMovement.canAttack())
        {
            Attack();
        }
        attackTime += Time.deltaTime;
    }

    private void Attack()
    {
        SoundManager.instance.playSound(iceBallSound);
        animator.SetTrigger("attack");
        attackTime = 0;

        //pooling iceballs

        int iceballIndex = FindIceball();
        iceBalls[iceballIndex].transform.position = firePoint.position;
        iceBalls[iceballIndex].SetActive(true);  // Ensure the iceball is active when set
        iceBalls[iceballIndex].GetComponent<Projectiles>().SetDirection(Mathf.Sign(transform.localScale.x));
    }

    private int FindIceball()
    {
        for (int i = 0; i < iceBalls.Length; i++)
        {
            if (!iceBalls[i].activeInHierarchy)
                return i;
        }
        return 0;
    }
}
