using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour {
    public Transform spawnPosition;
    public Stairs nextSpawn;
    private static bool transitioning = false; 
    [Space]

    [SerializeField]
    private LayerMask collidableLayer;

    private IEnumerator NextFloor() {
        transitioning = true;
        UISingleton.INSTANCE.FadeIn();
        Player_Test.player.canMove = false;

        yield return new WaitForSeconds(2f);
        print(Player_Test.player.transform.position);
        Player_Test.player.WarpPlayerToPos(this);


        print(Player_Test.player.transform.position);
        UISingleton.INSTANCE.FadeOut();
        Player_Test.player.canMove = true;
        transitioning = false;
    }

    private void OnTriggerEnter(Collider other) {
        if (!transitioning) {
            if (collidableLayer == (collidableLayer | 1 << other.gameObject.layer)) {
                if (other.gameObject == Player_Test.player.gameObject) {
                    print(other.gameObject);
                    StartCoroutine(NextFloor());
                }
            }
        }
    }
}
