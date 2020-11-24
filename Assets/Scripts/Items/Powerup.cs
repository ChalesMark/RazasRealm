using UnityEngine;

public enum PowerupType { money, health, ammo, armor,key }
public class Powerup : MonoBehaviour
{
    public int value;
    public PowerupType powerupType;
}

