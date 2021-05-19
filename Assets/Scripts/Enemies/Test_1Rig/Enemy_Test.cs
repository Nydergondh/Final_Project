using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Test : MonoBehaviour, IDamage, IDamageable
{
    public bool isRanged = false;
    public bool isBlind = false;

    [SerializeField]
    protected int health = 1;
    public int damage = 1;

    public bool alive = true;

    [HideInInspector]
    public FieldOfView_Test fov;
    [HideInInspector]
    public EnemyMovement_Test enemyMovement;
    [HideInInspector]
    public EnemyCombat_Test enemyCombat;

    public Weapom_SO currentWeapom;

    private NavMeshAgent navAgent;

    private CharacterController characterController;

    public HumanoidAnimations enemyAnim { get; set; }

    public Transform targetTransform;

    private Collider[] colliders;
    private Rigidbody[] rigidBodys;

    void Awake() {
        enemyAnim = GetComponent<HumanoidAnimations>();

        colliders = GetComponentsInChildren<Collider>();
        rigidBodys = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        navAgent = GetComponent<NavMeshAgent>();

        enemyCombat = GetComponent<EnemyCombat_Test>();
        enemyMovement = GetComponent<EnemyMovement_Test>();
        fov = GetComponent<FieldOfView_Test>();
    }

    private void Start() {
        foreach (Collider col in colliders) {
            col.enabled = false;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = true;
        }

        characterController.enabled = true;

        if (!isRanged) {
            navAgent.stoppingDistance = enemyCombat.minDistToAttack;
        }
        else {
            enemyAnim.SetCurretnWeapon(0);
        }

        navAgent.speed = enemyMovement.patrolSpeed;
        enemyAnim.SetAnimTotalSpeed(1.5f);
    }

    private void Update() {
        if (health > 0 && alive) {
            fov.FindVisibleTarget();
            enemyCombat.AttackTarget();
            enemyMovement.FollowTarget();
        }
        else {
            if (alive) {
                StageManager.INSTANCE.enemyCount--;
                EnableRagdoll();
                DropWeapom();
            }
        }
    }

    public int GetDamage() {
        return damage;
    }

    public NavMeshAgent GetNavAgent() {
        return navAgent;
    }

    public void OnDamage(int damage) {
        health -= damage;
    }

    private void EnableRagdoll() { 
        enemyAnim.SetAlive(false);

        foreach (Collider col in colliders) {
            col.enabled = true;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = false;
        }

        navAgent.enabled = false;
        characterController.enabled = false;
        alive = false;
    }

    public void DropWeapom() {
        if (currentWeapom) {
            Instantiate(currentWeapom.weaponObj, transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(-90, 0, 0));
        }
    }
}
