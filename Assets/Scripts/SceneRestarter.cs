using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneRestarter : MonoBehaviour
{
    private bool restarting = false;
    public float timeToRestart;

    private void Update() {
        //if (!Player.player.alive && !restarting) {
        //    StartCoroutine(RestartGame());
        //}
        if (!Player_Test.player.alive && !restarting) {
            StartCoroutine(RestartGame());
        }
        if (StageManager.INSTANCE.enemyCount <= 0 && !restarting) {
            StartCoroutine(StageCleared());
        }
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }
    }
    private IEnumerator RestartGame() {
        restarting = true;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(0);
    }

    private IEnumerator StageCleared() {
        restarting = true;
        UISingleton.INSTANCE.ShowStageCleared();
        yield return new WaitForSeconds(timeToRestart);
        SceneManager.LoadScene(0);
    }
}
