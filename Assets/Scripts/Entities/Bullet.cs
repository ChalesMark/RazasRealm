using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullet
// Last Updated: Sept 29 2020
// Mark Colling
// Code for handling projectiles
public class Bullet : MonoBehaviour
{
    // These are public fields that we edit in the inspector 
    [Header("Bullet Settings")]
    public float lifeSpan;
    public float moveSpeed;

    // Update
    // Runs every frame
    void Update()
    {
        transform.position += transform.forward * moveSpeed* Time.deltaTime;

        lifeSpan -= 1 * Time.deltaTime;

        if (lifeSpan <= 0)
            Destroy(this.gameObject);
    }

    // OnTriggerEnter
    // Is called when a collider enters. Used to check if a monster touches
    // Parama:	Collider other:		The other collider that touched this object
    void OnTriggerEnter(Collider other)
    {
        print(other);
        if (other.tag == "Enemy")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
