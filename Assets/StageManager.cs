using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int enemyCount = 0;
    public Transform enemiesReference;

    public static StageManager INSTANCE;
    public bool stageCleared = false;


    // Start is called before the first frame update
    void Awake()
    {
        if(!INSTANCE) {
            INSTANCE = this;
        }
        else {
            Destroy(gameObject);
        }

        foreach (Transform t in enemiesReference) {
            if (t.parent == enemiesReference) {
                enemyCount++;
            }
        }
    }

    public void StageCleared() {
        if (enemyCount <= 0) {
            stageCleared = true;
        }
        else {
            stageCleared = false;
        }
    }
}
