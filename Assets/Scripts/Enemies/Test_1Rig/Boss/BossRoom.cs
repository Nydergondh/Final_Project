using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField]
    private LayerMask collidableLayer;
    [SerializeField]
    private EnemyBoss enemyBoss;
    [SerializeField]
    private BoxCollider wallOff;

    private void OnTriggerEnter(Collider other) {
        print(other.gameObject);
        if (collidableLayer == (collidableLayer | 1 << other.gameObject.layer) && !enemyBoss.startFight ) {
            if (other.GetComponent<IDamageable>() != null) {
                if(other.gameObject == Player_Test.player.gameObject) {
                    wallOff.enabled = true;
                    enemyBoss.finalShowDown();
                    enemyBoss.StartBoss();

                    Destroy(this);
                }
            }
        }
    }
    
}
