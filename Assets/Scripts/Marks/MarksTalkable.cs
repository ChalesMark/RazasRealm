using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarksTalkable : MonoBehaviour, MarksIInteractable
{
    public string[] text;
    public string speaker;
    public float readTime;
    public bool showChatBox;
    CameraController cameraController;

    void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
    }

    public void Interact()
    {
        cameraController.ShowMessage(text, speaker, readTime, !showChatBox);
    }
}