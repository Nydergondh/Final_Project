using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement_Test : MonoBehaviour
{
    [SerializeField]
    private float minTimeToWait;
    [SerializeField]
    private float maxTimeToWait = 0.5f;

    public float strafeFactor = 1f;

    public float patrolSpeed = 2.5f;
    public float allertSpeed = 5f;
    public float rotateSpeed = 50f;

    public bool canMove = true;
    private Enemy_Test enemy;

    [SerializeField]
    private List<Transform> wayPoints = new List<Transform>();
    private int currentWayPoint; // used has index for acess the list of way Points
    private int previewsWayPoint;
    public bool reversePathAtEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy_Test>();
        currentWayPoint = 0;
        previewsWayPoint = -1;

        enemy.targetTransform = (wayPoints.Count > 0) ? wayPoints[0]: transform;
        enemy.GetNavAgent().destination = enemy.targetTransform.position;
        //enemy.enemyAnim.SetMoving(false);

        if (enemy.isRanged && enemy.GetNavAgent() != null) {
            enemy.GetNavAgent().updatePosition = false;
            enemy.GetNavAgent().updateRotation = true;
        }
    }
    //TODO rework this method so taht the enemy rotates to look at the position he wants to go, then goes to the pos
    public void Test() {
        if (!canMove) {
            //if not ranged stay still
            if (!enemy.isRanged) {
                enemy.GetNavAgent().isStopped = true;
                enemy.enemyAnim.SetMoving(false);
            }
            //if ranged rotate and shoot player, but stand on the same position
            else {
                if (enemy.fov.seeingPlayer) {
                    enemy.GetNavAgent().isStopped = false;
                    if (enemy.targetTransform == enemy.fov.currentTarget) {
                        enemy.GetNavAgent().destination = enemy.targetTransform.position;
                    }
                    else {
                        ChangeTarget();
                    }
                }
                else {
                    enemy.GetNavAgent().isStopped = true;
                }
            }
        }

        else {

            enemy.GetNavAgent().isStopped = false;

            if (enemy.fov.hearingPlayer && !enemy.fov.seeingPlayer) {
                if (enemy.targetTransform == enemy.fov.currentTarget) {
                    enemy.GetNavAgent().updatePosition = false;
                    enemy.GetNavAgent().destination = enemy.targetTransform.position;
                }
                else {
                    ChangeTarget();
                }
            }

            else if (enemy.fov.seeingPlayer) { // see the player therefore follow him
                if (enemy.targetTransform == enemy.fov.currentTarget) {
                    if (Vector3.Distance(transform.position, enemy.targetTransform.position) > enemy.enemyCombat.minDistToAttack && canMove) {
                        enemy.GetNavAgent().destination = enemy.targetTransform.position; //if the enemy is in range and moving update its position
                        enemy.enemyAnim.SetMoving(true);
                        enemy.GetNavAgent().updatePosition = true;
                    }
                    else {
                        if (enemy.enemyCombat.isAttacking && canMove) {
                            StartCoroutine(WaitToFollow());
                        }
                        //enemy.SetAnimMoving(false);
                    }
                    //if not fall under the condition above then the enemy is attacking the target
                }
                else {
                    ChangeTarget();
                }
            }

            else if (wayPoints.Count == 1 && !enemy.fov.seeingPlayer) { //if enemy is stationary TODO try to fix this
                //if we lost track of the target go to the target last known position
                if (Vector3.Distance(transform.position, enemy.GetNavAgent().destination) > enemy.GetNavAgent().stoppingDistance) {
                    enemy.enemyAnim.SetMoving(true);
                }
                else {
                    enemy.enemyAnim.SetMoving(false);
                }
            }

            else if (Vector3.Distance(transform.position, enemy.targetTransform.position) > enemy.GetNavAgent().stoppingDistance) { //Go To WayPoints when not seeing player
                enemy.GetNavAgent().updatePosition = true;
                enemy.enemyAnim.SetMoving(true);
            }
            else { // Change WayPoints if the IA reached a WayPoint
                ChangeTarget();
            }
        }

        Vector3 productVector = Vector3.Cross(transform.forward, (enemy.targetTransform.position - transform.position).normalized);
        //if is moving and the y of the cross product between velocity and pointToLook is bigger than a threshold hten strafe 
        if (Mathf.Abs(productVector.y) > 1.5) {
            //TODO change latter to just have the above or this condicion
            enemy.enemyAnim.SetStrafe(true);
        }
        else {
            //TODO change latter to just have the above or this condicion
            enemy.enemyAnim.SetStrafe(false);
        }

        //TODO put logic for waitng after reaching waypoint
    }

    public void FollowTarget() {

        if (enemy.targetTransform == Player_Test.player.transform) {
            enemy.GetNavAgent().speed = allertSpeed;
        }
        else {
            enemy.GetNavAgent().speed = patrolSpeed;
        }

        if (!canMove) {
            //if not ranged stay still
            if (!enemy.isRanged) {
                enemy.GetNavAgent().isStopped = true;
                enemy.enemyAnim.SetMoving(false);
            }
            //if ranged rotate and shoot player, but stand on the same position
            else {
                if (enemy.fov.seeingPlayer) {
                    enemy.GetNavAgent().isStopped = false;
                    if (enemy.targetTransform == enemy.fov.currentTarget) {
                        enemy.GetNavAgent().destination = enemy.targetTransform.position;
                    }
                    else {
                        ChangeTarget();
                    }
                }
                else {
                    enemy.GetNavAgent().isStopped = true;
                }
            }
        }

        else {

            if (enemy.targetTransform == Player_Test.player.transform) {
                if (enemy.enemyCombat.isAttacking) {
                    enemy.GetNavAgent().velocity = new Vector3(0, 0, 0);
                    enemy.GetNavAgent().isStopped = false;
                }
                else {
                    enemy.GetNavAgent().isStopped = false;
                    enemy.GetNavAgent().destination = enemy.targetTransform.position;
                    enemy.GetNavAgent().updatePosition = true;
                    enemy.enemyAnim.SetMoving(true);
                }
            }
            else {

                enemy.GetNavAgent().isStopped = false;
                //rotate enemy in case I can only hear him
                if (!enemy.enemyCombat.isAttacking) {

                }
                if (enemy.fov.hearingPlayer && !enemy.fov.seeingPlayer) {
                    if (enemy.targetTransform == enemy.fov.currentTarget) {
                        enemy.GetNavAgent().updatePosition = false;
                        enemy.GetNavAgent().destination = enemy.targetTransform.position;
                    }
                    else {
                        ChangeTarget();
                    }
                }
                // see the player therefore follow him
                else if (enemy.fov.seeingPlayer) {
                    if (enemy.targetTransform == enemy.fov.currentTarget) {
                        if (Vector3.Distance(transform.position, enemy.targetTransform.position) > enemy.enemyCombat.minDistToAttack && canMove) {
                            enemy.GetNavAgent().destination = enemy.targetTransform.position; //if the enemy is in range and moving update its position
                            enemy.enemyAnim.SetMoving(true);
                            enemy.GetNavAgent().updatePosition = true;
                            print("Moving to Player");
                        }
                        //if attacking then stop moving until attack is finished
                        else {
                            print("InRange!!!");
                            if (enemy.enemyCombat.isAttacking && canMove) {
                                print("Stop move to Attack");
                                StartCoroutine(WaitToFollow());
                            }
                        }
                        //if not fall under the condition above then the enemy is attacking the target
                    }
                    else {
                        ChangeTarget();
                    }
                }

                else if (wayPoints.Count <= 1 && !enemy.fov.seeingPlayer) { //if enemy is stationary TODO try to fix this
                    //if we lost track of the target go to the target last known position
                    if (Vector3.Distance(transform.position, enemy.GetNavAgent().destination) > enemy.GetNavAgent().stoppingDistance) {
                        enemy.enemyAnim.SetMoving(true);
                    }
                    else {
                        enemy.enemyAnim.SetMoving(false);
                    }
                }
                //move to WayPoint in Case of not seeing the player
                else if (Vector3.Distance(transform.position, enemy.targetTransform.position) > enemy.GetNavAgent().stoppingDistance) { //Go To WayPoints when not seeing player
                    enemy.GetNavAgent().updatePosition = true;
                    enemy.enemyAnim.SetMoving(true);
                }
                else { // Change WayPoints if the IA reached a WayPoint
                    ChangeTarget();
                }
            }

            Vector3 productVector = Vector3.Cross(transform.forward, (enemy.targetTransform.position - transform.position).normalized);
            //if is moving and the y of the cross product between velocity and pointToLook is bigger than a threshold hten strafe 
            if (Mathf.Abs(productVector.y) > strafeFactor) {
                //playerAnim.SetPlayerStrafe(true);
                //TODO change latter to just have the above or this condicion
                enemy.enemyAnim.SetStrafe(true);
            }
            else {
                //playerAnim.SetPlayerStrafe(false);
                //TODO change latter to just have the above or this condicion
                enemy.enemyAnim.SetStrafe(false);
            }
            //TODO put logic for waitng after reaching waypoint
        }
    }
    
    private void HandleRotation() {
        Vector3 dirToTarget = (enemy.targetTransform.position - transform.position).normalized; //direction to target
        Quaternion newRotation = Quaternion.LookRotation(dirToTarget); //rotation to achieve ideal look position

        if (transform.rotation != newRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime); //rotate towards the object direction
        }
    }

    private void ChangeTarget() {
        enemy.GetNavAgent().Warp(transform.position);

        if (enemy.fov.seeingPlayer || enemy.fov.hearingPlayer) { // check if is seeing or hearing player
            enemy.targetTransform = enemy.fov.currentTarget; // set player has target 
            enemy.GetNavAgent().destination = enemy.targetTransform.position;
        }                                                      //TODO transform targetTransform in to a Vector3
        else {
            SwitchWayPoint();
        }

    }

    private IEnumerator WaitToFollow() {
        if (!enemy.isRanged) {
            canMove = false;
            enemy.enemyAnim.SetMoving(false);

            while (enemy.enemyCombat.isAttacking) {
                yield return null;
            }
            canMove = true;
        }
    }

    private void SwitchWayPoint() {

        if (enemy.targetTransform.gameObject.layer == 8) { //Called when the player was being followed and now need to go back to the old Way Point
            enemy.targetTransform = wayPoints[currentWayPoint];
            enemy.GetNavAgent().destination = enemy.targetTransform.position;
        }
        else {
            if (currentWayPoint > previewsWayPoint && currentWayPoint < wayPoints.Count - 1) { // is going forward
                //print("1");
                previewsWayPoint = currentWayPoint;
                currentWayPoint++;
            }
            else if (currentWayPoint == wayPoints.Count - 1 && currentWayPoint > previewsWayPoint) { // is going forward and now gonna go back to 0
                //print("2");
                if (reversePathAtEnd) {
                    previewsWayPoint = currentWayPoint;
                    currentWayPoint--;
                }
                else {
                    previewsWayPoint = -1;
                    currentWayPoint = 0;
                }
                //previewsWayPoint = currentWayPoint;
                //currentWayPoint--;
            }
            else if (currentWayPoint < previewsWayPoint && currentWayPoint != 0) { // is going backwords to 0
                //print("3");
                previewsWayPoint = currentWayPoint;
                currentWayPoint--;
            }
            else if (currentWayPoint < previewsWayPoint && currentWayPoint == 0) { // is comming backwards to 0 and its time to go forward again
                //print("4");
                if (reversePathAtEnd) {
                    previewsWayPoint = currentWayPoint;
                    currentWayPoint++;
                }
                else {
                    previewsWayPoint = currentWayPoint;
                    currentWayPoint = 0;
                }
                //previewsWayPoint = currentWayPoint;
                //currentWayPoint++;
            }
            enemy.targetTransform = wayPoints[currentWayPoint];
            enemy.GetNavAgent().destination = enemy.targetTransform.position;
        }
    }
}
