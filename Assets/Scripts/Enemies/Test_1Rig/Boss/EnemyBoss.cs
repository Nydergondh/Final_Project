using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBoss : MonoBehaviour, IDamage, IDamageable {

    [HideInInspector] public SkinnedMeshRenderer[] skMeshRenderers;

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

    [Space]
    public bool isProtected = false;
    public float protectionDealy = 0.5f;
    public float protectionDuration = 2f;
    [SerializeField]
    private ParticleSystem[] fireProtection;
    [Space]

    public List<ProjectileSpawner> spawners;
    //public Queue<ProjectileSpawner> currentShootingSpawns;

    private CharacterController characterController;

    public HumanoidAnimations enemyAnim { get; set; }

    private ParticleSystem bloodParticles;

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
        skMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        enemyAnim = GetComponent<HumanoidAnimations>();
        bloodParticles = GetComponentInChildren<ParticleSystem>();

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
            if (health == 3) {
                HighAgreesion();
            }
            else if (health == 1) {
                UltraAgreesion();
            }
            StartCoroutine(ProtectBoss());
        }
    }

    public void OnDamage(int damage, Vector3 bloodDirection) {
        if (!isProtected) {
            health -= damage;
            bloodParticles.transform.rotation = Quaternion.LookRotation(bloodDirection, Vector3.up);
            bloodParticles.Play();
            if (health > 0) {
                if (health == 3) {
                    HighAgreesion();
                }
                else if (health == 1) {
                    UltraAgreesion();
                }
                StartCoroutine(ProtectBoss());
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
        fireProtection[1].gameObject.SetActive(true);
        protectionDealy += 1.5f;

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
        fireProtection[2].gameObject.SetActive(true);
        protectionDealy += 2f;

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

        foreach (SkinnedMeshRenderer skRenderer in skMeshRenderers) {
            skRenderer.enabled = true;
        }
        characterController.enabled = true;

        StageManager.INSTANCE.musicPlayer.Stop();
        StageManager.INSTANCE.musicPlayer.clip = SoundManager.GetSound(SoundAudios.Sound.BossMusic);
        StageManager.INSTANCE.musicPlayer.Play();
    }

    public IEnumerator ProtectBoss() {

        foreach (ParticleSystem fireParticles in fireProtection) {
            if (fireParticles.gameObject.activeSelf) {
                fireParticles.Play();
            }
        }

        //ParticleSystem.MainModule mainModule = fireProtection.main;
        //fireProtection.Play();
        isProtected = true;

        yield return new WaitForSeconds(protectionDealy);

        foreach (ParticleSystem fireParticles in fireProtection) {
            if (fireParticles.gameObject.activeSelf) {
                ParticleSystem.MainModule mainModule = fireParticles.main;
                fireParticles.GetComponent<InstantKillWall>().isWallActive = true;
                mainModule.startLifetime = 1f;
            }
        }

        //fireProtection.GetComponent<InstantKillWall>().isWallActive = true;
        //mainModule.startLifetime = 1f;

        yield return new WaitForSeconds(protectionDuration);

        foreach (ParticleSystem fireParticles in fireProtection) {
            if (fireParticles.gameObject.activeSelf) {
                ParticleSystem.MainModule mainModule = fireParticles.main;
                fireParticles.GetComponent<InstantKillWall>().isWallActive = false;
                mainModule.startLifetime = 0.3f;
                fireParticles.Stop();
            }
        }

        //fireProtection.GetComponent<InstantKillWall>().isWallActive = false;
        //mainModule.startLifetime = 0.3f;
        //fireProtection.Stop();

        isProtected = false;
    }

    
}
