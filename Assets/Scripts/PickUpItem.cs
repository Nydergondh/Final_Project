using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public Weapom_SO weapom;
    private Player_Combat_Test playerCombat;

    public void PickUpWeapom() {
        if (weapom != null) {
            playerCombat = Player_Test.player.playerCombat;
            playerCombat.DropWeapom();
            playerCombat.SetCurrentWeapom(weapom);
        }

        //open chest and equip player new weapom
        //UISingleton.INSTANCE.messageHighlighter.UpdateText("Picked up " + weapom.weapomName);
        //StartCoroutine(UISingleton.INSTANCE.messageHighlighter.ShowPickUpText());
        Destroy(gameObject);
    }
}
