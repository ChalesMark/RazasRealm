using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// SpawnPoint
// Last Updated: Oct 31 2020
// Mark Colling
// Use to bring the player to this position when the scene loads.
public class SpawnPoint : MonoBehaviour
{
    public string spawnName;
    Vector3 location;

    private void Start()
    {
        location = this.transform.position;
    }

    public Vector3 GetLocation()
    {
        return location;
    }
}
