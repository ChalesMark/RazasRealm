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
    public float currMoveSpeed;                // How fast the player moves
    private bool lockMovement = false;

    CharacterController characterController;    // The controller for handling collison and movment
    GearController gearController;
    Animator animator;

    //Stores Movement in Update and applies in FixedUpdate (This is how it should be done for physics based movement)
    private Vector3 movement;

    private GameObject currentlyLookingAt;
    public KeyCode actionKey = KeyCode.F;

    // Start
    // Runs once the object is loaded in
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        gearController = GetComponent<GearController>();
        animator = GetComponent<Animator>();
    }
    
    public void DecreaseSpeed(int speed) {
        currMoveSpeed -= speed;
    }

    public void IncreaseSpeed(int speed) {
        currMoveSpeed += speed;
    }


    // Update
    // Runs every frame
    void Update()
    {
        movement = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        MouseLook();
        //RayTraceInteract();

        if (animator.GetCurrentAnimatorStateInfo(1).IsName("player_armature|null"))
        {
            animator.SetLayerWeight(1, 0);
        }
        else
        {
            animator.SetLayerWeight(1, 1);
        }
    }

    private void FixedUpdate()
    {
        WASD();
    }
    

    void WASD() 
    {
        if(!lockMovement)
        {
            animator.SetBool("moving", movement == Vector3.zero ? false : true);
            characterController.Move(movement * currMoveSpeed * Time.deltaTime);
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

    public void LockMovement(bool lockMove) {
        lockMovement = lockMove;
    }
    
    /*
    // DO NOT DELETE THIS
    private void RayTraceInteract()
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

        if (Input.GetKeyDown(actionKey) && currentlyLookingAt != null)
        {

            if (currentlyLookingAt.GetComponent<MarksIInteractable>() != null)
            {
                currentlyLookingAt.GetComponent<MarksIInteractable>().Interact();           
            }
        }
    }
    */

    internal void StopAnimation()
    {
        animator.SetBool("moving", false);
    }

    internal void DisableGun(bool disable)
    {
        gearController.weapon.enabled = disable;
    }
}
