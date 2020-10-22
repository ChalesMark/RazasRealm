using Assets.Scripts.Entities;
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

    public GameObject startingGun;              // The Gun the player is holding at the start

    // Internal variables
    float dashTimeLeft;                         // How long the player has left to dash
    bool dashing = false;                       // Flag for if the player is dashing
    int keyCount;                               // How many keys they have
    CharacterController characterController;    // The controller for handling collison and movment
    Animator animator;
    
    // Shooting Variables
    Gun gunData;
    public GameObject heldGun;
    public Transform gunBone;
    float fireIntervals = 0;

    //Stores Movement in Update and applies in FixedUpdate (This is how it should be done for physics based movement)
    private Vector3 movement;

    public KeyCode FireKey = KeyCode.Mouse0;

    UnityEngine.Random random;

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
        PickupGun(startingGun);
        UpdateGunData();

        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        random = new UnityEngine.Random();       
    }

    void UpdateGunData()
    {
        print("FD");
        gunData = heldGun.GetComponent<Gun>();
    }

    public void PickupGun (GameObject gun)
    {
        //Destroy(heldGun.gameObject);
        //heldGun = Instantiate(gun);
        heldGun = gun;
        UpdateGunData();
    }

    // Update
    // Runs every frame
    void Update()
    {
        WASD();
        MouseLook();
        if (gunData.autoFire)
        {
            if (fireIntervals > 0)
                fireIntervals -= Time.deltaTime;
            if (fireIntervals <= 0)
                fireIntervals = 0;

            if (Input.GetKey(FireKey))
            {
                if (fireIntervals <= 0)
                    Shoot();
            }
        }
        else
            if (Input.GetKeyDown(FireKey))
        {
            Shoot();
        } 
    }

    /*
    private void FixedUpdate()
    {
        if (heldGun != null)
        {
            heldGun.transform.position = gunBone.transform.position;
            heldGun.transform.rotation = gunBone.transform.rotation;
        }
    }
    */

    void WASD() 
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        animator.SetBool("Moving", movement == Vector3.zero ? false : true);

        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    // Shoot
    // Handles shooting logic
    void Shoot()
    {
        animator.SetTrigger("Shoot");
        fireIntervals = gunData.firingSpeed;

        float randomSpread = UnityEngine.Random.Range(-gunData.bulletSpread, gunData.bulletSpread);

        for(int i=0;i<gunData.numberOfBullets;i++)
        Instantiate(gunData.bullet,
            characterController.transform.position + characterController.transform.forward,
            characterController.transform.rotation * Quaternion.Euler(0, randomSpread,0),
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
