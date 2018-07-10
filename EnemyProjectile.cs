using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    [SerializeField]
    private float projectileSpeed;               // how fast the projectile moves

    [SerializeField]
    private float damageAmount;                  // how much damage the projectile deals upon hit

    public AudioClip enemyFireSfx;               // the sound effect to play upon firing with this laser
    public AudioClip enemyProjectileImpactSfx;   // the sound effect to play upon impact with this laser

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

    // Destroys this projectile after impact animation played near a bug
    public void DestructAnimation()
    {
        Destroy(gameObject);
    }

    // turns off the enemy projectile collider when impacting the player
    public void TurnOffImpactCollider()
    {
        this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }
}
