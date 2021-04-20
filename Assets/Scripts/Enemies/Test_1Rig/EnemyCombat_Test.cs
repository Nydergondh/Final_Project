using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat_Test : MonoBehaviour
{
    private Enemy_Test enemy;

    public bool isAttacking = false;
    public float coolDownRangedAttack = 1f;
    public bool coolDown = false;

    public float minDistToAttack = 0.5f;
    private int attackType = 1;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy_Test>();
        if (enemy.isRanged) {
            minDistToAttack = enemy.fov.viewRadius;
        }

    }

    public void UnsetAttack() {
        isAttacking = false;
        StartCoroutine(AttackCoolDown());
    }

    IEnumerator AttackCoolDown() {
        coolDown = true;
        enemy.GetNavAgent().isStopped = false;
        yield return new WaitForSeconds(coolDownRangedAttack);
        coolDown = false;
    }

    public void AttackTarget() { //TODO maybe change name latter
        //melle behavior
        if (!enemy.isRanged) {

        // we see and hear
            if (enemy.fov.seeingPlayer && enemy.fov.hearingPlayer) {
                //in attack distance
                if (Vector3.Distance(transform.position, enemy.targetTransform.position) <= minDistToAttack) { 
                    if (!isAttacking) {
                        enemy.enemyAnim.SetAttack(true);
                        isAttacking = true;
                    }
                }
            }
        }

        //ranged behavior
        else {
            if (enemy.fov.seeingPlayer) {
                print(enemy.GetNavAgent().updatePosition);
                if (Vector3.Distance(transform.position, enemy.targetTransform.position) <= minDistToAttack) {
                    if (!isAttacking && !coolDown) {
                        enemy.enemyAnim.SetAttack(true);
                        isAttacking = true;

                        //enemy.GetNavAgent().Warp(transform.position);
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
