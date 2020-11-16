using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizationStation : MonoBehaviour, MarksIInteractable
{
    GameManager gameManager;
    CameraController cameraController;
    bool active;
    public Button menuCloseButton;
    public Button nextHat;
    public Button previousHat;
    GameObject specialCamera;
    List<GameObject> hats;
    int currentHat;
    void Awake()
    {
        specialCamera = new GameObject();
        specialCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
        specialCamera.transform.position = new Vector3(0, 1, -5);
        active = false;
        cameraController = Camera.main.GetComponent<CameraController>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        hats = gameManager.hats;
        menuCloseButton = cameraController.customClose;
        previousHat = cameraController.previousHat;
        nextHat = cameraController.nextHat;

        menuCloseButton.onClick.AddListener(Close);
        nextHat.onClick.AddListener(NextHat);
        previousHat.onClick.AddListener(PreviousHat);
        currentHat = 0;
    }

    private void PreviousHat()
    {
        currentHat--;
        if (currentHat < 0)
            currentHat = hats.Count - 1;
        GivePlayerHat();
    }

    private void NextHat()
    {
        currentHat++;
        if (currentHat > hats.Count - 1)
            currentHat = 0;
        GivePlayerHat();
    }

    public void Close()
    {
        cameraController.ChangeSpecialLook(null);
        cameraController.customizationMenu.SetActive(false);
        gameManager.ReturnControl();
    }

    public void Interact()
    {        
        cameraController.ChangeSpecialLook(specialCamera.transform);
        cameraController.customizationMenu.SetActive(true);
        gameManager.TakeAwayControl();
        currentHat = 0;
    }

    private void GivePlayerHat()
    {
        gameManager.GivePlayerHat(hats[currentHat]);
    }
}