using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour, IDamage, IDamageable {

    [HideInInspector] public SkinnedMeshRenderer skMeshRenderer;

    [SerializeField]
    protected int health = 1;
    public int damage = 1;

    [Space]
    public bool alive = true;
    public bool startFight = false;
    [Space]

    [Space]
    public int numberOfAttackers = 0;
    public int maxNumberOfAttackers = 3;
    [Space]

    public float minDistToAttack = 5f;

    public bool activateProtection = false;
    public float protectionDealy = 0.5f;
    public float protectionDuration = 2f;

    public List<ProjectileSpawner> spawners;
    //public Queue<ProjectileSpawner> currentShootingSpawns;

    private CharacterController characterController;

    public HumanoidAnimations enemyAnim { get; set; }

    public Transform targetTransform;

    private Collider[] colliders;
    private Rigidbody[] rigidBodys;

    public delegate void FinalShowDown();
    public FinalShowDown finalShowDown;

    public delegate void EndFight();
    public EndFight endFight;


    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool coolDown = false;
    [HideInInspector] public float colldownModfier = 1f;
    public float coolDownRangedAttack = 3f;
    public float rotateSpeed = 50f;

    void Awake() {
        skMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        enemyAnim = GetComponent<HumanoidAnimations>();

        colliders = GetComponentsInChildren<Collider>();
        rigidBodys = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start() {
        foreach (Collider col in colliders) {
            col.enabled = false;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = true;
        }

        characterController.enabled = false;

        enemyAnim.SetCurretnWeapon(0);
    }

    private void Update() {
        if (startFight) {
            if (health > 0 && alive) {
                HandleRotation();
                if (Vector3.Distance(transform.position, targetTransform.position) <= minDistToAttack) {
                    if (!isAttacking && !coolDown) {
                        enemyAnim.SetAttack(true);
                        isAttacking = true;
                    }
                }
            }
            else {
                if (alive) {
                    StageManager.INSTANCE.enemyCount--;
                    EnableRagdoll();
                    endFight();
                }
            }
        }
    }

    private void HandleRotation() {
        Vector3 dirToTarget = (targetTransform.position - transform.position).normalized; //direction to target
        Quaternion newRotation = Quaternion.LookRotation(dirToTarget); //rotation to achieve ideal look position

        if (transform.rotation != newRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime); //rotate towards the object direction
        }
    }

    IEnumerator AttackCoolDown() {
        coolDown = true;
        yield return new WaitForSeconds(coolDownRangedAttack * colldownModfier);
        coolDown = false;
    }

    public void UnsetAttack() {
        isAttacking = false;
        StartCoroutine(AttackCoolDown());
    }

    public int GetDamage() {
        return damage;
    }

    public void OnDamage(int damage) {
        health -= damage;
        if (health > 0) {
            StartCoroutine(ProtectBoss());
            if (health == 3) {
                HighAgreesion();
            }
            else if (health == 1) {
                UltraAgreesion();
            }
        }
    }

    private void EnableRagdoll() {
        enemyAnim.SetAlive(false);

        foreach (Collider col in colliders) {
            col.enabled = true;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = false;
        }

        characterController.enabled = false;
        alive = false;
    }

    private void HighAgreesion() {
        foreach (ProjectileSpawner projSpawner in spawners) {
            if (!projSpawner.bossProtection) {

                projSpawner.projSpeedModifier = 1.1f;
                projSpawner.maxNumOfProjModifier += 1;
                projSpawner.collDownModifier = 0.75f;
                colldownModfier = 1.25f;
            }
        }
    }

    private void UltraAgreesion() {
        foreach (ProjectileSpawner projSpawner in spawners) {
            if (!projSpawner.bossProtection) {
                projSpawner.projSpeedModifier = 1.25f;
                projSpawner.maxNumOfProjModifier += 1;
                projSpawner.collDownModifier = 0.85f;
                colldownModfier = 1.5f;
            }
        }
    }

    public void StartBoss() {
        startFight = true;
        targetTransform = Player_Test.player.transform;
        skMeshRenderer.enabled = true;
        characterController.enabled = true;
    }

    public IEnumerator ProtectBoss() {
        yield return new WaitForSeconds(protectionDealy);
        activateProtection = true;
        yield return new WaitForSeconds(protectionDuration);
        activateProtection = false;
    }
    
}
