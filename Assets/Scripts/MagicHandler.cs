using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHandler : MonoBehaviour
{
    public List<Magic> allMagics; // all magics currently in the game
    public GameObject[] magicPrefabs;  // objects you want to instanciate when magic is casted

    public int currentMagicId; // done for simplicity, TODO may refactor latter
    public Magic currentMagic; //currently magic selected

    public List<int> unlockedMagics; // an list of current unlocked magics (not used yet)

    public Transform magicSpawnPoint;

    //TODO IMPORTANT: Transfer this to other scripts, each one with one magic behaviour
    #region passive magic variables
    [SerializeField]
    private Material[] playerMaterials;

    //invisibility variables
    [SerializeField]
    private float invisibilityTime = 3f;
    [SerializeField] [Range (0f,1f)]
    private float invisibilityMinAlpha = 0.1f;
    [SerializeField]
    private float invisibilitySpeed = 1f;

    //walk trough walls
    [SerializeField]
    private float wallTime = 3f; // max time you can walk through walls if not inside one
    private bool canWalkTrough; //used to check if time of the wall walk is over.
    private bool isInsideWall = false; //used to check if I am still inside a wall

    [SerializeField]
    private float maxTimeSlowed = 3f;
    private bool timeSlowed = false; //used to check if I am still inside a wall
    #endregion

    // Start is called before the first frame update
    void Start() {
        currentMagic = new Magic();

        unlockedMagics = new List<int>();
        allMagics = new List<Magic>();

        InicializeAllMagics();

        currentMagic = allMagics[0];
    }

    private void InicializeAllMagics() {
        int i = 0;
        Magic magic;
        foreach (GameObject obj in magicPrefabs) {
            magic = new Magic(); //TODO maybe refactor this later to use currentMagic to avoid garbage collector overload
            magic.SetMagic(i, magicPrefabs[i], MagicType.Projectile);
            AddToAllMagics(magic);
            i++;
        }

        magic = new Magic(); //TODO maybe refactor this later to use currentMagic to avoid garbage collector overload
        magic.SetMagic(i, "Invisibility");
        AddToAllMagics(magic);
        i++;

        magic = new Magic(); //TODO maybe refactor this later to use currentMagic to avoid garbage collector overload
        magic.SetMagic(i, "Wall Crossing");
        AddToAllMagics(magic);
        i++;

        magic = new Magic(); 
        magic.SetMagic(i, "Time Stop");
        AddToAllMagics(magic);

    }

    private void AddToAllMagics(Magic magic) {
        allMagics.Add(magic);
    }

    public void UpdateCurrentMagic(int magicId) {
        foreach(Magic magic in allMagics) {
            if (magic.GetMagicId() == magicId) {
                currentMagic = magic;
                currentMagicId = currentMagic.GetMagicId();
            }
        }
    }

    public void UpdateCurrentMagic(string magicName) {
        foreach (Magic magic in allMagics) {
            if (magic.GetMagicString() == magicName) {
                currentMagic = magic;
            }
        }
    }

    public class Magic {
        public MagicType magicType;
        private int id;
        private string magicName;
        private GameObject prefab;

        //uses a name using magicN
        public void SetMagic(int magicId, string magicN, GameObject magicPrefab) {
            id = magicId;
            magicName = magicN;
            prefab = magicPrefab;
        }
        //uses prefab name
        public void SetMagic(int magicId, GameObject magicPrefab, MagicType type) { 
            id = magicId;
            magicType = type;
            prefab = magicPrefab;
        }

        public void SetMagic(int magicId, string magicN) {
            id = magicId;
            magicName = magicN;
            magicType = MagicType.Passive;
        }

        public int GetMagicId() {
            return id;
        }

        public string GetMagicString() {
            return magicName;
        }

        public GameObject GetMagicPrefab() {
            return prefab;
        }

    }

    public enum MagicType {
        Passive,
        Projectile
    }

    #region invisibility

    public IEnumerator WaitInvisibility() {

        float inviTimeLeft = invisibilityTime;
        float currentAlpha = invisibilityMinAlpha;

        while (inviTimeLeft > 0) {
            inviTimeLeft -= Time.deltaTime;
            yield return null;
        }

        while (currentAlpha < 1) {
            currentAlpha += Time.deltaTime * invisibilitySpeed;

            foreach (Material mat in playerMaterials) {
                mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, currentAlpha));
            }

            yield return null;
        }

    }
    //TODO HEAVY AS FUCK change later
    public IEnumerator BecomeInvisible() {

        float currentAlpha = 1f;

        while (currentAlpha > invisibilityMinAlpha) {
            currentAlpha -= Time.deltaTime * invisibilitySpeed;

            foreach (Material mat in playerMaterials) {
                mat.SetColor("_Color", new Color(mat.color.r, mat.color.g, mat.color.b, currentAlpha));
            }

            yield return null;
        }

        StartCoroutine(WaitInvisibility());
    }

    #endregion

    #region Wall Crossing
    public IEnumerator WalkTroughWall() {

        CharacterController controller = GetComponent<CharacterController>();
        gameObject.layer = LayerMask.NameToLayer("Player_NC"); //TODO really fucking bad, cahnge latter

        float wallTimeLeft = wallTime;
        
        canWalkTrough = true;
        controller.detectCollisions = false;// only detects other Rbs or character controllers, not static colliders
        controller.enableOverlapRecovery = false;

        while (wallTimeLeft > 0) {
            wallTimeLeft -= Time.deltaTime;
            yield return null;
        }

        canWalkTrough = false;

        while (isInsideWall) {
            yield return null;
        }

        gameObject.layer = LayerMask.NameToLayer("Player");
        //controller.enableOverlapRecovery = true; TODO uncomment this and find a way to player not snap outside of the walls
        controller.detectCollisions = true;
    }

    private void OnTriggerStay(Collider other) {
        isInsideWall = true;
    }

    private void OnTriggerExit(Collider other) {
        isInsideWall = false;
    }
    #endregion

    #region Time Stop
    public IEnumerator TimeStop() {
        HumanoidBicectAnim playerLegs = Player.player.GetTorsoAnim();
        HumanoidBicectAnim playerTorso = Player.player.GetTorsoAnim();

        float slowedTimeLeft = maxTimeSlowed;

        Time.timeScale = Player.player.timeSlowScale;

        Player.player.timeSlowed = true;

        playerTorso.SetAnimSpeed(2);
        playerLegs.SetAnimSpeed(2);

        while (slowedTimeLeft > 0) {
            slowedTimeLeft -= Time.unscaledDeltaTime;
            yield return null;
        }

        playerTorso.SetAnimSpeed(1);
        playerLegs.SetAnimSpeed(1);

        Time.timeScale = 1;

        Player.player.timeSlowed = false;
    }
    #endregion
}
