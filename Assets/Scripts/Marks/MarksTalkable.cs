using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarksTalkable : MonoBehaviour, IInteractable
{
    
    public bool AutoInteract { get; set; } = false;
    public string[] text;
    public string speaker;
    public float readTime;
    public bool showChatBox;
    CameraController cameraController;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Interact(InteractController controller)
    {
        cameraController.ShowMessage(text, speaker, readTime, !showChatBox);
    }

    public string GetInteractText() {
        return "Press Interact to Read Sign";
    }
}