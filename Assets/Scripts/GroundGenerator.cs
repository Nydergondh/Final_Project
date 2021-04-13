using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundGenerator : MonoBehaviour
{
    public int xSize;
    public int ySize;

    public float tileScaleFactor = 0.1f;

    public GameObject tile;
    public Renderer renderer;
    public Texture2D[] textures;

    // Start is called before the first frame update
    void Awake()
    {
        Vector3 pos;
        GameObject tileObj;
        for (int i = 0; i < xSize; i++) {
            for (int j = 0; j < ySize; j++) {
                pos = transform.position;
                print(pos);
                pos = new Vector3(pos.x + (1f * i), pos.y, pos.z + (1f * j));
                tileObj = Instantiate(tile, pos, Quaternion.identity ,transform);
                renderer = tileObj.GetComponent<Renderer>();
                renderer.material.SetTexture("_MainTexture", textures[Random.Range(0, textures.Length)]);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
