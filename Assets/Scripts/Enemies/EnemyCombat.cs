using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private Enemy enemy;

    public float minDistToAttack;
    private int attackType = 1;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    public void AttackTarget() { //TODO maybe change name latter
        // if the target is in range and not attacking already
        if (enemy.fov.seeingPlayer) {
            //print(Vector3.Distance(transform.position, enemy.targetTransform.position));
            if (Vector3.Distance(transform.position, enemy.targetTransform.position) <= minDistToAttack && !enemy.isAttacking ) {
                enemy.SetAnimAttack(true);
                enemy.isAttacking = true;
                enemy.GetNavAgent().isStopped = true;
            }
            else if(enemy.isAttacking && Vector3.Distance(transform.position, enemy.targetTransform.position) <= minDistToAttack) {
                enemy.GetNavAgent().isStopped = true;
            }
            else {
                enemy.GetNavAgent().isStopped = false;
            }
        }
        else {
            enemy.GetNavAgent().isStopped = false;
        }

    }

}
