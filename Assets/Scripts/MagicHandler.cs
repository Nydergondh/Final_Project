using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHandler : MonoBehaviour
{
    public static List<Magic> allMagics; // all magics currently in the game
    public GameObject[] magicPrefabs;  // objects you want to instanciate when magic is casted

    public Magic currentMagic; //currently magic selected

    public List<int> unlockedMagics; // an list of 

    public Transform magicSpawnPoint;


    // Start is called before the first frame update
    void Start() {
        currentMagic = null;

        unlockedMagics = new List<int>();
        allMagics = new List<Magic>();

        InicializeAllMagics();
    }

    private void InicializeAllMagics() {
        int i = 0;

        Magic magic = new Magic();

        magic.SetMagic(i, "Fire Ball", magicPrefabs[i]);
        AddToAllMagics(magic);

        i++;
    }
    
    private void AddToAllMagics(Magic magic) {
        allMagics.Add(magic);
    }

    //public void UpdateCurrentMagic(Magic magic) {

    //}

    public void UpdateCurrentMagic(int magicId) {
        foreach(Magic magic in allMagics) {
            if (magic.GetMagicId() == magicId) {
                currentMagic = magic;
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
        private int id;
        private string magicName;
        private GameObject prefab;

        public void SetMagic(int magicId, string magicN, GameObject magicPrefab) {
            id = magicId;
            magicName = magicN;
            prefab = magicPrefab;
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


}
