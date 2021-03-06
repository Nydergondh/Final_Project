﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatTest : MonoBehaviour
{
    private int attackType = 6;
    [SerializeField]
    private MeshRenderer[] weaponMeshes;

    private PlayerAnimations playerAnim;
    //private KeyCode pressedWeapon;
    // Start is called before the first frame update
    void Start()
    {
        playerAnim = GetComponent<PlayerAnimations>();

        for (int i = 1; i < weaponMeshes.Length; i++) {
            weaponMeshes[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerAnim.GetAttack()) {
            SetAtkType();
        }

        if (Input.GetMouseButtonDown(0)) {
            playerAnim.SetAttack(true);
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
            playerAnim.SetCurretnWeapon(attackType);

            SetWeaponMesh();
        }

    }

    private void SetWeaponMesh() {

        for (int i = 1; i < weaponMeshes.Length; i++) {
            if (attackType == i) {
                //if (attackType == 4) { //Use when Shield Enabled
                //    weaponMeshes[2].enabled = true;
                //}
                weaponMeshes[i].enabled = true;
            }
            else {
                weaponMeshes[i].enabled = false;
            }
        }

    }

    public void UnsetAttack() {
        playerAnim.SetAttack(false);
    }

}
