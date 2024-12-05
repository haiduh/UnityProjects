using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    [SerializeField] private float healthValue;
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            SoundManager.instance.playSound(pickupSound);
            collision.GetComponent<Health>().gainHealth(healthValue);
            gameObject.SetActive(false);

        }
    }
}
