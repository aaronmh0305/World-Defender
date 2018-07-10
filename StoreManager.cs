using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StoreManager : MonoBehaviour {

    public static bool isOrbiterBought = false;    // determines if the orbiter has been bought
    public static bool isSnDBought = false;        // determines if the SnD has been bought
    public static bool isPomBought = false;        // determines if the Pom has been bought 

    private bool updatedOrbiterSpriteState = false; // checks if orbiter already updated its sprite state to non-highlighted
    private bool updatedSnDSpriteState = false;     // checks if SnD already updated its sprite state to non-highlighted
    private bool updatedPomSpriteState = false;     // checks if POM already updated its sprite state to non-highlighted

    private Text playerExpText;                    // the player's experience points Text UI element

    private Image OrbiterButton;                   // the Orbiter Button UI Image named 'Orbiter Button' in Canvas 
    private Image SnDButton;                       // the SnD Button UI Image named 'SnD Button' in Canvas 
    private Image PomButton;                       // the Pom Button UI Image named 'POM Button' in Canvas

    public Sprite orbiterBoughtIcon;               // the Sprite that represents the Orbiter Button after it is Bought and grayed out
    public Sprite snDBoughtIcon;                   // the Sprite that represents the SnD Button after it is bought and grayed out
    public Sprite pomBoughtIcon;                   // the Sprite that represents the POM Button after it is bought and grayed out

    private Text orbiterPriceText;                 // the orbiter price UI Text
    private Text snDPriceText;                     // the SnD price UI Text
    private Text pomPriceText;                     // the POM price UI Text

    private const int sndPrice = 150;              // the price of the SnD
    private const int orbiterPrice = 175;          // the price of the orbiter
    private const int pomPrice = 200;              // the price of the POM

	// Use this for initialization
	void Start () {

        this.playerExpText = GameObject.Find("Player Exp Points").GetComponent<Text>();
        this.orbiterPriceText = GameObject.Find("Orbiter Price").GetComponent<Text>();
        this.snDPriceText = GameObject.Find("Snd Price").GetComponent<Text>();
        this.pomPriceText = GameObject.Find("POM Price").GetComponent<Text>();

        this.OrbiterButton = GameObject.Find("Orbiter Button").GetComponent<Image>();
        this.SnDButton = GameObject.Find("SnD Button").GetComponent<Image>();
        this.PomButton = GameObject.Find("POM Button").GetComponent<Image>();

    }
	
	// Update is called once per frame
	void Update () {

        this.playerExpText.text = "" + GameManagement.expPoints;

        this.orbiterPriceText.text = "EXP: " + orbiterPrice;
        this.snDPriceText.text = "EXP: " + sndPrice;
        this.pomPriceText.text = "EXP: " + pomPrice;

        // unity only allows you to change the 'Source Image' by changing its 'sprite' field
        if (isSnDBought)
        {
            if(!updatedSnDSpriteState)
            {
                // remove the highlighted sprite state completely
                SpriteState boughtSnDState = new SpriteState();
                boughtSnDState.highlightedSprite = null;

                // set the new sprite state to not include the SnD highlighted state
                GameObject.Find("SnD Button").GetComponent<Button>().spriteState = boughtSnDState;
                updatedSnDSpriteState = true;

            }

            SnDButton.sprite = this.snDBoughtIcon;

        }

        if (isOrbiterBought)
        {

            if(!updatedOrbiterSpriteState)
            {
                // remove the highlighted sprite state completely!
                SpriteState boughtOrbiterState = new SpriteState();
                boughtOrbiterState.highlightedSprite = null;

                // set the new sprite state to not include the orbiter highlighted state
                GameObject.Find("Orbiter Button").GetComponent<Button>().spriteState = boughtOrbiterState;

                updatedOrbiterSpriteState = true;
                
            }

            OrbiterButton.sprite = this.orbiterBoughtIcon;

        }

        if (isPomBought)
        {

            if(!updatedPomSpriteState)
            {
                // remove the highlighted sprite state completely
                SpriteState boughtPomState = new SpriteState();
                boughtPomState.highlightedSprite = null;

                // set the new sprite state to not include the orbiter highlighted state
                GameObject.Find("POM Button").GetComponent<Button>().spriteState = boughtPomState;

                updatedPomSpriteState = true;
            }

            PomButton.sprite = this.pomBoughtIcon;

        }

    }

    // for purchasing the orbiter after the user presses the Orbiter Button down
    public void PurchaseOrbiter()
    {
        // if the orbiter isn't bought yet, and the user has enough points
        if (!isOrbiterBought && GameManagement.expPoints >= orbiterPrice)
        {
            GameManagement.expPoints -= orbiterPrice;
            StoreManager.isOrbiterBought = true;
           
        } 
    }

    // for purchasing the SnD after the user presses the SnD Button down
    public void PurchaseSnD()
    {
        // if the SnD isn't bought yet, and the user has enough points
        if (!isSnDBought && GameManagement.expPoints >= sndPrice)
        {
            GameManagement.expPoints -= sndPrice;
            StoreManager.isSnDBought = true;

        }
    }

    // for purchasing the pom after the user presses the POM Button down
    public void PurchasePom()
    {

        // if the POM isn't bought yet, and the user has enough points
        if (!isPomBought && GameManagement.expPoints >= pomPrice)
        {
            GameManagement.expPoints -= pomPrice;
            StoreManager.isPomBought = true;

        }
    }
}
