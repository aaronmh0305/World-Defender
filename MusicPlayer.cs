using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ATTACH A MUSIC PLAYER ONLY IN THE FIRST SCENE THAT WILL BE RUN // 
public class MusicPlayer : MonoBehaviour {

    public static MusicPlayer instance = null;

    public AudioClip[] songs = new AudioClip[2];   // index 0 = main menu theme, 1 - battle

    private AudioSource mySource;

    private bool isMainMenuPlaying;

	// Use this for initialization
	void Start () {
		
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            Debug.Log("Duplicate destroying");
        } else
        {
            instance = this;
            Object.DontDestroyOnLoad(gameObject);
            this.mySource = GetComponent<AudioSource>();

            mySource.volume = 0.75f;
            mySource.clip = songs[0];
            mySource.loop = true;
            mySource.Play();
            isMainMenuPlaying = true;
        }

	}
	
	private void OnLevelWasLoaded(int level)
    {
        
        // if the level is infinite mode
        if(level == 1) {
           
            mySource.Stop();
            isMainMenuPlaying = false;
            mySource.volume = 0.30f;
            mySource.clip = songs[1];
            mySource.loop = true;
            mySource.Play();
           
        } else if (level == 0 && !isMainMenuPlaying)
        {
            mySource.Stop();
            isMainMenuPlaying = true;
            mySource.volume = 0.6f;
            mySource.clip = songs[0];
            mySource.loop = true;
            mySource.Play();
        }
   

        
    }
}
