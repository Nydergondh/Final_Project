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

    private CharacterController playerController;

    [HideInInspector]
    private PlayerAnimations playerAnimator;

    public Transform targetTransform;

    private Collider[] col;
    private Rigidbody[] rigidBodys;

    public bool _isMakingNoise = false;

    void Awake() {
        if (player != null) {
            Destroy(gameObject);
        }
        else {
            player = this;
        }

        m_Renderes = GetComponentsInChildren<MeshRenderer>();
        s_Renderes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void Start() {

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
        if (usingOldMaterial) {
            foreach(MeshRenderer mR in m_Renderes) {
                mR.material = oldMaterial;
            }
            foreach (SkinnedMeshRenderer mS in s_Renderes) {
                mS.material = oldMaterial;
            }
        }
        else {
            foreach (MeshRenderer mR in m_Renderes) {
                mR.material = newMaterial;
            }
            foreach (SkinnedMeshRenderer mS in s_Renderes) {
                mS.material = newMaterial;
            }
        }
        //END TEST
        #endregion

        playerCombat = GetComponent<Player_Combat_Test>();
        playerMovement = GetComponent<Player_Movement_Test>();

        playerController.enabled = true;
    }

    void Update() {
        if (health > 0) {
            playerMovement.Move();
            playerMovement.MakingNoise();
            playerCombat.Attack();
        }
        else {
            if (alive) {
                EnableRagdoll();
                //alive = false;
                //gameObject.SetActive(false);
            }
        }
    }

    public int GetDamage() {
        return damage;
    }

    public void OnDamage(int damage) {
        health -= damage;
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
    
}
