using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    #region DATA
    SheepManager sm;                        // SheepManager instance
    Dog dog;                                // Dog instance
    DataManager data;                       // DataManager instance

    public Vector2 sheepRepulsion;          // Sheep Repulsion Speed
    float sheepSpeed;                        // Minimum Vertical Sheep Speed
    float distance;                         // Distance from Dog
    float angle;                            // Angle from Dog
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
    }


    // Update is called once per frame
    void Update()
    {
        // Get distance & angle from dog
        distance = Vector2.Distance(transform.position, dog.transform.position);
        angle = Mathf.Atan2(dog.transform.position.y - transform.position.y, 
                            dog.transform.position.x - transform.position.x);

        // Move sheep up slightly
        transform.position = new Vector3(   transform.position.x - (sheepRepulsion.x * (1/distance) * Mathf.Cos(angle) * Time.deltaTime),
                                            transform.position.y + (sheepSpeed * Time.deltaTime)
                                                                - (sheepRepulsion.y * (1 / distance) * Mathf.Sin(angle) * Time.deltaTime),
                                            transform.position.z);

        // Check for death
        if (transform.position.y < sm.sheepDeathPos)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        sm.sheepCount -= 1;
    }
    #endregion
}
