using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, IInteractable
{
    public string[] text;
    public string speaker;
    public float readTime;
    public bool showChatBox;

    public void Interact(InteractController controller) {
        Camera.main.GetComponent<CameraController>().ShowMessage(text, speaker, readTime, showChatBox);
    }

    public string GetInteractText() {
        return "Press [Interact] to Interact with Sign";
    }
}
