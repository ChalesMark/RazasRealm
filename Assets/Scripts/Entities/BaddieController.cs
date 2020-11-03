using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BaddieController : MonoBehaviour
{
    GameObject player;
    public GameObject blood;

    private Vector3 target;
    private bool playerSpotted;
    private float distanceToTarget;

    [Header("Movement AI Settings:")]
    [Tooltip("Max distance an enemy can roam in a single direction at a time")]
    [Range(5.0f, 40.0f)]
    public int RoamDistance = 10;
    [Tooltip("Distance the enemy can spot the player from")]
    [Range(0.0f, 40.0f)]
    public int AggroRange = 10;
    [Tooltip("Distance the player must be from the enemy to resume free roam")]
    [Range(0.0f, 40.0f)]
    public int IgnoreDistance = 20;
    [Tooltip("Speed the enemy travels")]
    [Range(0.0f, 10.0f)]
    public int Speed = 1;

    private void Start()
    {
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
            FollowPlayer();
        else
            Roam();

        MoveForward();
    }

    public void Bleed()
    {
        Instantiate(blood, transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));
    }

    void GetRandomTarget()
    {
        target = new Vector3(Random.Range(transform.position.x - RoamDistance, transform.position.x + RoamDistance), 
                             transform.position.y, 
                             Random.Range(transform.position.z - RoamDistance, transform.position.z + RoamDistance));
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
}
