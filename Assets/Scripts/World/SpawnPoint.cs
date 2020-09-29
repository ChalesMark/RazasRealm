using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SpawnPoint
// Last Updated: Sept 29 2020
// Mark Colling
// Use to bring the player to this position when the scene loads.
public class SpawnPoint : MonoBehaviour
{
    // Awake
    // Runs once all gameobjects are loaded in
    void Awake()
    {
        
        // Reenables both smooth camera movement (so it follows the player) and the player controller
        Camera.main.GetComponent<CameraController>().SetSmoothCameraMovement(true);
        StartCoroutine(MovePlayer());

        // fade back to screen
        StartCoroutine(Camera.main.GetComponent<CameraController>().FadeToScreen());       
    }
    
    // MovePlayer
    // Causes the game to wait until the player is moved.
    // I had to do it this way as unity runs async so the player may be moved but then forced to a different position.
    // So thus, the game is forced to wait. It doesn't wait too long, but enough time so that the player is moved.
    IEnumerator MovePlayer()
    {
        PlayerController player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayer();

        while (Vector3.Distance(player.transform.position,this.transform.position) > .1)
        {
            player.transform.position = this.transform.position;
            yield return null;
        }

        player.enabled = true;
    }
}
