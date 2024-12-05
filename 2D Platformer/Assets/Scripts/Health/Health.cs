using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth;
    [SerializeField] private float healthGainDuration;
    [SerializeField] private float healthFlashes;

    public float currentHealth { get; set; }
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float invulnerabilityDuration;
    [SerializeField] private float damageFlashes;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;

    [Header("Sounds")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Start()
    {
        currentHealth = startingHealth;
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
    }

    public void damageTaken (float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth);

        if (currentHealth > 0)
        {
            animator.SetTrigger("hurt");
            SoundManager.instance.playSound(hurtSound);
            StartCoroutine(Invulnerability());
        }
        else
        {
            if (!dead)
            {
                
                foreach (Behaviour component in components)
                    component.enabled = false;

                animator.SetBool("grounded", true);
                animator.SetTrigger("die");

                dead = true;
                SoundManager.instance.playSound(deathSound);
            }
        }
    }

    public void gainHealth(float gain)
    {
        // Increase max health and current health when a heart is collected
        startingHealth += gain;  // Increase the player's max health
        currentHealth = Mathf.Clamp(currentHealth + gain, 0, startingHealth);  // Increase current health without exceeding max health

        StartCoroutine(gainHealthFunc());
    }

    public void Respawn()
    {
        dead = false;
        currentHealth = startingHealth;
        Debug.Log(startingHealth);
        animator.ResetTrigger("die");
        animator.Play("idle");
        StartCoroutine(Invulnerability());

        foreach (Behaviour component in components)
            component.enabled = true;
    }

    //Used because of the yield return waitseconds function
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        for (int i = 0; i < damageFlashes; i++)
        {
            spriteRenderer.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(invulnerabilityDuration / (damageFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(invulnerabilityDuration / (damageFlashes * 2));
        }
        //Duration of invulnerability
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private IEnumerator gainHealthFunc()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        for (int i = 0; i < healthFlashes; i++)
        {
            spriteRenderer.color = new Color(0, 1, 0, 0.5f);
            yield return new WaitForSeconds(healthGainDuration / (healthFlashes * 2));
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(healthGainDuration / (healthFlashes * 2));
        }
        //Duration of invulnerability
        Physics2D.IgnoreLayerCollision(9, 10, false);
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
