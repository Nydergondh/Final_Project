using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHandler : MonoBehaviour
{
    public static List<Magic> allMagics; // all magics currently in the game
    public GameObject[] magicPrefabs;  // objects you want to instanciate when magic is casted

    public int currentMagicId; // done for simplicity, TODO may refactor latter
    public Magic currentMagic; //currently magic selected

    public List<int> unlockedMagics; // an list of current unlocked magics (not used yet)

    public Transform magicSpawnPoint;

    #region passive magic variables
    [SerializeField]
    private Material[] playerMaterials;

    [SerializeField]
    private float invisibilityTime = 3f;
    [SerializeField] [Range (0f,1f)]
    private float invisibilityMinAlpha = 0.1f;
    [SerializeField]
    private float invisibilitySpeed = 1f;
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
}
