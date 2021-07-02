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
    public Material[] tileMaterials;

    //public Texture2D[] textures;

    //public Texture2D[] normals;

    // Start is called before the first frame update
    void Awake() {

        Vector3 pos;
        GameObject tileObj;
        int randomTile;

        List<int> tileObjects = new List<int>();

        for (int i = 0; i < xSize; i++) {

            for (int j = 0; j < ySize; j++) {

                bool exclude_4_6 = false;
                bool exclude_5_6 = false;
                pos = transform.position;
                pos = new Vector3(pos.x + (tileScaleFactor * j), pos.y, pos.z + (tileScaleFactor * i));

                tileObj = Instantiate(tile, pos, Quaternion.identity, transform);
                _renderer = tileObj.GetComponent<MeshRenderer>();
                try {
                    if (tileObjects.Count > 0) {
                        #region middle_check
                        if (i != 0 && j != 0) {

                            if (tileObjects[(i * xSize) + j - 1] == 5) {
                                exclude_5_6 = true;
                            }

                            else if (tileObjects[(i * xSize) + j - 1] == 6) {
                                exclude_5_6 = true;
                            }

                            if (tileObjects[(i * xSize) + j - xSize] == 4) {
                                exclude_4_6 = true;
                            }

                            else if(tileObjects[(i * xSize) + j - xSize] == 6) {
                                exclude_4_6 = true;
                            }
                        }
                        #endregion

                        #region corners_check
                        else {
                            Debug.Log("VibeCheck");
                            if (i == 0) {
                                if (tileObjects[(i + j) - 1] == 5) {
                                    exclude_5_6 = true;
                                }

                                else if (tileObjects[(i + j) - 1] == 6) {
                                    exclude_5_6 = true;
                                }
                            }
                            else if (j == 0) {
                                if (tileObjects[(i * xSize) + j - xSize] == 4) {
                                    exclude_4_6 = true;
                                }

                                else if (tileObjects[(i * xSize) + j - xSize] == 6) {
                                    exclude_4_6 = true;
                                }
                            }
                        }
                        #endregion

                        #region TEST_delete
                        //if (exclude_4_6 || exclude_5_6) {
                        //    if (exclude_4_6) {
                        //        Debug.Log("exclude_4_6 in " + "i = " + i + " j = " + j);
                        //    }
                        //    if (exclude_5_6) {
                        //        Debug.Log("exclude_5_6 in " + "i = " + i + " j = " + j);
                        //    }
                        //}
                        #endregion

                        #region apply_textures
                        if (!exclude_4_6 && !exclude_5_6) {
                            randomTile = Random.Range(0, tileMaterials.Length);
                            tileObjects.Add(randomTile + 1);
                            ApplyGroundTexture(randomTile);
                        }

                        else if (!exclude_4_6 && exclude_5_6) {
                            randomTile = Random.Range(0, tileMaterials.Length - 2);
                            tileObjects.Add(randomTile + 1);
                            ApplyGroundTexture(randomTile);
                        }

                        else if (exclude_4_6 && !exclude_5_6) {
                            randomTile = Random.Range(0, 5);

                            if (randomTile != 3) {
                                tileObjects.Add(randomTile + 1);
                                ApplyGroundTexture(randomTile);
                            }

                            else {
                                tileObjects.Add(5);
                                ApplyGroundTexture(randomTile + 1);
                            }
                        }

                        else {
                            randomTile = Random.Range(0, tileMaterials.Length - 3);
                            tileObjects.Add(randomTile + 1);
                            ApplyGroundTexture(randomTile);
                        }
                        #endregion
                    }

                    else {
                        randomTile = Random.Range(0, tileMaterials.Length);
                        tileObjects.Add(randomTile + 1);
                        ApplyGroundTexture(randomTile);
                    }

                }
                catch {
                    Debug.Log("Bug at " + i + " " + j +". Number of elements = " +tileObjects.Count);
                    break;
                }

            }
        }
        tileObjects.Clear();
    }

    private void ApplyGroundTexture(int tile) {
        try {
            _renderer.sharedMaterial = tileMaterials[tile];
        }
        catch {
            print(tile);
        }
    }
}
