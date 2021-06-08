using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamage, IDamageable
{
    public bool isRanged = false;

    [SerializeField]
    protected int health = 1;
    public int damage = 1;

    public bool alive = true;
    public bool isAttacking = false; // TODO maybe refactor this like to other script 

    public FieldOfView fov;
    public EnemyMovement enemyMovement; //TODO maybe refactor this to not be public
    public EnemyCombat enemyCombat;
    private NavMeshAgent navAgent;

    private CharacterController characterControl;

    [SerializeField]
    private HumanoidBicectAnim enemyAnimTorso;
    [SerializeField]
    private HumanoidBicectAnim enemyAnimLegs;

    public Transform targetTransform;

    private Collider[] colliders;
    private Rigidbody[] rigidBodys;

    void Awake() {
        colliders = GetComponentsInChildren<Collider>();
        rigidBodys = GetComponentsInChildren<Rigidbody>();
        characterControl = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Start() {

        foreach (Collider col in colliders) {
            col.enabled = false;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = true;
        }

        characterControl.enabled = true;
        enemyCombat = GetComponent<EnemyCombat>();
        enemyMovement = GetComponent<EnemyMovement>();
    }

    void Update() {
        if (health > 0) {
            fov.FindVisibleTarget();
            enemyCombat.AttackTarget();
            enemyMovement.FollowTarget();
        }
        else {
            if (alive) {
                //EnableRagdoll();
                gameObject.SetActive(false);
                alive = false;
            }
        }
    }

    #region anim seters
    public void SetAnimAttack(bool value) {
        enemyAnimTorso.SetAttack(value);
        enemyAnimLegs.SetAttack(value);
    }

    public void SetAnimMoving(bool value) {
        enemyAnimTorso.SetMoving(value);
        enemyAnimLegs.SetMoving(value);
    }

    public void SetAnimStrafing(bool value) {
        enemyAnimTorso.SetStrafe(value);
        enemyAnimLegs.SetStrafe(value);
    }
    #endregion

    public int GetDamage() {
        return damage;
    }

    public NavMeshAgent GetNavAgent() {
        return navAgent;
    }

    public void OnDamage(int damage) {
        health -= damage;
    }

    public void OnDamage(int damage, Vector3 bloodDirection) {
        health -= damage;
    }


    private void EnableRagdoll() {
        GetComponent<Rigidbody>().useGravity = false;
        enemyAnimTorso.EndAnimator(true);
        enemyAnimLegs.EndAnimator(true);

        foreach (Collider col in colliders) {
            col.enabled = true;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = true;
        }
        
    }
}
