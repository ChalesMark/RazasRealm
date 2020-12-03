using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// LevelPortal
// Last Updated: Oct 31 2020
// Mark Colling
// Use to load scenes when the player enters it's trigger zone
public class LevelPortal : MonoBehaviour
{
    // These are public fields that we edit in the inspector 
    [Header("Enter the scene's name here. NOTE: the scene must be added to the build list to be loadable")]
    public string scene;
    [Header("Enter the scene's spawn point you want to teleport to")]
    public string spawnPointName;
    public AudioClip teleportSound;

    // OnTriggerEnter
    // Is called when a collider enters. Used to check if the player touches it
    // Parama:	Collider other:		The other collider that touched this object
    void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Player")
        {
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.GetComponent<AudioSource>().PlayOneShot(teleportSound, 0.3f);
            gm.GetPlayer().enabled = false;
            gm.LoadScene(scene, spawnPointName);            
        }
    }
}
