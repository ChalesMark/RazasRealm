using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// LevelPortal
// Last Updated: Sept 29 2020
// Mark Colling
// Use to load scenes when the player enters it's trigger zone
public class LevelPortal : MonoBehaviour
{
    // These are public fields that we edit in the inspector 
    [Header("Enter the scene's name here. NOTE: the scene must be added to the build list to be loadable")]
    public string scene;

    // OnTriggerEnter
    // Is called when a collider enters. Used to check if the player touches it
    // Parama:	Collider other:		The other collider that touched this object
    void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Player")
        {
            // Disable smooth camera movement (so the camera snaps to the new location)
            // And disables the player controller
            Camera.main.GetComponent<CameraController>().SetSmoothCameraMovement(false);
            PlayerController player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();
            player.enabled = false;
            // Fade to Black
            StartCoroutine(Camera.main.GetComponent<CameraController>().FadeToBlack());
            // Then load scene
            GameObject.Find("GameManager").GetComponent<GameManager>().LoadScene(scene);
        }
    }
}
