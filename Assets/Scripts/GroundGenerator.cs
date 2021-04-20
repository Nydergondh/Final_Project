using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public int xSize;
    public int ySize;

    public float tileScaleFactor = 0.1f;

    public GameObject tile;
    public MeshRenderer _renderer;
    public Texture2D[] textures;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 pos;
        GameObject tileObj;
        for (int i = 0; i < xSize; i++) {
            for (int j = 0; j < ySize; j++) {
                pos = transform.position;
                pos = new Vector3(pos.x + (1f * i), pos.y, pos.z + (1f * j));
                tileObj = Instantiate(tile, pos, Quaternion.identity ,transform);
                _renderer = tileObj.GetComponent<MeshRenderer>();
                print(_renderer.name);
                _renderer.material.SetTexture("_MainTexture", textures[Random.Range(0, textures.Length)]);
            }
        }
    }
}
