using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetHighscoreButton : MonoBehaviour, IInteractable
{
    bool autoInteractable = false;
    bool IInteractable.AutoInteract { get => autoInteractable; set => autoInteractable = value; }

    public string GetInteractText()
    {
        return "Press F to Reset Save File";
    }

    public void Interact(InteractController interactController)
    {
        PlayerPrefs.DeleteAll();
        Camera.main.transform.Find("Canvas").Find("Highscore").GetComponent<Text>().text = "Highest Wave: " + PlayerPrefs.GetInt("Highscore").ToString();
    }
}
