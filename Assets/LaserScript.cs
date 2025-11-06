using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{

    public Sprite laserOnSprite;
    public Sprite laserOffSprite;

    // time to toggle the lazer
    public float toggleInterval = 0.5f;
    // rotaion speed of lazer
    public float rotationSpeed = 0.0f;

    private bool isLaserOn = true;
    private float timeUntilNextToggle;
  
    private Collider2D laserCollider;
    private SpriteRenderer laserRenderer;


    public void Start()
    {
        // 
        timeUntilNextToggle = toggleInterval;
        // assign refs
        laserCollider = gameObject.GetComponent<Collider2D>();
        laserRenderer = gameObject.GetComponent<SpriteRenderer>();

    }

    public void Update()
    {
        // minus the timer
        timeUntilNextToggle -= Time.deltaTime;
        //2
        if (timeUntilNextToggle <= 0)
        {
            // unset the bool
            isLaserOn = !isLaserOn;
            // set/unset the collider
            laserCollider.enabled = isLaserOn;
            // change the sprite due to bool flag
            if (isLaserOn)
            {
                laserRenderer.sprite = laserOnSprite;
            }
            else
            {
                laserRenderer.sprite = laserOffSprite;
            }
            // reset the timer
            timeUntilNextToggle = toggleInterval;
        }
        // rotate around axis with the positive is relative to counter clockwise
        transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);

    }

}
