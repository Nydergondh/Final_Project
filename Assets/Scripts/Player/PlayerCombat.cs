using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour {
    private Player player;

    private int attackType = 0;
    [SerializeField]
    private MeshRenderer[] weaponMeshes;

    private MagicHandler magicHandler;

    //private KeyCode pressedWeapon;
    // Start is called before the first frame update
    void Start() {
        player = GetComponent<Player>();
        magicHandler = GetComponent<MagicHandler>();

        for (int i = 1; i < weaponMeshes.Length; i++) {
            weaponMeshes[i].enabled = false;
        }
    }

    public void Attack() {
        if (!player.isAttacking) {
            SetAtkType();
            if (Input.GetMouseButtonDown(0)) {
                player.SetAnimAttack(true);
                player.isAttacking = true;
            }
        }

        MagicSwitching();
    }

    private void MagicSwitching() {
        if (Input.GetButtonDown("Next Magic")) {
            //if the current magic is the last magic in the list
            if (magicHandler.currentMagicId + 1 == MagicHandler.allMagics.Count) {
                magicHandler.UpdateCurrentMagic(0);
            }
            else {
                magicHandler.UpdateCurrentMagic(magicHandler.currentMagicId + 1);
            }

        }
        else if (Input.GetButtonDown("Previous Magic")) {
            //if the current magic is the first magic in the list
            if (magicHandler.currentMagicId == 0) {
                magicHandler.UpdateCurrentMagic(MagicHandler.allMagics.Count - 1);
            }
            else {
                magicHandler.UpdateCurrentMagic(magicHandler.currentMagicId - 1);
            }
        }
    }

    private void SetAtkType() {
        //unarmed
        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            attackType = 0;
        }
        //rapier
        else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            attackType = 1;
        }
        //1h sword
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            attackType = 2;
        }
        //2h sword
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            attackType = 3;
        }
        //shiled
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            attackType = 4;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha5)) {
            attackType = 5;
        }

        if (attackType <= 5 && attackType >= 0) {
            //playerAnim.SetCurretnWeapon(attackType);
            //TODO change latter to just have the above or this condicion
            player.SetAnimCurrentWeapon(attackType);
            SetWeaponMesh();
        }

    }

    private void SetWeaponMesh() {

        for (int i = 1; i < weaponMeshes.Length; i++) {
            if (attackType == i) {
                if (attackType == 4) {
                    weaponMeshes[2].enabled = true;
                }
                weaponMeshes[i].enabled = true;
            }
            else {
                weaponMeshes[i].enabled = false;
            }
        }

    }

}
