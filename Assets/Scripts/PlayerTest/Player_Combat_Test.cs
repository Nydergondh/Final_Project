using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Combat_Test: MonoBehaviour
{

    private int attackType = 0;
    [SerializeField]
    private Weapom[] weaponObj;

    public Weapom_SO currentWeapom;

    public bool usingMagic = true;

    private MagicHandler magicHandler;
    private PlayerAnimations playerAnim;

    public bool isAttacking = false;

    //private KeyCode pressedWeapon;
    // Start is called before the first frame update
    void Start() {
        magicHandler = GetComponent<MagicHandler>();

        for (int i = 1; i < weaponObj.Length; i++) {
            if (weaponObj[i].gameObject != null) { 
                weaponObj[i].gameObject.SetActive(false);
            }
        }

        playerAnim = GetComponent<PlayerAnimations>();
    }

    //switch magic to melle or melle to magic
    public void SwitchCombat() {
        if (usingMagic) {
            usingMagic = false;
        }
        else {
            usingMagic = true;
        }
        SetAtkType(usingMagic);
    }

    public void Combat() {
        if (!isAttacking) {

            if (Input.GetKeyDown(KeyCode.Space)) {
                SwitchCombat();
            }

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

    public void SetCurrentWeapom(Weapom_SO newWeapom) {
        currentWeapom = newWeapom;
        usingMagic = false;
        SetAtkType(false);
    }

    private void SetAtkType(bool useMagic) {
        SetWeaponMesh(useMagic);
        if (!useMagic) {
            playerAnim.SetCurretnWeapon(currentWeapom.id);
        }
        else {
            playerAnim.SetCurretnWeapon(0);
        }
    }

    public void UnsetAttack() {
        isAttacking = false;
    }

    private void SetWeaponMesh(bool useMagic) {
        //in case is of not using magic, switch to weapom
        if (!useMagic) {
            foreach (Weapom weapom in weaponObj) {
                if (weapom.weapomTemplate.id == currentWeapom.id) {
                    print(weapom.name);
                    weapom.gameObject.SetActive(true);
                }
                else {
                    print("Not Using "+ weapom.name);
                    weapom.gameObject.SetActive(false);
                }
            }
        }
        //in case is using magic, switch to no weapom
        else {
            foreach (Weapom weapom in weaponObj) {
                weapom.gameObject.SetActive(false);
            }
        }
    }

    public void DropWeapom() {
        if (currentWeapom) {
            Instantiate(currentWeapom.weaponObj, transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0 ,0));
        }
    }

}
