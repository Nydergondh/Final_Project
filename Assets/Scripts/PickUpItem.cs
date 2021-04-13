using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Weapom weapom;

    private void OnMouseDown() {
        Player_Test.player.playerCombat.currentWeapom = weapom;
        Destroy(gameObject);
    }
}
