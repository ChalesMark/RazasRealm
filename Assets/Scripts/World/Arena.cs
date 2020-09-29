using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Arena
// Last Updated: Sept 29 2020
// Mark Colling
// Used to handle arena behaviours
public class Arena : MonoBehaviour
{

	// These are public fields that we edit in the inspector 
	[Header("Arena Settings (Hover over for details)")]
	[Tooltip("Enemies\nDrag and drop enemies here. Note: if you want multiples of the same, you must drop the enemy in multiple times.")]
	public List<GameObject> enemies;
	[Tooltip("Enemy Spawn Points\nPlace 'empty' gameobjects here to represent spawn points. Can place more than one.")]
	public List<GameObject> enemySpawnPoints;
	[Tooltip("Rewards\nPlace item rewards you want the player to receive.")]
	public List<GameObject> rewards;
	[Tooltip("Reward Spawn Point\nPlace a single 'empty' gameobject to represent where the rewards will spawn from")]
	public GameObject rewardSpawnPoint;
	[Tooltip("Obstacle Rewards\nOPTIONAL: Place gameobjects you want to be 'deleted' here. Things like doors that open when the arena is complete.")]
	public List<GameObject> obstacleRewards;

	// Internal variables
	List<GameObject> ememiesInPlay;		// Used to track enemies in play
	bool begun;							// Flag for figuring out if the arena has begun

	
	// Update
	// Runs every frame
	void Update()
	{
		if (begun)
		{
			ememiesInPlay.RemoveAll(item => item == null);
		}
		if (begun && ememiesInPlay.Count == 0)
			FinishArena();
	}

	// OnTriggerEnter
	// Is called when a collider enters. Used to check if the player has entered the arena trigger
	// Parama:	Collider other:		The other collider that touched this object
	void OnTriggerEnter(Collider other)
	{
		
		if (other.tag == "Player")
		{
			if (!begun)
				StartArena();
		}
	}

	// StartArena
	// Function to start the arena. Spawns all enemies.
	void StartArena()
	{
		begun = true;
		List<GameObject> tempEnemies = new List<GameObject>();

		foreach (GameObject e in enemies)
		{
			Vector3 randomSpawn = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count - 1)].transform.position;
			tempEnemies.Add(Instantiate(e, randomSpawn, Quaternion.identity, null));  
		}

		ememiesInPlay = new List<GameObject>(tempEnemies);
		// The reason I don't add the enemies directly to the enemiesInPlay is so 
		// the update above that is checking for an empty list doesn't accidently activate											
	}

	void FinishArena()
	{
		foreach (GameObject r in rewards)
		{
			Instantiate(r, rewardSpawnPoint.transform.position, Quaternion.identity, null);
		}

		if (obstacleRewards != null)
        {
			foreach (GameObject or in rewards)
            {
				// Note, there might be a better way of doing this. Note I tried with a switch but because its a method, I couldn't fit it in
				if (or.CompareTag("Door"))
					or.GetComponent<Door>().OpenDoor();
			}

		}			

		Destroy(this.gameObject);
		// I delete this gameobject so there is no way it will accidently activate
	}
}

