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
    public float baseMoveSpeed;
    public float currMoveSpeed;                // How fast the player moves
    public float DashDuration;                  // How long their dash lasts
    public float DashSpeed;                     // How fast the dash is

    private bool lockShooting = false;
    private bool lockMovement = false;

    public GameObject startingGun;              // The Gun the player is holding at the start
    public GameObject startingSword;            // The Gun the player is holding at the start

    // Internal variables
    float dashTimeLeft;                         // How long the player has left to dash
    bool dashing = false;                       // Flag for if the player is dashing
    int keyCount;                               // How many keys they have
    CharacterController characterController;    // The controller for handling collison and movment
    CameraController camera;
    Animator animator;
    GameObject ground;
    
    // Shooting Variables
    Gun gunData;
    public GameObject heldGun;
    public Transform gunBone;
    float fireIntervals = 0;

    // Sword Stuff
    //Sword swordData;
    public GameObject heldSword;

    // Cosmetic Stuff
    public GameObject startingHat;
    GameObject hat;
    public Transform hatBone;

    //Stores Movement in Update and applies in FixedUpdate (This is how it should be done for physics based movement)
    private Vector3 movement;

    public KeyCode fireKey = KeyCode.Mouse0;
    public KeyCode actionKey = KeyCode.F;

    GameObject currentlyLookingAt;

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
        camera = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        ground = GameObject.Find("Plane");
        random = new UnityEngine.Random();

        PickupGun(startingGun);
        PickupSword(startingSword);
        PickupHat(startingHat);
        currMoveSpeed = baseMoveSpeed;
        heldSword.GetComponentsInChildren<Renderer>()[0].enabled = false;
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

    public void PickupSword(GameObject sword)
    {
        print("Picked up Sword");
        if (heldSword != null)
            Destroy(heldSword.gameObject);

        heldSword = Instantiate(sword);
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
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Interactable();
        Weapons();  
    }

    private void Weapons()
    {        
        if(!lockShooting) {
            if (gunData != null && gunData.autoFire)
            {
                if (fireIntervals > 0)
                    fireIntervals -= Time.deltaTime;
                if (fireIntervals <= 0)
                    fireIntervals = 0;

                if (Input.GetKey(fireKey))
                {
                    if (fireIntervals <= 0)
                        Shoot();
                }
            }
            else if (Input.GetKeyDown(fireKey))
            {
                Shoot();
            }
            else
            {
                if (animator.GetCurrentAnimatorStateInfo(1).IsName("player_armature|null"))
                {
                    heldGun.GetComponentsInChildren<Renderer>()[0].enabled = true;
                    heldSword.GetComponentsInChildren<Renderer>()[0].enabled = false;
                }
            }
            if (animator.GetCurrentAnimatorStateInfo(1).IsName("player_armature|null"))
            {
                animator.SetLayerWeight(1, 0);
            }
            else
            {
                animator.SetLayerWeight(1, 1);
            }
        }
    }

    private void Interactable()
    {
        RaycastHit hit;
        if (Physics.Raycast(
            new Vector3(this.transform.position.x, 0.5f, this.transform.position.z),
            this.transform.forward,
            out hit,
            3f
            ))
        {
            currentlyLookingAt = hit.transform.gameObject;
        }
        else
        {
            currentlyLookingAt = null;
        }

        if (Input.GetKey(actionKey) && currentlyLookingAt != null)
        {
            if (currentlyLookingAt.GetComponent<Talkable>() != null)
            {
                Talkable t = currentlyLookingAt.GetComponent<Talkable>();
                camera.ShowMessage(t.text,t.speaker, t.readTime,t.showChatBox);
            }
        }
    }

    private void FixedUpdate()
    {
        WASD();
        MouseLook();
        if (heldSword != null)
        {
            heldSword.transform.position = gunBone.position;
            heldSword.transform.rotation = gunBone.rotation;
        }
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
        if(!lockMovement)
        {
            animator.SetBool("moving", movement == Vector3.zero ? false : true);
            characterController.Move(movement * currMoveSpeed * Time.deltaTime);
        }
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

    public void LockShooting(bool lockShoot) 
    {
        lockShooting = lockShoot;
    }

    public void LockMovement(bool lockMove) {
        lockMovement = lockMove;
    }

}
