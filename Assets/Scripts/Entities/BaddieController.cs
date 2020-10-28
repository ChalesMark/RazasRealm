using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaddieController : MonoBehaviour
{
    GameObject player;

    // Update is called once per frame
    void Update()
    {
        if(player == null)
            player = GameObject.Find("GameManager").GetComponent<GameManager>().GetPlayerObject();

        Vector3 newVect = new Vector3(player.transform.position.x,  this.transform.position.y, player.transform.position.z);
        this.transform.LookAt(newVect);
    }
}
