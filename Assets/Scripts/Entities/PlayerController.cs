using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerController
// Last Updated: Sept 29 2020
// Mark Colling
// Use to manage player behaviours
public class PlayerController : MonoBehaviour
{
    // These are public fields that we edit in the inspector 
    [Header("Player Settings")]
    public float moveSpeed;                     // How fast the player moves
    public float DashDuration;                  // How long their dash lasts
    public float DashSpeed;                     // How fast the dash is

    public GameObject bullet;                   // The gameobject for what is fired. NOTE: we'll probablly want to change this

    // Internal variables
    float dashTimeLeft;                         // How long the player has left to dash
    bool dashing = false;                       // Flag for if the player is dashing
    int keyCount;                               // How many keys they have
    CharacterController characterController;    // The controller for handling collison and movment

    //Stores Movement in Update and applies in FixedUpdate (This is how it should be done for physics based movement)
    private Vector3 movement;

    #region Getters and Setters
    // AddKey
    // Adds key to the keyCount
    // Parama:  int k,  amount of keys
    public void AddKey(int k)
    {
        keyCount += k;
    }
    #endregion

    // Start
    // Runs once the object is loaded in
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update
    // Runs every frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));   
        MouseLook();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
        } 
    }



    void FixedUpdate() 
    {
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    // Dash
    // handles dashing logic
    void Dash()
    {        
        // Check if the dash button has been pressed
        if (Input.GetKeyDown(KeyCode.LeftControl) && !dashing)
        {
            dashTimeLeft = DashDuration;
            dashing = true;            
        }

        // Runs if dashing
        if (dashing) {
            if (dashTimeLeft < 0)
            {
                dashTimeLeft = 0;
                dashing = false;
            }
            else
            {
                dashTimeLeft -= 1 * Time.deltaTime;
            }
            
            characterController.Move(characterController.transform.forward * DashSpeed * Time.deltaTime);            
        }
    }

    // Shoot
    // Handles shooting logic
    void Shoot()
    {
        Instantiate(bullet,
            characterController.transform.position + characterController.transform.forward, 
            characterController.transform.rotation,
            null);
    }

    // MouseLook
    // Handles player looking logic
    void MouseLook()
    {
        Plane floorTarget = new Plane(Vector3.up, this.transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float raycastOut;
        if (floorTarget.Raycast(ray, out raycastOut))
        {
            this.transform.LookAt(ray.GetPoint(raycastOut));
        }
    }

    // OnControllerColliderHit
    // Runs when another object his this one
    // Parama:	ControllerColliderHit hit:		The other collider that touched this object
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Check if door
        if (hit.gameObject.CompareTag("Door"))
        {
            Door door = hit.gameObject.GetComponent<Door>();
            if (keyCount >= door.KeyRequirement())    // Check if player has enough keys
            {
                keyCount -= door.keyRequirement;
                door.OpenDoor();
            }
        }
    }
}
