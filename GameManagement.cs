using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

// The GameManagement class helps to manage the respawning of the player ship and controls a lot of the UI logic
public class GameManagement : MonoBehaviour {

    private static int numberOfLives;                 // the maximum number of lives the player has

    public static int highScore;                      // represents the highest score the player achieved
    public static int scoreOnePlayThrough;            // the score of the player in ONE playthrough
    public static int waveNumber;                     // represents the current wave number of the player
    public static int expPoints = 0;                  // the experience points of the player overall
    public static int maxExpAndScore = 999999999;     // the maximum possible EXP and Score the player can get
    public static bool inRespawnPhase;

    [SerializeField]
    private GameObject playerShipPrefab;              // the prefab used when respawning the player

    private PlayerShip respawnedShip;                 // the current respawned ship

    private Slider playerHealthBar;                   // the player's health bar

    private Text playerLivesText;                     // the UI text of the player lives
    private Text expText;                             // the UI text of the player Exp points
    private Text playerScoreText;                     // the UI text of the player score 
    private Text info;                                // the UI text of the Info tab (Wave Number in Infinite and Allied Ship Icon in Mission)

    public AudioClip gameOverSfx;                     // the game over sfx 
    public GameObject gameOverIcon;                   // the game over icon

    private bool isCoroutineExecuting = false;        // determines if the coroutine is already executing
    private const int ZERO = 0;                       // the minimum possible EXP and Score the player will start with

    // Use this for initialization
    void Start () {

        GameManagement.numberOfLives = 3;
        GameManagement.scoreOnePlayThrough = ZERO;
        GameManagement.waveNumber = 1;
        GameManagement.inRespawnPhase = false;
        this.playerHealthBar = FindObjectOfType<Slider>();
        this.playerLivesText = GameObject.Find("Lives").GetComponent<Text>();
        this.expText = GameObject.Find("EXP").GetComponent<Text>();
        this.playerScoreText = GameObject.Find("Score").GetComponent<Text>();
        this.info = GameObject.Find("WaveInfo").GetComponent<Text>();
        this.respawnedShip = null;
        
    }
	
	// Update is called once per frame
	void Update () {

        // update the number of the lives the player has!
        this.playerLivesText.text = "" + GameManagement.numberOfLives;

        // update the experience points upon collecting one of those orbs
        this.expText.text = "" + GameManagement.expPoints;

        // update the player score upon killing bugs
        this.playerScoreText.text = "" + scoreOnePlayThrough;

        if(GameManagement.waveNumber <= 99)
        {
            this.info.text = "" + GameManagement.waveNumber;
        } else
        {
            this.info.text = "99+";
        }
      
        // if there isn't currently a player ship in the game and we still have lives
		if(FindObjectOfType<PlayerShip>() == null && GameManagement.numberOfLives > 0)
        {
            // execute the respawn of the ship after 1.5 seconds
           StartCoroutine(ExecuteRespawn(1.5f));
        }

	}

    public void RespawnPlayer()
    {

        // decrease the number of lives by 1
        numberOfLives--;

        // if there are still lives remaining
        if(numberOfLives > 0 )
        {
            // create another player in the center of the screen and reset health to 100%
            PlayerShip newPlayer = Instantiate(this.playerShipPrefab, this.playerShipPrefab.transform.position, Quaternion.identity).GetComponent<PlayerShip>();
            newPlayer.SetLaserIndex(PlayerShip.laserIndexBeforeDeath);
            this.respawnedShip = newPlayer;
            this.playerHealthBar.GetComponent<Slider>().value = 100;
            StartCoroutine(FlashShip(12, 0.125f, false));

        } else
        {


            // if you want to just play infinite mode scene without main menu first and w/o Music Player, this prevents a Null Pointer 
            if(MusicPlayer.instance != null)
            {
                MusicPlayer.instance.GetComponent<AudioSource>().Stop();
            }

            GameObject gameOver = Instantiate(this.gameOverIcon, new Vector3(0f, 0f, 0f), Quaternion.identity);
            AudioSource.PlayClipAtPoint(this.gameOverSfx, gameOver.transform.position);

            if(GameManagement.scoreOnePlayThrough > GameManagement.highScore)
            {
                GameManagement.highScore = GameManagement.scoreOnePlayThrough;
            }


            Invoke("LoadMainMenu", 5f);
        }
        
    }

    // Coroutine designed for respawning the player
    IEnumerator ExecuteRespawn(float time)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(time);

        RespawnPlayer();

        isCoroutineExecuting = false;

    }

    // Coroutine designed for flashing the player a certain number of times, either a red color or on/off depending on disable
    public IEnumerator FlashShip(int numTimesToFlash, float delay, bool disable=false) 
    {

        // set the respawn phase to be true to avoid switching colliders upon animation in PlayerShip!
        GameManagement.inRespawnPhase = true;

        // turn off all the colliders 
        this.respawnedShip.DisableAllColliders();

        // flash the player a certain number of times
        for (int i = 0; i < numTimesToFlash; i++)
        {

            if(disable)
            {
                this.respawnedShip.GetComponent<SpriteRenderer>().enabled = false;
            } else
            {
                this.respawnedShip.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f);
            }
           

            yield return new WaitForSeconds(delay);

            if(disable)
            {
                this.respawnedShip.GetComponent<SpriteRenderer>().enabled = true;
            } else
            {
                this.respawnedShip.GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
            }

            yield return new WaitForSeconds(delay);

        }

        GameManagement.inRespawnPhase = false;

    }

    // for returning back to the main menu upon death
    public void LoadMainMenu()
    {

        // reset the wave number back to 1
        GameManagement.waveNumber = 1;

        SceneManager.LoadScene("Main Menu");
    }

}
