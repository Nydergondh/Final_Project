using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Test : MonoBehaviour, IDamage, IDamageable {

    [SerializeField]
    protected int health = 1;
    public int damage = 1;
    public bool alive = true;
    public bool isInvisible = false;
    public bool timeSlowed = false;
    public bool invertControls = false;
    public float timeSlowScale = 0.5f;

    public bool canMove = true;

    #region TEST_VARIABLES
    //TESTING ONLY
    private MeshRenderer[] m_Renderes;
    private SkinnedMeshRenderer[] s_Renderes;
    public bool usingOldMaterial;

    public Material oldMaterial;
    public Material newMaterial;
    //
    #endregion

    public static Player_Test player;

    public Player_Movement_Test playerMovement;
    public Player_Combat_Test playerCombat;

    private MagicHandler magicHandler;

    private AudioSource audioSource;

    private CharacterController playerController;

    [HideInInspector]
    private PlayerAnimations playerAnimator;

    public Transform targetTransform;

    private Collider[] col;
    private Rigidbody[] rigidBodys;

    private ParticleSystem bloodParticles;

    public bool _isMakingNoise = false;

    void Awake() {
        if (player != null) {
            Destroy(gameObject);
        }
        else {
            player = this;
        }
        bloodParticles = GetComponentInChildren<ParticleSystem>();
        m_Renderes = GetComponentsInChildren<MeshRenderer>();
        s_Renderes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();
        playerController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimations>();

        col = GetComponentsInChildren<Collider>();
        rigidBodys = GetComponentsInChildren<Rigidbody>();

        foreach (Collider collider in col) {
            collider.enabled = false;
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = true;
        }
        #region TEST
        //TEST
        //if (usingOldMaterial) {
        //    foreach(MeshRenderer mR in m_Renderes) {
        //        mR.material = oldMaterial;
        //    }
        //    foreach (SkinnedMeshRenderer mS in s_Renderes) {
        //        mS.material = oldMaterial;
        //    }
        //}
        //else {
        //    foreach (MeshRenderer mR in m_Renderes) {
        //        mR.material = newMaterial;
        //    }
        //    foreach (SkinnedMeshRenderer mS in s_Renderes) {
        //        mS.material = newMaterial;
        //    }
        //}
        //END TEST
        #endregion

        playerCombat = GetComponent<Player_Combat_Test>();
        playerMovement = GetComponent<Player_Movement_Test>();

        playerController.enabled = true;
    }

    void Update() {
        if (health > 0) {
            if (canMove){
                playerMovement.Move();
                playerMovement.MakingNoise();
                playerCombat.Combat();
            }
        }
        else {
            if (alive) {
                EnableRagdoll();
            }
        }
        if (Input.GetKeyDown(KeyCode.P)) {
            health = 100;
        }
        if (Input.GetKeyDown(KeyCode.L)) {
            health = 1;
        }
    }

    public int GetDamage() {
        return damage;
    }

    public void OnDamage(int damage) {
        if (health > 0) {
            audioSource.PlayOneShot(SoundManager.GetSound(SoundAudios.Sound.BloodSplash));
            health -= damage;
        }
    }

    public void OnDamage(int damage, Vector3 bloodDirection) {
        bloodParticles.transform.rotation = Quaternion.LookRotation(bloodDirection, Vector3.up);
        bloodParticles.Play();
        if (health > 0) {
            audioSource.PlayOneShot(SoundManager.GetSound(SoundAudios.Sound.BloodSplash));
            health -= damage;
        }
    }

    private void EnableRagdoll() {
        playerController.enabled = false;
        playerAnimator.SetAlive(false);

        foreach (Collider collider in col) {
            if (!collider.isTrigger) {
                collider.enabled = true;
            }
        }
        foreach (Rigidbody rb in rigidBodys) {
            rb.isKinematic = false;
        }

        alive = false;
    }
    
    public void WarpPlayerToPos(Stairs stairs)  {
        playerController.enabled = false;
        transform.position = stairs.nextSpawn.spawnPosition.position;
        playerController.enabled = true;
    }

    public void PlayAttackSound() {
        print("Played Attack Sound!");
        audioSource.Stop();
        if (!audioSource.isPlaying) {
            int sound = Random.Range(1, 3);

            switch (sound){
                case 1:
                    audioSource.PlayOneShot(SoundManager.GetSound(SoundAudios.Sound.WeapomSwing_1));
                    break;
                case 2:
                    audioSource.PlayOneShot(SoundManager.GetSound(SoundAudios.Sound.WeapomSwing_2));
                    break;
            }
        }
    }

}
