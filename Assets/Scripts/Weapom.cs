using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapom", menuName = "Weapoms/Weapom")]
public class Weapom : ScriptableObject
{
    public GameObject weaponObj; // show up weapom when equipped
    public int id; // used to pickUp Weapom and equip it
}
