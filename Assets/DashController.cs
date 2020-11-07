using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashController : MonoBehaviour
{

    public float distanceModifier;              //The distance you will travel upon release

    private PlayerController playerController;
    private Animator animator;
    private CharacterController characterController;
    private KeyboardControls keyboardControls;

    //TEMP VALUES
    public int dashType = 0;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        animator.SetInteger("DashType", dashType);
        keyboardControls = GetComponent<KeyboardControls>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("DashCharging", Input.GetKeyDown(keyboardControls.meleeKey));
        if(animator.GetBool("DashCharging")) {
            playerController.LockMovement(true);
            playerController.LockShooting(true);
        }
        animator.SetBool("Dashing", Input.GetKeyUp(keyboardControls.meleeKey));
        if(animator.GetBool("Dashing")) 
        {
            characterController.Move(transform.forward * playerController.currMoveSpeed * distanceModifier * Time.deltaTime);
            playerController.LockMovement(false);
            playerController.LockShooting(false);
        }
    }
}
