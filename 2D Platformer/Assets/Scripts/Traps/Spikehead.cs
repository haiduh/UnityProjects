using UnityEngine;

public class Spikehead : EnemyDamage
{
    [Header("Attributes")]
    [SerializeField]private float speed;
    [SerializeField]private float range;
    [SerializeField]private float checkDelay;
    [SerializeField]private LayerMask playerLayer;

    [Header("SFX")]
    [SerializeField] private AudioClip impactSound;

    private float checkTimer;
    private Vector3 destination;
    private bool attacking;
    private Vector3[] directions = new Vector3[4];


    private void OnEnable()
    {
        Stop();
    }
    private void Update()
    {
        //Move spikehead to the player
        if (attacking)
        transform.Translate(destination * Time.deltaTime * speed);
        
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
                checkForPlayer();
        }
    }

    private void checkForPlayer()
    {
        calculateDirections();

        //Checks the position of the player.
        for (int i = 0; i < directions.Length; i++)
        {
            Debug.DrawRay(transform.position, directions[i],Color.red);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, directions[i], range, playerLayer);

            if (hit.collider != null && !attacking)
            {
                attacking = true;
                destination = directions[i];
                checkTimer = 0;
            }   
        }
    }

    private void calculateDirections ()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.up * range;
    }

    private void Stop()
    {
        destination = transform.position; //Set destination as current position so it does not move.
        attacking = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.instance.playSound(impactSound);
        base.OnTriggerEnter2D(collision);
        Stop(); //Stop
    }
}
