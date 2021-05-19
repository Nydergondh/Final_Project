using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public Weapom_SO constWeapom;
    public Weapom_SO[] weapomArray;
    public bool used;
    
    Animator anim;
    Player_Combat_Test playerCombat;
    string chestWeapomName;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OpenChest() {
        //static weapom or random weapom
        if (!used) {
            playerCombat = Player_Test.player.playerCombat;
            playerCombat.DropWeapom();
            if (constWeapom != null) {
                playerCombat.SetCurrentWeapom(constWeapom);
                chestWeapomName = constWeapom.weapomName;
            }
            else {
                playerCombat.SetCurrentWeapom(weapomArray[Random.Range(1, weapomArray.Length)]);
                chestWeapomName = playerCombat.currentWeapom.weapomName;
            }

            //open chest and equip player new weapom
            //UISingleton.INSTANCE.messageHighlighter.UpdateText("Picked up " + chestWeapomName);
            //StartCoroutine(UISingleton.INSTANCE.messageHighlighter.ShowPickUpText());
            used = true;
            anim.SetBool("Open", true);
        }
    }
}
