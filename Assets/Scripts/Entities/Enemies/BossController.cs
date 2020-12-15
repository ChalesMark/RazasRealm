using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject dropItem;
    
    public void DropLoot()
    {
        if (!GameObject.Find("WaveManager").GetComponent<WaveManager>().GetKeyDropped())
        {
            Instantiate(dropItem, transform.position, transform.rotation);
            GameObject.Find("WaveManager").GetComponent<WaveManager>().SetKeyDropped();
        }    
        else
            GetComponent<BaddieController>().DropAllLoot();
    }
}
