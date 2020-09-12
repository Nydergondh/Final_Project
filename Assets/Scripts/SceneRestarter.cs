using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneRestarter : MonoBehaviour
{
    private bool restarting = false;

    private void Update() {
        if (!Player.player.alive && !restarting) {
            StartCoroutine(RestartGame());
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
}
