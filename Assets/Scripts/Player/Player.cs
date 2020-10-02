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

    public static Player player;

    private PlayerMovement playerMovement;
    private PlayerCombat playerCombat;

    private CharacterController characterControl;

    [SerializeField]
    private PlayerBisectAnim playerAnimTorso;
    [SerializeField]
    private PlayerBisectAnim playerAnimLegs;

    public Transform targetTransform;

    private Collider[] col;
    private Rigidbody[] rigidBodys;

    void Awake() {
        if (player != null) {
            Destroy(gameObject);
        }
        else {
            player = this;
        }
    }

    void Start() {
        
        col = GetComponentsInChildren<Collider>();
        rigidBodys = GetComponentsInChildren<Rigidbody>();
        characterControl = GetComponent<CharacterController>();

        foreach (Collider collider in col) {
            collider.enabled = false;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = true;
        }

        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();

        characterControl.enabled = true;
    }

    void Update() {
        if (health > 0) {
            playerMovement.Move();
            playerCombat.Attack();
        }
        else {
            if (alive) {
                //EnableRagdoll();
                alive = false;
                gameObject.SetActive(false);
            }
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

    public HumanoidBicectAnim GetLegsAnim() {
        return playerAnimLegs;
    }

    public HumanoidBicectAnim GetTorsoAnim() {
        return playerAnimTorso;
    }

    #endregion

    public int GetDamage() {
        return damage;
    }

    public void OnDamage(int damage) {
        health -= damage;
        print(gameObject.name + " Got hit!");
    }

    private void EnableRagdoll() {

        characterControl.enabled = false;

        playerAnimTorso.EndAnimator(false);
        playerAnimLegs.EndAnimator(false);

        foreach (Collider collider in col) {
            collider.enabled = true;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = false;
        }
        alive = false;
    }
    //TODO IMPORTANT!!! change this later (3 calls just to do instanciation in animation frame)
    //public void CallAnimProjectile(GameObject prefab, Vector3 pos, Quaternion rot) {
    //    playerAnimTorso.InstanciateProjectile(prefab, pos, rot);
    //}
}
