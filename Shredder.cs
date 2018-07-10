using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shredder : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D collidedItem)
    {

        // only destroy the collided game object if it is a Player Projectile, enemy projectile or exp orb off screen
        if(collidedItem.gameObject.GetComponent<PlayerProjectile>() != null 
            || collidedItem.gameObject.GetComponent<EnemyProjectile>() != null 
            || collidedItem.gameObject.GetComponent<ExpOrb>() != null)
        {

            Destroy(collidedItem.gameObject);

        }
    }
}
