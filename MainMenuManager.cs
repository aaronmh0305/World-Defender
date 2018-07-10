using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MainMenuManager : MonoBehaviour {

    private Text highScoreText;     // the text that represents the player's high score

	// Use this for initialization
	void Start () {
        this.highScoreText = GameObject.Find("High Score").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        this.highScoreText.text = "High Score: " + GameManagement.highScore;
	}
}
