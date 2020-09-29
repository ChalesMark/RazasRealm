using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Key
// Last Updated: Sept 29 2020
// Mark Colling
// Use to store key behaviours
public class Key : MonoBehaviour
{
    // OnTriggerEnter
    // Is called when a collider enters. Used to check if the player touches it
    // Parama:	Collider other:		The other collider that touched this object
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().AddKey(1);
            Destroy(this.gameObject);
        }
    }
}
