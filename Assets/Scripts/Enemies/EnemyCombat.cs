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
        //melle behavior
        if (!enemy.isRanged) {

        // if the target is in range and not attacking already
            if (enemy.fov.seeingPlayer && enemy.fov.hearingPlayer) {
            
                if (Vector3.Distance(transform.position, enemy.targetTransform.position) <= minDistToAttack) {
                    if (enemy.isAttacking) {
                        enemy.GetNavAgent().isStopped = true;
                    }
                    else {
                        enemy.SetAnimAttack(true);
                        enemy.isAttacking = true;
                    }
                }
                else {
                    enemy.GetNavAgent().isStopped = false;
                }
            }
            else {
                enemy.GetNavAgent().isStopped = false;
            }
        }

        //ranged behavior
        else {
            if (enemy.fov.seeingPlayer) {
                if (Vector3.Distance(transform.position, enemy.targetTransform.position) <= minDistToAttack) {
                    if (!enemy.isAttacking) {
                        enemy.SetAnimAttack(true);
                        enemy.isAttacking = true;
                        enemy.GetNavAgent().Warp(transform.position);
                    }
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

}
