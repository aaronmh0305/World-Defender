using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    [SerializeField]
    private GameObject enemyProjectile;     // this enemy's specific projectile spawned when firing

    [SerializeField]
    private GameObject expOrb;             // the enemy experience orb that is spawned upon bug death for the player to collect

    [SerializeField]
    private float firingRate;              // this enemy's firing probability

    [SerializeField]
    private float enemyHealth;             // this enemy's health

    [SerializeField]
    private int enemyScorePerDeath;        // this enemy's score points given per death!

    private Animator enemyAnimator;        // the enemy's animator

    public AudioClip deathSfx;             // bug death sfx

	// Use this for initialization
	void Start () {
        this.enemyAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        // if this bug is NOT in the idle state, do NOT allow firing!
        if(!this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Arrival"))
        {

            // frame rate independent firing probability (for most cases)
            float frIndependentProbability = Time.deltaTime * this.firingRate;
            if (Random.value <= frIndependentProbability)
            {

                // if the bug is a beetle1 or a beetle2, play their firing animations
                if(this.tag == "Beetle1" || this.tag == "Beetle2")
                {
                    
                    this.enemyAnimator.SetTrigger("Attack");
                   
                }

                Fire();

            }

        }

        // if the enemy's health goes to 0, play their death animation
        if(this.enemyHealth <= 0)
        {
            this.enemyAnimator.SetTrigger("Death");
        }
        
	}

    void OnTriggerEnter2D(Collider2D collidedItem)
    {

        // get the collided game object
        GameObject projectile = collidedItem.gameObject;

        // if the collided object has a playerProjectile script attached to it,
        if(projectile.GetComponent<PlayerProjectile>() != null)
        {

            // stop the projectile movement immediately
            projectile.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            if(projectile.tag != "SnD")
            {
                // move the  projectile to be closer to the bug it hits
                projectile.transform.position = this.transform.position;
            }

            // set the player's projectile impact animation to render on TOP of this enemy
            projectile.GetComponent<SpriteRenderer>().sortingLayerName = "Impacts";

            // trigger a Impact animation
            projectile.GetComponent<Animator>().SetTrigger("Impact");


            // play this laser's impact sfx!
            AudioSource.PlayClipAtPoint(projectile.GetComponent<PlayerProjectile>().impactSfx, this.transform.position);

            // if this bug is in their idle or attack animation state, then allow damage to this bug!
            if (this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || this.enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {

                // take the projectiles damage away from this bug's health
                this.enemyHealth -= projectile.GetComponent<PlayerProjectile>().GetDamageAmount();

            }
               
            
        }
    }

    private void Fire()
    {
        // the vector offset of the current laser (needed in order to add to transform.position)
        Vector3 offset = new Vector3(0f, -.2f, 0f);

        // get the current Laser Prefab and instantiate one
        GameObject currentLaser = Instantiate(this.enemyProjectile, this.transform.position + offset, Quaternion.identity) as GameObject;

        // play this enemy laser's fire sfx!
        AudioSource.PlayClipAtPoint(currentLaser.GetComponent<EnemyProjectile>().enemyFireSfx, this.transform.position);

        // set its velocity so that it moves upward
        currentLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -currentLaser.GetComponent<EnemyProjectile>().GetProjectileSpeed());
    }

    // destroy this bug game object after the explosion animation plays
    public void DestructAnimation()
    {

        Destroy(gameObject);
    }

    // disables the bug's collider upon death to allow projecitles to pass through and create new exp orb
    public void DisableColliderAndSpawnExp()
    {

        AudioSource.PlayClipAtPoint(this.deathSfx, this.transform.position);

        // if this bug is a mosquito, turn of its circle collider
        if (this.tag == "Mosquito")
        {
            GetComponent<CircleCollider2D>().enabled = false;
        } else
        {
            // otherwise, turn off the box collider
            GetComponent<BoxCollider2D>().enabled = false;
        }

        // then add to the score upon enemy death
        if(GameManagement.scoreOnePlayThrough + this.enemyScorePerDeath < GameManagement.maxExpAndScore)
        {
            // add to the score upon each enemy's death
            GameManagement.scoreOnePlayThrough += this.enemyScorePerDeath;
        } else
        {
            GameManagement.scoreOnePlayThrough = GameManagement.maxExpAndScore;
        }

        // make the exp orb have a 60% probability of spawning for each bug
        float expProbability = Random.value;
        if(expProbability < 0.60)
        {
            // then create the exp orb to collect
            GameObject orb = Instantiate(this.expOrb, this.transform.position, Quaternion.identity) as GameObject;
            orb.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, -3f);
        }

        
    }



}
