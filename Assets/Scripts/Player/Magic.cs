using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
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
