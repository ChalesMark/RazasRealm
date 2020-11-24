using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public List<SpawnPoint> spawnPoints;
    
    public SpawnPoint GetSpawnPoint(string name)
    {
        if (spawnPoints.Count != 0)
        {
            foreach (var sp in spawnPoints)
                if (sp.spawnName.Equals(name))
                    return sp;                
            return spawnPoints[0];
        }
        return null;
    }

}
