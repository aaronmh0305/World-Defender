using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFormation : MonoBehaviour {

    [SerializeField]
    private GameObject[] enemyPrefabs;   // index 0 - beetle1, 1 - mosquito, 2 - beetle 2, 3 - bruiser beetle

    private float xMin;                  // the minimum x-coordinate of the camera (play space)
    private float xMax;                  // the maximum x-coordinate of the camera (play space)
    private float enemyFormationSpeed;   // the enemy formation speed 
    private float spawnDelay;            // the time delay for spawning the next enemy
    private bool isMovingRight;          // tracks whether the enemy formation is moving to the right or left 

    public float width;                  // the width of the Wire Cube surrounding the Bugs (Enemy Formation Space)
    public float height;                 // the height of the Wire Cube surrounding the Bugs (Enemy Formation Space)

    // Use this for initialization
    void Start () {

        // z distance between the camera and this enemy formation Empty 
        float zDistance = this.transform.position.z - Camera.main.transform.position.z;

        // get access to the bottom-left and the bottom-right coordinates of the camera
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, zDistance));
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1f, 0f, zDistance));

        // set the minimum and maximum x coordinates of the play space
        this.xMin = bottomLeft.x;
        this.xMax = bottomRight.x;
        this.isMovingRight = true;

        this.enemyFormationSpeed = 1.5f;
        this.spawnDelay = 1f;

        SpawnNextRow();
    }
	
	// Update is called once per frame
	void Update () {

        // if the enemy formation is moving right
		if(this.isMovingRight)
        {

            // update the position of the entire enemy formation to the right
            this.transform.position += Vector3.right * Time.deltaTime * this.enemyFormationSpeed;

            // if the formation reaches the right side of the screen (or goes off of it slightly)
            if(this.transform.position.x + (this.width / 2) >= this.xMax)
            {

                // switch to move left
                this.isMovingRight = false;
            }

        } else
        {
            // if moving left, update the position of entire enemy formation to the left
            this.transform.position += Vector3.left * Time.deltaTime * this.enemyFormationSpeed;

            // if the formation reaches the left side of the screen (or goes off slightly)
            if(this.transform.position.x - (this.width / 2) <= this.xMin)
            {

                // switch to move right
                this.isMovingRight = true;
            }
        }

        // respawn all bugs once they're dead
        if(AllEnemiesDead())
        {
            SpawnNextRow();
        }

	}

    // checks if all enemies are dead
    private bool AllEnemiesDead()
    {

        // for each position object in the enemy formation
        foreach (Transform positionObj in this.transform)
        {

            // if there is still another bug left, then not all are dead
            if(positionObj.childCount > 0)
            {
                return false;
            }

        }

        // add 1 to the wave number
        GameManagement.waveNumber++;

        return true;
    }


    // used when wanting to spawn all enemies at once (at start of game)
    private void SpawnAllEnemies()
    {

        // for each position object underneath the Enemy Formation Empty
        foreach (Transform currentPos in this.transform)
        {

            // generate a random index for which Bug to Spawn and parent that bug to its Position Transform
            int randomEnemyIndex = Random.Range(0, this.enemyPrefabs.Length);
            GameObject randomEnemySpawned = Instantiate(this.enemyPrefabs[randomEnemyIndex], currentPos.transform.position, Quaternion.identity) as GameObject;
            randomEnemySpawned.transform.parent = currentPos;

        }
    }

    // spawns each individual row
    private void SpawnNextRow()
    {
        for(int i=0; i < 13; i++)
        {
            Transform positionObj = FindNextFreePosition();

            // generate a random index for which Bug to Spawn
            int randomEnemyIndex;

            if (GameManagement.waveNumber <= 2)
            {
                // spawn only beetle 1's and mosquitoes in the first 2 waves
                randomEnemyIndex = Random.Range(0, 2);

            } else if(GameManagement.waveNumber == 3)
            {
                // spawn beetle 1's, mosquitoes and beetle 2's in wave 3
                randomEnemyIndex = Random.Range(0, 3);
            } else
            {

                // anything from wave 4 and onward includes the bruiser
                randomEnemyIndex = Random.Range(0, this.enemyPrefabs.Length);

            }

            GameObject randomEnemySpawned = Instantiate(this.enemyPrefabs[randomEnemyIndex], positionObj.transform.position, Quaternion.identity) as GameObject;
            randomEnemySpawned.transform.parent = positionObj;
        }

        // if there is another free position, then repeat spawning
        if (FindNextFreePosition() != null)
        {
            Invoke("SpawnNextRow", this.spawnDelay);
        }
    }

    // used when wanting to spawn enemies one-by-one
    private void SpawnUntilFull()
    {

        // check to see if there is another position available
        Transform freePosition = FindNextFreePosition();

        if(freePosition != null)
        {

            // generate a random index for which Bug to Spawn and parent that bug to its Position Transform
            int randomEnemyIndex = Random.Range(0, this.enemyPrefabs.Length);
            GameObject randomEnemySpawned = Instantiate(this.enemyPrefabs[randomEnemyIndex], freePosition.transform.position, Quaternion.identity) as GameObject;
            randomEnemySpawned.transform.parent = freePosition;

        }

        // if there is another free position, then repeat spawning
        if(FindNextFreePosition() != null)
        {
            Invoke("SpawnUntilFull", this.spawnDelay);
        }
    }

    private Transform FindNextFreePosition()
    {

        // find the first position that doesn't have an enemy in it and return it, if it exists
        foreach (Transform positionObj in this.transform) {

            if(positionObj.childCount == 0)
            {
                return positionObj;
            }
        }

        // return null if a free position doesn't exist
        return null;
    }

    // Draws the box in the editor for the Enemy formation
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(this.transform.position, new Vector3(this.width, this.height, 0f));
    }


}
