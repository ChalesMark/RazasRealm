using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class BaddieController : MonoBehaviour, IEnemyController
{
    
    public GameObject blood;
    public TextMeshPro damageNumbers;
    public int killGold;
    NavMeshAgent agent;

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

    [Header("Attack Settings:")]
    [Range(0.0f, 10.0f)]
    public float AttackCooldownTime = 3;
    [Range(0.0f, 50.0f)]
    public int Damage = 1;

    private GameObject player;
    private Animator animator;
    private Vector3 target;
    private float distanceToTarget;
    private bool playerSpotted;
    private bool attackOnCooldown = false;

    //Roam boundary square
    private float roamXBottom;
    private float roamXTop;
    private float roamZBottom;
    private float roamZTop;

    private CharacterController characterController;

    [Header("This toggle makes the enemy always ignore the player (mainly for testing purposes)")]
    public bool blind;

    int IEnemyController.KillGold { get { return killGold; } }
    bool IEnemyController.PlayerSpotted { get { return playerSpotted; } }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!agent)
        {
            characterController = GetComponent<CharacterController>();
            roamXBottom = transform.position.x - RoamDistance;
            roamXTop = transform.position.x + RoamDistance;
            roamZBottom = transform.position.z - RoamDistance;
            roamZTop = transform.position.z + RoamDistance;
        }
        else
        {
            agent.speed = this.Speed;
        }

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

        if (agent)
        {
            if (!blind && playerSpotted)
                NavToPlayer();
            else
                NavRoam();
        }
        else
        {
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

    void NavGetRandomTarget()
    {
        do {
            target = new Vector3(Random.Range(roamXBottom, roamXTop),
                                 transform.position.y,
                                 Random.Range(roamZBottom, roamZTop));
        } while (agent.CalculatePath(target, new NavMeshPath()));
    }

    void MoveForward()
    {
        //transform.position += transform.TransformDirection(Vector3.forward) * Speed * Time.deltaTime;
        characterController.Move(transform.forward * Speed * Time.deltaTime);
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

    void NavToPlayer()
    {
        if ((player.transform.position - this.transform.position).magnitude < IgnoreDistance)
        {
            agent.destination = player.transform.position;
        }
        else
        {
            playerSpotted = false;
            GetRandomTarget();
        }
    }

    public void NavRoam()
    {
        agent.destination = target;
        if (distanceToTarget < 3)
            NavGetRandomTarget();
    }

    public void Roam()
    {
        if (distanceToTarget < 3)
            GetRandomTarget();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !attackOnCooldown)
        {
            print("TRIGGER STAY" + name + " " + other.gameObject.name);
            other.gameObject.GetComponent<HealthController>().DecreaseCurrentHealth(Damage);
            damageNumbers.text = Damage.ToString();
            Instantiate(damageNumbers, transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - transform.position));
            AttackCooldown();
        }

    }

    private void AttackCooldown()
    {
        attackOnCooldown = true;
        Invoke("AttackOffCooldown", AttackCooldownTime);

    }

    private void AttackOffCooldown()
    {
        attackOnCooldown = false;
    }
    
}
