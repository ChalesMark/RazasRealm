﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaddieController : MonoBehaviour, IEnemyController
{
    private GameObject player;
    public GameObject blood;
    public int killGold;
    private Vector3 target;
    private bool playerSpotted;
    private float distanceToTarget;

    [Header("Movement AI Settings:")]
    [Tooltip("Max distance an enemy can roam in any direction from it's spawm")]
    [Range(5.0f, 100.0f)]
    public int RoamDistance = 10;
    [Tooltip("Distance the enemy can spot the player from")]
    [Range(0.0f, 40.0f)]
    public int AggroRange = 10;
    [Tooltip("Distance the player must be from the enemy to resume free roam")]
    [Range(0.0f, 100.0f)]
    public int IgnoreDistance = 20;
    [Tooltip("Speed the enemy travels")]
    [Range(0.0f, 10.0f)]
    public int Speed = 1;

    Animator animator;

    //Roam boundary square
    private float roamXBottom;
    private float roamXTop;
    private float roamZBottom;
    private float roamZTop;

    int IEnemyController.KillGold { get { return killGold; } }
    bool IEnemyController.PlayerSpotted { get { return playerSpotted; } }


    private void Start()
    {
        roamXBottom = transform.position.x - RoamDistance;
        roamXTop = transform.position.x + RoamDistance;
        roamZBottom = transform.position.z - RoamDistance;
        roamZTop = transform.position.z + RoamDistance;

        animator = GetComponent<Animator>();
        animator.SetBool("walk",true);
        playerSpotted = false;
        GetRandomTarget();
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
            player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayerObject();

        distanceToTarget = (transform.position - target).magnitude;

        ScanForPlayer();

        if (playerSpotted)
        {
            //animator.SetFloat("AgroBlend", 0f);
            FollowPlayer();
        }
        else
        {
            //animator.SetFloat("AgroBlend", 1f);
            Roam();
        }

        MoveForward();
    }

    public void Bleed()
    {
        Instantiate(blood, transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));
    }


    void GetRandomTarget()
    {
        target = new Vector3(Random.Range(roamXBottom, roamXTop), 
                             transform.position.y, 
                             Random.Range(roamZBottom, roamZTop));
        transform.LookAt(target);
    }

    void MoveForward()
    {
        transform.position += transform.TransformDirection(Vector3.forward) * Speed * Time.deltaTime;
    }

    void ScanForPlayer()
    {
        if ((player.transform.position - this.transform.position).magnitude < AggroRange)
        {
            playerSpotted = true;
        }
    }

    void FollowPlayer()
    {
        if ((player.transform.position - this.transform.position).magnitude < IgnoreDistance)
        {
            target = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
            this.transform.LookAt(target);
        }
        else
        {
            playerSpotted = false;
            GetRandomTarget();
        }
    }
    public void Roam()
    {
        if (distanceToTarget < 3)
            GetRandomTarget();
    }

    private void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag != "Player"  && !playerSpotted)
            GetRandomTarget();
        else if(coll.gameObject.tag == "Player")
        {
            //Damage player
        }
    }
}
