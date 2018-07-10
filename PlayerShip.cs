using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerShip : MonoBehaviour {

    public static int laserIndexBeforeDeath;   // represents the current laser index before the player died

    private float playerShipSpeed;     // represents the speed of the player ship horizontally
    private float xMin;                // represents the minimum x-coordinates of the camera the ship can move
    private float xMax;                // represents the maximum x-coordinate of the camera the ship can move 
    private float padding;             // represents an offset of the player ship to fit within the playspace 
    private float fireOffset;          // represents the offset of the projectile from the player

    private Rigidbody2D playerRB;      // needed for Physics of the player ship
    private Slider playerHealthBar;    // the player's health bar next to HP 
    private Animator playerAnimator;   // the animator of the player

    [SerializeField]                                // make 'playerColliders' accessible in the Inspector
    private PolygonCollider2D[] playerColliders;    // different ship colliders

    [SerializeField]
    private GameObject[] playerLasers;              // different weapon prefabs of player (0 - TZapper, 1- SnD, 2- Orbiter, 3- Pelt-O-matic)

    private int currentColliderIndex = 0;           // current active collider (0 - idle, 1- right, 2 - left)
    private int currentLaserIndex = 0;              // current active player weapon 

    public AudioClip playerDeathSfx;                // player death sfx

	// Use this for initialization
	void Start () {

        this.playerRB = GetComponent<Rigidbody2D>();
        this.playerHealthBar = FindObjectOfType<Slider>();
        this.playerAnimator = GetComponent<Animator>();
      
        this.playerShipSpeed = 8f;
        this.padding = .5f;
        this.fireOffset = .3f;

        // acquire z coordinate distance between the sprite and the camera
        float zDistance = this.transform.position.z - Camera.main.transform.position.z;

        // acquire bottom-left and bottom-right Coordinates of the Camera playspace
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, zDistance));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, zDistance));

        // get the minimum and maximum x coordinates the ship will be clamped to on the screen
        this.xMin = bottomLeft.x + this.padding;
        this.xMax = bottomRight.x - this.padding;

	}

    // Update is called once per frame
    void Update()
    {
        ClampShip();

        if (this.playerHealthBar.GetComponent<Slider>().value <= 0)
        {
            GetComponent<Animator>().SetTrigger("Death");
            this.playerRB.velocity = new Vector3(0f, 0f, 0f);
            CancelInvoke("Fire");
        }

        // if this player is in Death State, do NOT allow movement or firing
        if (!this.playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("Death"))
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {

                GetComponent<Animator>().SetTrigger("Trigger_Right");
                this.playerRB.velocity = new Vector3(this.playerShipSpeed, 0f, 0f);

            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {

                GetComponent<Animator>().SetTrigger("Trigger_Left");
                this.playerRB.velocity = new Vector3(-(this.playerShipSpeed), 0f, 0f);
            }
            else
            {


                GetComponent<Animator>().SetTrigger("Trigger_Idle");
                this.playerRB.velocity = new Vector3(0f, 0f, 0f);
            }

            // check if the space bar is pressed down to fire the laser
            CheckIfSpaceBarDown();


            if (StoreManager.isSnDBought && Input.GetKeyDown(KeyCode.Alpha2))
            {
                CancelInvoke("Fire");
                this.currentLaserIndex = 1;

            }
            else if (StoreManager.isOrbiterBought && Input.GetKeyDown(KeyCode.Alpha3))
            {
                CancelInvoke("Fire");
                this.currentLaserIndex = 2;
            }
            else if (StoreManager.isPomBought && Input.GetKeyDown(KeyCode.Alpha4))
            {
                CancelInvoke("Fire");
                this.currentLaserIndex = 3;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CancelInvoke("Fire");
                this.currentLaserIndex = 0;
            }
        }

    }

    void ClampShip()
    {

        // acquire the current Transform of the player ship
        Vector3 currentTransform = this.transform.position;

        // clamp the x position of the player ship to fit between the min and max positions of camera
        currentTransform.x = Mathf.Clamp(currentTransform.x, this.xMin, this.xMax);

        // finally, update the transform the ship
        this.transform.position = currentTransform;

    }


    // This method is called in the Animation Tab with Event Handling upon animations with key presses
    public void SetColliderForSprite(int spriteNum)
    {
        if(!GameManagement.inRespawnPhase)
        {
            // deactivate the current collider
            this.playerColliders[currentColliderIndex].enabled = false;

            //switch to next collider in the animation
            currentColliderIndex = spriteNum;

            // enable the next collider
            this.playerColliders[currentColliderIndex].enabled = true;
        }
       

    }

    private void Fire()
    {

        // the vector offset of the current laser (needed in order to add to transform.position)
        Vector3 offset = new Vector3(0f, this.fireOffset, 0f);

        // get the current Laser Prefab and instantiate one
        GameObject currentLaser = Instantiate(this.playerLasers[currentLaserIndex], this.transform.position + offset, Quaternion.identity) as GameObject;

        // play this laser's fire sfx!
        AudioSource.PlayClipAtPoint(currentLaser.GetComponent<PlayerProjectile>().fireSfx, this.transform.position);

        // set its velocity so that it moves upward
        currentLaser.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, currentLaser.GetComponent<PlayerProjectile>().GetProjectileSpeed());
    }

    private void CheckIfSpaceBarDown()
    {
        
        // if the player presses/holds the spacebar down
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // get the firing rate of the current laser for HOLDING the spacebar
            float fireRate = this.playerLasers[this.currentLaserIndex].GetComponent<PlayerProjectile>().GetFiringRate();

            // get the delay before firing this laser when TAPPING the spacebar
            float delay = this.playerLasers[this.currentLaserIndex].GetComponent<PlayerProjectile>().GetWaitDelay();
            
            // fire the laser at a rate of fireRate after 'delay' seconds have passed (prevents spamming space to fire)
            InvokeRepeating("Fire", delay, fireRate);
        }
        else if (!Input.GetKey(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }
    }

    void OnTriggerEnter2D(Collider2D collidedItem)
    {

        // get the collided game object
        GameObject someCollidedObject = collidedItem.gameObject;

        // if the collided object is an enemy projectile
        if (someCollidedObject.GetComponent<EnemyProjectile>() != null)
        {

            // stop the enemy projectile movement immediately
            someCollidedObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);

            // set the enemy projectile impact animation to render on TOP of the player ship
            someCollidedObject.GetComponent<SpriteRenderer>().sortingLayerName = "Impacts";

            // trigger a Impact animation
            someCollidedObject.GetComponent<Animator>().SetTrigger("Impact");

            // play this enemy's laser impact sfx!
            AudioSource.PlayClipAtPoint(someCollidedObject.GetComponent<EnemyProjectile>().enemyProjectileImpactSfx, this.transform.position);

            // lower the player's health upon impact
            this.playerHealthBar.GetComponent<Slider>().value -= someCollidedObject.GetComponent<EnemyProjectile>().GetDamageAmount() * 25;

        }
    }

    // Destroys the player ship game object
    public void DestructAnimation()
    {
        PlayerShip.laserIndexBeforeDeath = this.currentLaserIndex;
        Destroy(gameObject);
    }   

    // handles information at the beginning of death, such as turning off the collider and play sfx
    public void Death()
    {
        // disable all the colliders upon explosions
        DisableAllColliders();

        // upon death, play the explosion sfx right away
        AudioSource.PlayClipAtPoint(this.playerDeathSfx, this.transform.position);
    }

    // Used for Setting the Laser Index After Dying
    public void SetLaserIndex(int currentLaser)
    {
        this.currentLaserIndex = currentLaser;
    }

    // used for disabling all the colliders on the current player, especially upon death
    public void DisableAllColliders()
    {
        for(int i=0; i < this.playerColliders.Length; i++)
        {
            // deactivate each individual collider on the player
            this.playerColliders[i].enabled = false;
        }
       
    }
}
