using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat_Test: MonoBehaviour
{

    private int attackType = 0;
    [SerializeField]
    private GameObject[] weaponObj;
    public Weapom currentWeapom; 

    private MagicHandler magicHandler;
    private PlayerAnimations playerAnim;

    public bool isAttacking = false;

    //private KeyCode pressedWeapon;
    // Start is called before the first frame update
    void Start() {
        magicHandler = GetComponent<MagicHandler>();

        for (int i = 1; i < weaponObj.Length; i++) {
            if (weaponObj[i] != null) {
                weaponObj[i].SetActive(false);
            }
        }

        playerAnim = GetComponent<PlayerAnimations>();
    }

    public void Attack() {
        if (!isAttacking) {
            SetAtkType();
            if (Input.GetMouseButtonDown(0)) {
                playerAnim.SetAttack(true);
                isAttacking = true;
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

        //rapier

        //1h sword

        //2h sword

        //Halbert

        //shiled

        if (attackType <= 8 && attackType >= 0) {
            //playerAnim.SetCurretnWeapon(attackType);
            //TODO change latter to just have the above or this condicion
            playerAnim.SetCurretnWeapon(attackType);

            SetWeapomId();
            if (attackType >= 0 && attackType <= 8) {
                SetWeaponMesh();
            }
        }

    }

    public void UnsetAttack() {
        isAttacking = false;
    }

    private int SetWeapomId() {
        if (currentWeapom != null) {
            attackType = currentWeapom.id;
        }
        else{
            attackType = 0;
        }
        return attackType;
    }

    private void SetWeaponMesh() {
        foreach (GameObject weapom in weaponObj) {
            if (weapom == currentWeapom.weaponObj) {
                weapom.SetActive(true);
            }
            else {
                weapom.SetActive(true);
            }
        }
    }

}
