using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuyable
{
    int Cost { get; set; }
}

public enum BuyableType
{
    GUN,
    DOOR,
    UPGRADE,
    HEALTH
}
