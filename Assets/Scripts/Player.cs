using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamage, IDamageable
{
    [SerializeField]
    protected int health = 1;
    public int damage = 1;
    public bool alive = true;
    public bool isAttacking = false; // TODO maybe refactor this like to other script 

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    [SerializeField]
    private PlayerBisectAnim playerAnimTorso;
    [SerializeField]
    private PlayerBisectAnim playerAnimLegs;

    public Transform targetTransform;


    void Start() {
        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update() {
        if (health > 0) {
            playerMovement.Move();
            playerCombat.Attack();
        }
        else {
            gameObject.SetActive(false);
        }
    }

    #region anim seters
    public void SetAnimAttack(bool value) {
        playerAnimTorso.SetAttack(value);
        playerAnimLegs.SetAttack(value);
    }

    public void SetAnimMoving(bool value) {
        playerAnimTorso.SetMoving(value);
        playerAnimLegs.SetMoving(value);
    }

    public void SetAnimStrafing(bool value) {
        playerAnimTorso.SetStrafe(value);
        playerAnimLegs.SetStrafe(value);
    }

    public void SetAnimCurrentWeapon(int atkType) {
        playerAnimTorso.SetCurretnWeapon(atkType);
        playerAnimLegs.SetCurretnWeapon(atkType);
    }
    #endregion

    public int GetDamage() {
        return damage;
    }

    public void OnDamage(int damage) {
        health -= damage;
        print(gameObject.name + " Got hit!");
    }
}
