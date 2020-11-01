using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// GameManager
// Last Updated: Sept 29 2020
// Mark Colling
// Used to control major game stuff like the player, possible points and upgrades. Basiclly global stuff
public class GameManager : MonoBehaviour
{
    // These are public fields that we edit in the inspector 
    [Header("playerPrefab: Place the gameobject that will become the player here")]
    public GameObject playerPrefab;     // The prefab of the player

    GameObject player;                  // The actual player object

    [Header("Toggle for overriding the start game functions. Used so you can create test levels. If set, player will spawn at (0,0,0)")]
    public bool ignoreGameStart;

    #region Getters and Setters
    // GetPlayer
    // retrieves the player controller
    // Return:  PlayerController
    public PlayerController GetPlayer()
    {
        return player.GetComponent<PlayerController>();
    }

    public GameObject GetPlayerObject()
    {
        return player;
    }
    #endregion

    // Start
    // Runs once the gameobject is loaded in
    void Start()
    {
        Camera.main.cullingMask = 1 << 0;        

        // Sets gameobjects to DontDestroyOnLoad so they continue to exist between level changes
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(Camera.main);

        if(!ignoreGameStart)
            LoadScene("Hub","main");
        else
        {
            player = Instantiate(playerPrefab);
            Camera.main.GetComponent<CameraController>().SetTarget(player);
            player.transform.position = Vector3.zero;
        }
    }

    // LoadScene
    // loads the scene. NOTE: the scene must be added to the build list to be loadable
    // Parama:  string scene:       The name of the scene.
    public void LoadScene(string scene,string spawnPointName)
    {
        CameraController camera = Camera.main.GetComponent<CameraController>();
        camera.SetFade(0);

        StartCoroutine(camera.FadeToBlack(scene, spawnPointName,this));
             
    }

    public void LoadScene2(string scene, string spawnPointName)
    {
        SceneManager.LoadScene(scene);

        if (SceneManager.GetActiveScene().name != scene)
        {

            StartCoroutine(WaitForSceneLoad(scene, spawnPointName));
        }
    }

    IEnumerator WaitForSceneLoad(string scene, string spawnPointName)
    {
        while (SceneManager.GetActiveScene().name != scene)
        {
            yield return null;
        }

        if (SceneManager.GetActiveScene().name == scene)
        {
            FindPoint(spawnPointName);
        }
    }

    private void FindPoint(string spawnPointName)
    {
        CameraController camera = Camera.main.GetComponent<CameraController>();
        GameObject lmGM = GameObject.Find("LevelManager");
        LevelManager lm = lmGM.GetComponent<LevelManager>();

        Vector3 mainSP = lm.GetSpawnPoint(spawnPointName).GetLocation();
        
        player = Instantiate(playerPrefab, mainSP, Quaternion.identity, null);
        camera.SetTarget(player);
        
        StartCoroutine(camera.FadeToScreen());
    }

    IEnumerator LoadAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene,LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {      
            yield return null;
        }
    }
}
