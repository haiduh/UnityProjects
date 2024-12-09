using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth = 100f;
    [SerializeField] private float enragedThreshold = 30f;

    public float currentHealth { get; private set; }
    private bool isEnraged = false; // Tracks whether the boss is enraged
    private bool dead = false;

    [Header("Damage Settings")]
    [SerializeField] private float damageDuration = 1f;
    [SerializeField] private float damageFlashes = 5f;

    [Header("Components")]
    [SerializeField] private Behaviour[] components; // Components to disable on death
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject finishObj;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip enragedSound;

    [Header("UI")]
    [SerializeField] private Slider healthBar;

    private void Start()
    {
        currentHealth = startingHealth;
        UpdateHealthBar();
    }

    /// Handles damage taken by the boss.
    public void TakeDamage(float damage)
    {
        if (dead) return;

        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            // Trigger hurt sound
            SoundManager.instance.playSound(hurtSound);

            // Check if the boss should become enraged
            CheckEnrageState();

        }
        else
        {
            // Boss is dead
            Die();
        }

        UpdateHealthBar();
    }

    /// Checks if the boss should enter the enraged state.
    private void CheckEnrageState()
    {
        if (!isEnraged && currentHealth <= enragedThreshold)
        {
            isEnraged = true;
            SoundManager.instance.playSound(enragedSound);
            animator.SetBool("IsEnraged", true); // Set enraged animation.
            StartCoroutine(Invulnerability());
        }
    }

    /// Handles the death of the boss.
    public void Die()
    {
        if (dead) return;

        dead = true;

        // Stop the boss background music
        if (SoundManager.instance != null)
        {
            SoundManager.instance.changeBackgroundMusic(null); // Stop background music
        }

        // Play the death sound
        if (SoundManager.instance != null && deathSound != null)
        {
            SoundManager.instance.playSound(deathSound); // Play the death sound
        }

        // Trigger death animation
        if (animator != null)
        {
            animator.SetTrigger("die");
        }

        // Disable components after a short delay
        foreach (Behaviour component in components)
            component.enabled = false;

        // Destroy boss object after a delay
        Destroy(gameObject, 8f);
    }

    /// Updates the health bar.
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth / startingHealth;
        }
    }

    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(12, 8, true);
        for (int i = 0; i < damageFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f); // Red flash
            yield return new WaitForSeconds(damageDuration / (damageFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(damageDuration / (damageFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(12, 8, false);
    }

    private void finishObject()
    {
        if (finishObj != null)
        {
            finishObj.SetActive(true); // Activate the finish object
            animator.SetBool("idle", true); // Ensure the finish object animator is set to idle
        }
    }


    public bool IsEnraged()
    {
        return isEnraged;
    }

}
