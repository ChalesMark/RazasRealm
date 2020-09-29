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

    #region Getters and Setters
    // GetPlayer
    // retrieves the player controller
    // Return:  PlayerController
    public PlayerController GetPlayer()
    {
        return player.GetComponent<PlayerController>();
    }
    #endregion

    // Start
    // Runs once the gameobject is loaded in
    void Start()
    {
        Camera.main.cullingMask = 1 << 0;
        player = Instantiate(playerPrefab);
        Camera.main.GetComponent<CameraController>().SetTarget(player);

        // Sets gameobjects to DontDestroyOnLoad so they continue to exist between level changes
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(Camera.main);

        LoadScene("Hub");
    }

    // LoadScene
    // loads the scene. NOTE: the scene must be added to the build list to be loadable
    // Parama:  string scene:       The name of the scene.
    public void LoadScene(string scene)
    {
        StartCoroutine(LoadAsyncScene(scene));
    }

    IEnumerator LoadAsyncScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {      
            yield return null;
        }
    }
}
