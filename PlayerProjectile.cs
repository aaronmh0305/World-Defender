using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    [SerializeField]
    private float projectileSpeed;     // how fast the projectile moves

    [SerializeField]
    private float damageAmount;        // how much damage the projectile deals upon hit

    [SerializeField]
    private float firingRate;         // how much time it takes to spawn the next laser when HOLDING down the spacebar

    [SerializeField]
    private float waitDelayUponSpace; // how much time it takes before invoking the fire when TAPPING the spacebar

    public AudioClip fireSfx;        // the sound effect to play upon firing with this laser
    public AudioClip impactSfx;      // the sound effect to play upon impact with this laser


    // gets the projectile speed
    public float GetProjectileSpeed()
    {
        return this.projectileSpeed;
    }

    // gets the damage this projectile deals
    public float GetDamageAmount()
    {
        return this.damageAmount;
    }

    // gets the firing rate of this projectile
    public float GetFiringRate()
    {
        return this.firingRate;
    }

    // gets the wait delay before spawning the next projectile
    public float GetWaitDelay()
    {
        return this.waitDelayUponSpace;
    }

    // Destroys this projectile after impact animation played near a bug
    public void DestructAnimation()
    {
        Destroy(gameObject);
    }

    // turns off this player's projectile collider when impacting a bug
    public void TurnOffImpactCollider()
    {
        if(this.tag == "TZ")
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        } else
        {
            this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        }
        
    }
}
