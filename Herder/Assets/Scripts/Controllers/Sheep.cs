using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    #region DATA
    SheepManager sm;                        // SheepManager instance
    Dog dog;                                // Dog instance
    DataManager data;                       // DataManager instance
    SpriteRenderer fade;                    // SpriteManager Reference rUsed for Fading Sheep
    SoundManager soundManager;              // SoundManager Reference
    public AudioClip sheepSpawnSound;       // Sheep Spawn Sound
    public AudioClip sheepDrownSound;       // Sheep Drown Sound

    public Vector2 sheepRepulsion;          // Sheep Repulsion Speed
    float sheepSpeed;                        // Minimum Vertical Sheep Speed
    float distance;                         // Distance from Dog
    float angle;                            // Angle from Dog 
    float sheepRadius = 5.0f;               // Radius of Effect                  
    #endregion

    #region MAIN
    // Start is called before the first frame update
    void Start()
    {
        print("sheep");

        // Sheep Manager Reference
        sm = SheepManager.instance;

        // Dog Reference
        dog = Dog.instance;

        // Data Reference
        data = Singleton.instance.data;

        // Update Count
        sm.sheepCount += 1;

        // SoundManager Reference
        soundManager = Singleton.instance.sound;

        // Used for Fading
        fade= GetComponent<SpriteRenderer>();

        // Play Sheep Spawn Sound
        soundManager.PlayAudioClip(sheepSpawnSound);
    }

    // Update is called once per frame
    void Update()
    {
        // Get distance & angle from dog
        distance = Vector2.Distance(transform.position, dog.transform.position);
        angle = Mathf.Atan2(dog.transform.position.y - transform.position.y, 
                            dog.transform.position.x - transform.position.x);


        //Move sheep up slightly
        // transform.position = new Vector3(   transform.position.x - (sheepRepulsion.x * (1/distance) * Mathf.Cos(angle) * Time.deltaTime),
        //                                     transform.position.y + (sheepSpeed * Time.deltaTime)
        //                                                         - (sheepRepulsion.y * (1 / distance) * Mathf.Sin(angle) * Time.deltaTime),
        //                                     transform.position.z);

        if (Vector2.Distance ( dog.transform.position, transform.position) < sheepRadius){
            transform.position = new Vector3(   transform.position.x - (sheepRepulsion.x * (1/distance) * Mathf.Cos(angle) * Time.deltaTime),
                                            transform.position.y + (sheepSpeed * Time.deltaTime)
                                                                - (sheepRepulsion.y * (1 / distance) * Mathf.Sin(angle) * Time.deltaTime),
                                            transform.position.z);
        } 

        // Rotate Sheep towards where it moves
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle + 90);

        // Check for death
        if (transform.position.y < sm.sheepDeathPos)
        {
            Destroy(this.gameObject);
        }
    }

    // When sheep is destroyed, reduce sheepCount
    private void OnDestroy()
    {
        sm.sheepCount -= 1;
    }

    // Handle Sheep's collision
    private void OnCollisionEnter2D(Collision2D collision){
        //If Sheep Collides with Lake, Make the Sheep Fade then Destroy itself
        if (collision.gameObject.tag == "Lake"){
            sheepSpeed = 0;                                     // Reduce sheep speed
            soundManager.PlayAudioClip(sheepDrownSound);        // Play Drowning Sound
            StartCoroutine("FadeSheepSpriteInLake");            // Call IEnumerator
        }
    }
       
    //Allows for timing the fading/destruction of sheep.
   IEnumerator FadeSheepSpriteInLake(){
       // Fade sheep a bit every 0.05 seconds
       for (float f =1f; f > 0f; f-= 0.05f){
           Color c = fade.material.color;
           c.a = f;                                         // Changing alpha (transparency)
           fade.material.color = c;
           yield return new WaitForSeconds(0.05f);
       }
    
    }
    #endregion
}

