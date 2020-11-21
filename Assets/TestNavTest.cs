using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavTest : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject player;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
            player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayerObject();
        if (agent && player)
            agent.destination = player.transform.position;
    }
}
