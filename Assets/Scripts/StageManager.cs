using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int enemyCount = 0;
    public Transform enemiesReference;

    public static StageManager INSTANCE;
    public bool stageCleared = false;

    [HideInInspector]
    public AudioSource musicPlayer;

    public static bool restarting = false;
    private static int currentScene = 1;
    public float timeToRestart;

    // Start is called before the first frame update
    void Awake() {
        if (!INSTANCE) {
            INSTANCE = this;
        }
        else {
            Destroy(gameObject);
        }

        //GetEnemies();

        musicPlayer = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }


    private void Update() {
        if (!Player_Test.player.alive && !restarting && currentScene < 3) {
            StartCoroutine(RestartGame());
        }
        if (enemyCount <= 0 && !restarting && currentScene <= 3) {
            StageCleared();
        }
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }

    private void GetEnemies() {
        foreach (Transform t in enemiesReference) {
            if (t.parent == enemiesReference) {
                enemyCount++;
            }
        }
    }

    private IEnumerator RestartGame() {
        restarting = true;
        yield return new WaitForSeconds(1f);
        restarting = false;
        SceneManager.LoadScene(currentScene);
    }

    public void StageCleared() {
        if (enemyCount <= 0) {
            stageCleared = true;
            restarting = true;
            UISingleton.INSTANCE.ShowStageCleared();
        }
        else {
            stageCleared = false;
        }
    }

    public static void LoadNewScene() {
        if (currentScene == 0) {
            currentScene++;
        }
        else if (currentScene == 1) {
            currentScene++;
        }
        else if (currentScene == 2) {
            currentScene++;
        }
        restarting = false;
        SceneManager.LoadScene(currentScene);
    }

    public void FindEnemyReferece() {
         enemyCount = 0;
         enemiesReference = FindObjectOfType<EnemyReference>().transform;
         print(SceneManager.GetActiveScene().buildIndex);
         print(enemiesReference.transform.name);
    }

    private void OnLevelWasLoaded(int level) {
        if (level != 3) {
            FindEnemyReferece();
            GetEnemies();
        }
        else {
            FinishGame();
        }
    }

    private void FinishGame() {
        enemyCount = 1;
    }
}
