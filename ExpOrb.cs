using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpOrb : MonoBehaviour {

    public AudioClip orbCollisionSfx;   // the orb collection sfx

    void OnTriggerEnter2D(Collider2D collidedShip)
    {

        // if this experience orb collided with the player ship
        if(collidedShip.gameObject.GetComponent<PlayerShip>() != null)
        {

            // if the experience points are less than the maximum possible points, add one point
            if(GameManagement.expPoints < GameManagement.maxExpAndScore)
            {
                // add to the Exp UI
                GameManagement.expPoints += 1;
            }

            AudioSource.PlayClipAtPoint(this.orbCollisionSfx, this.transform.position);
            Destroy(gameObject);

        }

    }
}
