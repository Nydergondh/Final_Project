using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneRestarter : MonoBehaviour
{
    //public bool restarting = false;
    //private int currentScene = 0;
    //public float timeToRestart;


    //private void Update() {
    //    //if (!Player.player.alive && !restarting) {
    //    //    StartCoroutine(RestartGame());
    //    //}
    //    if (!Player_Test.player.alive && !restarting && currentScene < 2) {
    //        StartCoroutine(RestartGame());
    //    }
    //    if (StageManager.INSTANCE.enemyCount <= 0 && !restarting && currentScene <= 2) {
    //        StartCoroutine(StageCleared());
    //    }
    //    if (Input.GetKey(KeyCode.Escape)) {
    //        Application.Quit();
    //    }
    //}
    //private IEnumerator RestartGame() {
    //    restarting = true;
    //    yield return new WaitForSeconds(1f);
    //    SceneManager.LoadScene(currentScene);
    //}

    //private IEnumerator StageCleared() {
    //    restarting = true;
    //    UISingleton.INSTANCE.ShowStageCleared();
    //    if (currentScene == 1) {
    //        SceneManager.LoadScene(2);
    //    }
        
    //}
}
