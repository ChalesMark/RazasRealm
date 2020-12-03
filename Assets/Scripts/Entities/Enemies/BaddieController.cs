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
    public List<GameObject> lootDrops;

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
    private bool attackOnCooldown = false;

    private CharacterController characterController;

    [Header("This toggle makes the enemy always ignore the player (mainly for testing purposes)")]
    public bool blind;

    int IEnemyController.KillGold { get { return killGold; } }


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!agent)
        {
            characterController = GetComponent<CharacterController>();
        }
        else
        {
            agent.speed = this.Speed;
        }

        animator = GetComponent<Animator>();
        animator.SetBool("walk",true);
    }

    // Update is called once per frame
    void Update()
    {
        if(player == null)
            player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayerObject();

        if (agent && !blind)
            NavToPlayer();
    }

    public void Bleed()
    {
        Instantiate(blood, transform.position, Quaternion.LookRotation(Camera.main.transform.position - transform.position));
    }

    void NavToPlayer()
    {
        agent.destination = player.transform.position;
    }

    public void DropLoot()
    {
        if(lootDrops.Count != 0)
        {
            int lootRoll = Random.Range(0, 10);
            if (lootRoll < lootDrops.Count)
                Instantiate(lootDrops[lootRoll], transform.position, transform.rotation);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !attackOnCooldown)
        {
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
