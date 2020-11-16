﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// CameraController
// Last Updated: Sept 29 2020
// Mark Colling
// Code for handling the camera
public class CameraController : MonoBehaviour
{
    // These are public fields that we edit in the inspector 
    [Header("Camera Settings")]
    public float distance;                              // Distance from the user, I left this public in case we wanted to play with it

    [Header("Please don't change these, unless we have to")]
    public GameObject fade;                             // This is where the big black image thing is put for level transitions
    public Text outputText;
    public Text speaker;
    public Image chatBoxBG;
    
    float outputTextFade;

    public Transform target;                                   // The target that the camera will be following
    float fadeOpacity = 0;                              // The opacity of the 'fade' object
    float fadeSpeed = 1f;

    [Header("Customization Menu Stuff")]
    public GameObject customizationMenu;
    public Button previousHat;
    public Button nextHat;
    public Button customClose;

    Transform specialLook = null;                       // This is used for special camera angles

    void Awake()
    {
        customizationMenu.SetActive(false);
    }

    // Update
    // Runs every frame
    void FixedUpdate()
    {
        if (target != null)
        {
            if (!specialLook)
            {
                this.transform.rotation = Quaternion.Euler(50, 0, 0);
                this.transform.position = target.position + new Vector3(0, distance + distance / 4, -distance);
            }
            else
            {
                this.transform.rotation = specialLook.rotation;
                this.transform.position = target.position + specialLook.position;
            }
        }
    }

    public void ChangeSpecialLook(Transform transform)
    {
        specialLook = transform;
    }

    // SetTarget
    // Used to set the target for the camera
    // Parama:	GameObject target:		The gameobject that the camera will now follow
    public void SetTarget(GameObject target)
    {
        this.target = target.transform;
    }

    // FadeToBlack
    // Fades the camera to black. Also disables player control. Use with the FadeToScreen function
    // Return:  IEnumerator
    public IEnumerator FadeToBlack(string scene, string spawnPointName,GameManager gm)
    {
        SetFade(0);
        do
        {            
            yield return null;
            fadeOpacity += fadeSpeed * Time.deltaTime;
            fade.GetComponent<Image>().color = new Color(0, 0, 0, fadeOpacity);            
        } while (fadeOpacity < 1);
        SetFade(1);
        gm.LoadScene2(scene, spawnPointName);
    }

    // FadeToScreen
    // Fades the camera back to the screen. Returns control to the player once the fade is complete
    // Return:  IEnumerator
    public IEnumerator FadeToScreen()
    {
        SetFade(1);
        do
        {
            yield return null;
            fadeOpacity -= fadeSpeed * Time.deltaTime;
            fade.GetComponent<Image>().color = new Color(0, 0, 0, fadeOpacity);
        } while (fadeOpacity > 0);

        SetFade(0);
    }

    public void SetFade(float fade)
    {
        fadeOpacity = fade;
        this.fade.GetComponent<Image>().color = new Color(0, 0, 0, fade);
    }

    public void ShowMessage(string[] text, string speakerName, float time, bool showChatBox)
    {
        if (speakerName != "")
            speaker.text = speakerName;
        if (!showChatBox)
            chatBoxBG.color = new Color(chatBoxBG.color.r, chatBoxBG.color.g, chatBoxBG.color.b, 1);
        else
            chatBoxBG.color = new Color(chatBoxBG.color.r, chatBoxBG.color.g, chatBoxBG.color.b, 0);

        outputText.color = new Color(1, 1, 1, 1);
        speaker.color = new Color(1, 1, 1, 1);
        StartCoroutine(WaitToRead(text,time));       
    }

    IEnumerator WaitToRead(string[] text,float time)
    {
        foreach(var t in text) {
            outputText.text = t;
            outputTextFade = 1;
            outputText.color = new Color(1, 1, 1, outputTextFade);
            yield return new WaitForSeconds(time);
        }
        StartCoroutine(FadeOutTextOutput());
       
    }

    public Button GetCustomCloseButton()
    {
        return customClose;
    }

    public IEnumerator FadeOutTextOutput()
    {
        do
        {
            yield return null;
            outputTextFade -= .1f * Time.deltaTime;
            outputText.color = new Color(1, 1, 1, outputTextFade);
            speaker.color = new Color(1, 1, 1, outputTextFade);

            if (chatBoxBG.color.a != 0)
                chatBoxBG.color = new Color(chatBoxBG.color.r, chatBoxBG.color.g, chatBoxBG.color.b, outputTextFade);

        } while (outputTextFade > 0);

        outputTextFade = 0;
        outputText.text = "";
        speaker.text = "";
    }
}
