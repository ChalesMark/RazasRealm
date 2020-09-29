using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Door
// Last Updated: Sept 29 2020
// Mark Colling
// Used to handle door behaviours
public class Door : MonoBehaviour {

    // These are public fields that we edit in the inspector 
    [Header("How many keys are needed")]
    public int keyRequirement;

    // OpenDoor
    // Opens the door. NOTE: Currently just destorys it. Will add animation later
    public void OpenDoor()
    {
        Destroy(this.gameObject);
    }

    // KeyRequirement
    // Returns how many keys are needed
    // Return:  int
    public int KeyRequirement()
    {
        return keyRequirement;
    }
}
