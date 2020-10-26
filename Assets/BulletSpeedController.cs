using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BulletSpeedController : MonoBehaviour
{
    public float Speed;

    // Update
    // Runs every frame
    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;   
    }


}