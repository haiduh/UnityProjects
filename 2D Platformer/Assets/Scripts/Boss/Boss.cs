using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Transform player;

    public bool isFlipped = false;

    public void lookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f; // Prepare the flipped scale

        // If the player is to the left and the boss is not flipped
        if (transform.position.x > player.position.x && !isFlipped)
        {
            transform.localScale = flipped; // Flip the boss
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true; // Mark as flipped
        }
        // If the player is to the right and the boss is flipped
        else if (transform.position.x < player.position.x && isFlipped)
        {
            transform.localScale = flipped; // Flip the boss back
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false; // Mark as not flipped
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
