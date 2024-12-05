using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private float attackCooldown;    // Time between attacks
    [SerializeField] private Transform firePoint;     // Point from where the arrow is fired
    [SerializeField] private GameObject[] arrows;     // Array of arrow game objects
    private float cooldownTimer;                      // Timer to track cooldown

    [Header("SFX")]
    [SerializeField] private AudioClip arrowSFX;

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        // If cooldown timer exceeds or equals attack cooldown, attack
        if (cooldownTimer >= attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        
        int arrowIndex = FindArrows(); // Find an available arrow

        if (arrowIndex != -1) // Ensure a valid arrow is found
        {
            cooldownTimer = 0; // Reset cooldown timer
            SoundManager.instance.playSound(arrowSFX);
            // Reset the position of the arrow to the fire point
            arrows[arrowIndex].transform.position = firePoint.position;

            // Activate the arrow
            arrows[arrowIndex].GetComponent<EnemyProjectiles>().ActivateProjectile();
        }

    }

    private int FindArrows()
    {
        // Find an inactive arrow
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy)
            {
                return i; // Return the index of the first inactive arrow
            }
        }

        // Return -1 if no arrow is available
        return -1;
    }
}
