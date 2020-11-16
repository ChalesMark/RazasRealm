using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingDamageNumbers : MonoBehaviour
{

    public GameObject damageNumbers;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {      
        print("COLLIDED");
        Instantiate(damageNumbers, other.transform.position , Quaternion.LookRotation(Camera.main.transform.position - this.transform.position)); 
    }
}

