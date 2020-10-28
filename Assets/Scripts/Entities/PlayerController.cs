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
    GameObject ground;
    
    // Shooting Variables
    Gun gunData;
    public GameObject heldGun;
    public Transform gunBone;
    float fireIntervals = 0;

    // Cosmetic Stuff
    public GameObject startingHat;
    GameObject hat;
    public Transform hatBone;

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
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        ground = GameObject.Find("Plane");
        random = new UnityEngine.Random();

        PickupGun(startingGun);
        PickupHat(startingHat);
    }

    void UpdateGunData()
    {
        gunData = heldGun.GetComponent<Gun>();
        switch (gunData.AnimationType)
        {
            case Gun.ShootType.Normal:
                animator.SetInteger("shootType", 0);
                break;
            case Gun.ShootType.MachineGun:
                animator.SetInteger("shootType", 1);
                break;
            case Gun.ShootType.Steady:
                animator.SetInteger("shootType", 2);
                break;
            case Gun.ShootType.Big:
                animator.SetInteger("shootType", 3);
                break;
        }
        
    }

    public void PickupGun (GameObject gun)
    {
        print("Picked up Gun");
        if (heldGun != null)
            Destroy(heldGun.gameObject);

        heldGun = Instantiate(gun);
        heldGun.GetComponent<BoxCollider>().enabled = false;
        //heldGun.GetComponent<Gun>().AssignOwner(this.playerName);
        UpdateGunData();
    }
    public void PickupHat(GameObject hat)
    {
        if (this.hat != null)
            Destroy(this.hat.gameObject);
        this.hat = Instantiate(hat);
    }

    // Update
    // Runs every frame
    void Update()
    {        
        if (gunData != null && gunData.autoFire)
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
        else if (Input.GetKeyDown(FireKey))
        {
            Shoot();
        }
    }

    
    private void FixedUpdate()
    {
        if (animator != null)
            WASD();
        MouseLook();
        if (heldGun != null)
        {
            heldGun.transform.position = gunBone.position;
            heldGun.transform.rotation = gunBone.rotation;
        }
        if (hat != null)
        {
            hat.transform.position = hatBone.position;
            hat.transform.rotation = hatBone.rotation;
        }
    }
    

    void WASD() 
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        animator.SetBool("moving", movement == Vector3.zero ? false : true);
        characterController.Move(movement * moveSpeed * Time.deltaTime);
    }

    // Shoot
    // Handles shooting logic
    void Shoot()
    {
        animator.SetTrigger("shoot");
        fireIntervals = gunData.firingSpeed;

        float randomSpread;

        for (int i = 0; i < gunData.numberOfBullets; i++)
        {
            randomSpread = UnityEngine.Random.Range(-gunData.bulletSpread, gunData.bulletSpread);
            Instantiate(gunData.bullet,
                heldGun.transform.position + characterController.transform.forward,
                characterController.transform.rotation * Quaternion.Euler(0, randomSpread, 0),
                null);
        }
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
