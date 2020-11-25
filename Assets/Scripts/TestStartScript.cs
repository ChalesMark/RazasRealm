using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStartScript : MonoBehaviour, IInteractable
{
    
    public bool AutoInteract { get; set; } = false;
    public GameObject waveManager;

    private bool started = false;

    public string GetInteractText()
    {
        if (started)
            return "Interact to restart";
        else
            return "Interact to start Wave 1";
    }

    public void Interact(InteractController interactController)
    {
        if (started)
        {
            waveManager.GetComponent<WaveManager>().Restart();
            started = false;
        }
        else
        {
            waveManager.GetComponent<WaveManager>().StartFirstWave();
            started = true;
        }
        interactController.RefreshText(gameObject);
    }
}
