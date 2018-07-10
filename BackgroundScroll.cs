using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{

    private Vector3 startPosition;       // Vector3 position to set the background at upon replacement
    private int replacePosition = -10;   // the y-position on the screen to replace the background at

    private SpriteRenderer backgroundSpriteRenderer;   // accesses the sprite renderer of this background

    private bool hasBackgroundReplaced;                // determines if the background has replaced to ensure it's okay to change its color
    private float saturation;
    private float value;

    // Use this for initialization
    void Start()
    {
        this.startPosition = new Vector3(0f, 140f, 0f);
        this.backgroundSpriteRenderer = GetComponent<SpriteRenderer>();

        // create temporary variables to store the hue, saturation, and value of the editor's color
        float h, s, v;
        Color.RGBToHSV(this.backgroundSpriteRenderer.color, out h, out s, out v);

        // save the saturation and value of the original color 
        this.saturation = s;
        this.value = v;

        // change to a random color
        changeTintOfBackground();

        this.hasBackgroundReplaced = false;
    }

    // Update is called once per frame
    void Update()
    {

        // if the backgrounds are lower than the replace position, reset them!
        if (this.transform.position.y <= replacePosition)
        {
            this.transform.position = startPosition;
            this.hasBackgroundReplaced = true;
        }

        if(hasBackgroundReplaced)
        {
            changeTintOfBackground();
            this.hasBackgroundReplaced = false;

        }

        // translate the backgrounds down at a speed of 1.5
        this.transform.position += Vector3.down * Time.deltaTime * 2.5f;
    }

    void changeTintOfBackground()
    {
        // get a random hue ONLY (value between 0 and 1)
        float hue = Random.value;
        
        // change the hue in RGB terms and keep the saturation and value the same as the first iteration (Set in the Editor)
        Color newColor = Color.HSVToRGB(hue, this.saturation, this.value);

        // update the sprite renderer with the new color
        this.backgroundSpriteRenderer.color = newColor;
    }
}
