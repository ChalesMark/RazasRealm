using System;
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
    [Header("Please don't change this, unless we have to")]
    public GameObject fade;                             // This is where the big black image thing is put for level transitions

    public Transform target;                                   // The target that the camera will be following
    bool disableSmoothCameraMove = false;               // Used for level transitions. If set to false, will snap to player position
    float fadeOpacity = 0;                              // The opacity of the 'fade' object
    float transitionSpeed = .5f;                        // The speed in which the fade happens

    #region Getters and Setters
    // SetSmoothCameraMovement
    // Sets the disableSmoothCameraMove variable
    // Parama:  bool toggle
    public void SetSmoothCameraMovement(bool toggle)
    {
        disableSmoothCameraMove = toggle;
    }
    #endregion

    // Update
    // Runs every frame
    void FixedUpdate()
    {
        if (target != null)
        {
            /*
            if (!disableSmoothCameraMove)
            {
                this.transform.rotation = Quaternion.Euler(50, 0, 0);
                this.transform.position =
                    Vector3.Lerp(
                        this.transform.position,
                        target.position + new Vector3(0, distance + distance / 4, -distance),
                        Vector3.Distance(this.transform.position, target.position) * Time.deltaTime);
            }
            else
            {
            */
                this.transform.rotation = Quaternion.Euler(50, 0, 0);
                this.transform.position = target.position + new Vector3(0, distance + distance / 4, -distance);
            //}
        }
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
    public IEnumerator FadeToBlack()
    {
        
        fadeOpacity = 0;
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        while (fadeOpacity < 1)
        {
            fadeOpacity += transitionSpeed * Time.deltaTime;
            fade.GetComponent<Image>().color = new Color(0, 0, 0, fadeOpacity);
            yield return null;
        }
        
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        this.transform.position = target.position + new Vector3(0, distance + distance / 4, -distance);     
    }

    // FadeToScreen
    // Fades the camera back to the screen. Returns control to the player once the fade is complete
    // Return:  IEnumerator
    public IEnumerator FadeToScreen()
    {
        fadeOpacity = 1;
        fade.GetComponent<Image>().color = new Color(0, 0, 0, 1);
        while (fadeOpacity > 0)
        {
            fadeOpacity -= transitionSpeed * Time.deltaTime;
            fade.GetComponent<Image>().color = new Color(0, 0, 0, fadeOpacity);
            yield return null;
        }

        fade.GetComponent<Image>().color = new Color(0, 0, 0, 0);        
    }
}
