using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat_Test: MonoBehaviour
{

    private int attackType = 6;
    [SerializeField]
    private GameObject[] weaponMeshes;

    private MagicHandler magicHandler;

    //private KeyCode pressedWeapon;
    // Start is called before the first frame update
    void Start() {
        magicHandler = GetComponent<MagicHandler>();

        for (int i = 1; i < weaponMeshes.Length; i++) {
            if (weaponMeshes[i] != null) {
                weaponMeshes[i].SetActive(false);
            }
        }
    }

    public void Attack() {
        if (!Player_Test.player.isAttacking) {
            SetAtkType();
            if (Input.GetMouseButtonDown(0)) {
                Player_Test.player.SetAnimAttack(true);
                Player_Test.player.isAttacking = true;
            }
        }

        MagicSwitching();
    }

    private void MagicSwitching() {
        if (Input.GetButtonDown("Next Magic")) {
            //if the current magic is the last magic in the list
            if (magicHandler.currentMagicId + 1 == magicHandler.allMagics.Count) {
                magicHandler.UpdateCurrentMagic(0);
            }
            else {
                magicHandler.UpdateCurrentMagic(magicHandler.currentMagicId + 1);
            }

        }
        else if (Input.GetButtonDown("Previous Magic")) {
            //if the current magic is the first magic in the list
            if (magicHandler.currentMagicId == 0) {
                magicHandler.UpdateCurrentMagic(magicHandler.allMagics.Count - 1);
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

        else if (Input.GetKeyDown(KeyCode.Alpha6)) {
            attackType = 6;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha7)) {
            attackType = 7;
        }

        else if (Input.GetKeyDown(KeyCode.Alpha8)) {
            attackType = 8;
        }

        if (attackType <= 8 && attackType >= 0) {
            //playerAnim.SetCurretnWeapon(attackType);
            //TODO change latter to just have the above or this condicion
            Player_Test.player.SetAnimCurrentWeapon(attackType);
            SetWeaponMesh();
        }

    }

    private void SetWeaponMesh() {

        for (int i = 1; i < weaponMeshes.Length; i++) {
            if (attackType == i) {
                //if (attackType == 4) {
                //    weaponMeshes[2].SetActive(true);
                //}
                weaponMeshes[i].SetActive(true);
            }
            else {
                if (weaponMeshes[i] != null) {
                    weaponMeshes[i].SetActive(false);
                }
            }
        }

    }

}
