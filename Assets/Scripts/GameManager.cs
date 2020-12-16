using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [Header("Other Stuff")]
    public Button startGameButon;
    public Slider dresserRotate;
    public List<GameObject> hats;

    // Transition holding place
    public GameObject lastGun;
    public GameObject lastHat;

    MusicManager musicManager;
    private Transform canvas;

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
        musicManager = GetComponent<MusicManager>();
        canvas = Camera.main.transform.Find("Canvas");

        // Sets gameobjects to DontDestroyOnLoad so they continue to exist between level changes
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(Camera.main);

        musicManager.Play(Music.menu);

        if (ignoreGameStart)
        {
            musicManager.Play(Music.hub);
            startGameButon.gameObject.SetActive(false);
            Vector3 mainSP;
            if (GameObject.Find("SpawnPoint"))
                mainSP = GameObject.Find("SpawnPoint").transform.position;
            else
                mainSP = Vector3.zero;

            player = Instantiate(playerPrefab, mainSP, Quaternion.identity, null);
            Camera.main.GetComponent<CameraController>().SetTarget(player);
        }

        if (startGameButon)
            startGameButon.onClick.AddListener(StartGame);
        if (dresserRotate)
            dresserRotate.onValueChanged.AddListener(delegate { player.transform.rotation = Quaternion.Euler(0, -dresserRotate.value, 0); });
    }

    public void StartGame()
    {
        print("START GAME!");
        LoadScene("Hub", "main");
        musicManager.Play(Music.hub);
        canvas.Find("TitleText").gameObject.SetActive(false);
        canvas.Find("TeamNameText").gameObject.SetActive(false);
        canvas.Find("OurNamesText").gameObject.SetActive(false);
        canvas.Find("VersionText").gameObject.SetActive(false);
        startGameButon.gameObject.SetActive(false);
        PlayerPrefs.SetInt("MoneyEarned", 20);
        Camera.main.transform.Find("Canvas").Find("PlayerHealth").gameObject.SetActive(true);
        Camera.main.transform.Find("Canvas").Find("MoneyText").GetComponent<Text>().enabled = true;
        //AHHHH I CAN FEEL THE STRUCTURE FALLING APART :(
        Camera.main.GetComponentInChildren<PauseScript>().Resume();
    }

    // LoadScene
    // loads the scene. NOTE: the scene must be added to the build list to be loadable
    // Parama:  string scene:       The name of the scene.
    public void LoadScene(string scene,string spawnPointName)
    {
        if (musicManager.GetMusic() == Music.level)
            musicManager.PlayVictory();
        if (player)
        {
            lastGun = Instantiate(player.GetComponent<GearController>().weapon.gameObject);
            lastGun.transform.position = new Vector3(0,10000,0);
            lastHat = Instantiate(player.GetComponent<GearController>().hat);
            lastHat.transform.position = new Vector3(0, 10000, 0);
            DontDestroyOnLoad(lastHat);
            DontDestroyOnLoad(lastGun);
        }
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
            FindPoint(scene, spawnPointName);
        }
    }

    private void FindPoint(string scene, string spawnPointName)
    {
        CameraController camera = Camera.main.GetComponent<CameraController>();
        GameObject lmGM = GameObject.Find("LevelManager");
        LevelManager lm = lmGM.GetComponent<LevelManager>();

        Vector3 mainSP = lm.GetSpawnPoint(spawnPointName).GetLocation();
        
        player = Instantiate(playerPrefab, mainSP, Quaternion.identity, null);
        if (lastGun && lastHat)
        {
            player.GetComponent<GearController>().RecieveValues(lastHat,lastGun);
        }
        camera.SetTarget(player);
        
        StartCoroutine(camera.FadeToScreen(scene, this));
    }

    IEnumerator LoadAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene,LoadSceneMode.Single);

        while (!asyncLoad.isDone)
        {      
            yield return null;
        }
    }

    internal void TakeAwayControl()
    {
        if(player == null) {
            return;
        }
        PlayerController pc = player.GetComponent<PlayerController>();
        pc.StopAnimation();
        pc.DisableGun(false);
        pc.enabled = false;
        dresserRotate.value = 180;
    }
    internal void ReturnControl()
    {
        if(player == null) {
            return;
        }

        PlayerController pc = player?.GetComponent<PlayerController>();
        pc.DisableGun(true);
        pc.enabled = true;
    }
    internal void GivePlayerHat(GameObject gameObject)
    {
        player.GetComponent<GearController>().SwitchHat(gameObject);
    }
}
