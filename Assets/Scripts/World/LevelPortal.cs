using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

// LevelPortal
// Last Updated: Oct 31 2020
// Mark Colling
// Use to load scenes when the player enters it's trigger zone
public class LevelPortal : MonoBehaviour, IInteractable
{
    // These are public fields that we edit in the inspector 
    [Header("Enter the scene's name here. NOTE: the scene must be added to the build list to be loadable")]
    public string scene;
    [Header("Enter the scene's spawn point you want to teleport to")]
    public string spawnPointName;
    public AudioClip teleportSound;
    public bool active;
    public int activatePrice;
    [Header("Leave this at 0 if it is not a portal to enter a level")]
    public int levelNum = 0;



    private VisualEffect portalEffect;
    private bool autoInteract = false;

    public bool AutoInteract { get => autoInteract; set => autoInteract = value; }

    public string GetInteractText()
    {
        if (!active)
            return "Interact to unlock level for $" + activatePrice + ".00";
        else
            return "";
    }

    public void Interact(InteractController interactController)
    {
        if(!active && Buy(interactController.GetComponent<MoneyController>()))
        {
            portalEffect.enabled = true;
            active = true;
            PlayerPrefs.SetInt("Level" + levelNum + "Unlocked", 1);
        }
    }

    public bool Buy(MoneyController controller)
    {
        if (activatePrice <= controller.GetMoney())
        {
            controller.DeductMoney(activatePrice);
            return true;
        }
        else
            return false;
    }

    public void Start()
    {
        portalEffect = transform.Find("PortalVFX").GetComponent<VisualEffect>();
        if (PlayerPrefs.GetInt("Level" + levelNum + "Unlocked") == 1)
            active = true;

        if (active)
            portalEffect.enabled = true;
        else
            portalEffect.enabled = false;

    }
    // OnTriggerEnter
    // Is called when a collider enters. Used to check if the player touches it
    // Parama:	Collider other:		The other collider that touched this object
    void OnTriggerEnter(Collider other)
	{
        if (other.tag == "Player" && active)
        {
            PlayerPrefs.SetInt("MoneyEarned", other.GetComponent<MoneyController>().GetMoney());
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.GetComponent<AudioSource>().PlayOneShot(teleportSound, 0.3f);
            gm.GetPlayer().enabled = false;
            gm.LoadScene(scene, spawnPointName);            
        }
    }
}
