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
    public bool titleScreen;

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
        animator.SetBool("walk", true);
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
        if(player != null) {
            agent.destination = player.transform.position;
        }
    }

    public void DropLoot()
    {
        if(lootDrops.Count != 0)
        {
            int lootRoll = Random.Range(0, 10);
            if (lootRoll < lootDrops.Count)
                Instantiate(lootDrops[lootRoll], transform.position, transform.rotation);
        }
        if (titleScreen)
            TitleScreenRespawn();
    }

    public void DropGuaranteedLoot()
    {
        if(lootDrops.Count != 0)
        {
            int lootRoll = Random.Range(0, lootDrops.Count - 1);
            Instantiate(lootDrops[lootRoll], transform.position, transform.rotation);
        }
    }

    public void DropAllLoot()
    {
        if (lootDrops.Count != 0)
        {
            foreach (GameObject loot in lootDrops)
            {
                Vector3 spawnOffset = new Vector3(Random.Range(-2.0f, 2.0f), 
                                                  0, 
                                                  Random.Range(-2.0f, 2.0f));
                Instantiate(loot, transform.position + spawnOffset, transform.rotation);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && !attackOnCooldown)
        {
            other.gameObject.GetComponent<HealthController>().DecreaseCurrentHealth(Damage);
            damageNumbers.text = Damage.ToString();
            Instantiate(damageNumbers, other.transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)), Quaternion.LookRotation(Camera.main.transform.position - other.transform.position));
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
    
    private void TitleScreenRespawn()
    {
        if(GameObject.Find("TitleSpawnGroup"))
        {
            List<Transform> spawns = new List<Transform>();
            foreach (Transform spawn in GameObject.Find("TitleSpawnGroup").transform)
                spawns.Add(spawn);

            Transform spawnPoint = spawns[Random.Range(0, spawns.Count)];
            Quaternion rotation = new Quaternion(spawnPoint.rotation.x, Random.Range(0f, 360f), spawnPoint.rotation.z, spawnPoint.rotation.w);
            Instantiate(gameObject, spawnPoint.position, rotation);
        }
    }
}
