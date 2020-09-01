using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float timeToWait = 0;
    [SerializeField]
    private float minTimeToWait;
    [SerializeField]
    private float maxTimeToWait;

    public float moveSpeed = 5f;
    public float rotateSpeed = 50f;

    private Enemy enemy;

    [SerializeField]
    private HumanoidBicectAnim torsoAnim;
    [SerializeField]
    private HumanoidBicectAnim legsAnim;

    [SerializeField]
    private List<Transform> wayPoints = new List<Transform>();
    private int currentWayPoint; // used has index for acess the list of way Points
    private int previewsWayPoint;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        currentWayPoint = 0;
        previewsWayPoint = -1;

        enemy.targetTransform = wayPoints[0];
    }

    // Update is called once per frame

    public void FollowTarget() {
        //TODO probrably tehre is a better place to put ChangeTarget
        if (enemy.fov.seeingPlayer) { // see the player therefore follow him
            if (enemy.targetTransform == enemy.fov.currentTarget) {
                if (Vector3.Distance(transform.position, enemy.targetTransform.position) > enemy.enemyCombat.minDistToAttack) {
                    transform.position += (enemy.targetTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                    HandleRotation();
                    enemy.SetAnimMoving(true);
                }
                //if not fall under the condition above then the enemy is attacking the target
            }
            else {
                ChangeTarget();
            }
        }

        else if (!enemy.fov.seeingPlayer && enemy.targetTransform.gameObject.layer == 8) { //was seeing player and now lost track of it (go back to way points)
            ChangeTarget();
        }

        else if (Vector3.Distance(transform.position, enemy.targetTransform.position) > 0.25f) { //Go To WayPoints when not seeing player
            transform.position += (enemy.targetTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            HandleRotation();
            enemy.SetAnimMoving(true);
        }
        else { // Change WayPoints if the IA reached a WayPoint
            ChangeTarget(); 
        }


        Vector3 productVector = Vector3.Cross(transform.forward, (enemy.targetTransform.position - transform.position).normalized);
        //if is moving and the y of the cross product between velocity and pointToLook is bigger than a threshold hten strafe 
        if (Mathf.Abs(productVector.y) > 1.5) {
            //playerAnim.SetPlayerStrafe(true);
            //TODO change latter to just have the above or this condicion
            enemy.SetAnimStrafing(true);
        }
        else {
            //playerAnim.SetPlayerStrafe(false);
            //TODO change latter to just have the above or this condicion
            enemy.SetAnimStrafing(false);
        }

        //TODO put logic for waitng after reaching waypoint
    }
    
    private void HandleRotation() {
        Vector3 dirToTarget = (enemy.targetTransform.position - transform.position).normalized; //direction to target
        Quaternion newRotation = Quaternion.LookRotation(dirToTarget); //rotation to achieve ideal look position

        if (transform.rotation != newRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotateSpeed * Time.deltaTime); //rotate towards the object direction
        }

    }


    private void ChangeTarget() {
        if (enemy.fov.seeingPlayer) { // check if is seeing the player
            enemy.targetTransform = enemy.fov.currentTarget; // set player has target 
        }                                                      //TODO transform targetTransform in to a Vector3
        else {
            SwitchWayPoint();
        }
    }

    private void SwitchWayPoint() {

        if (enemy.targetTransform.gameObject.layer == 8) { //Called when the player was being followed and now need to go back to the old Way Point
            enemy.targetTransform = wayPoints[currentWayPoint];
        }

        else {
            if (currentWayPoint > previewsWayPoint && currentWayPoint < wayPoints.Count - 1) { // is going forward
                previewsWayPoint = currentWayPoint;
                currentWayPoint++;
            }

            else if (currentWayPoint == wayPoints.Count - 1 && currentWayPoint > previewsWayPoint) { // is going forward and now gonna go back to 0
                previewsWayPoint = currentWayPoint;
                currentWayPoint--;
            }

            else if (currentWayPoint < previewsWayPoint && currentWayPoint != 0) { // is going backwords to 0
                previewsWayPoint = currentWayPoint;
                currentWayPoint--;
            }
            else if (currentWayPoint < previewsWayPoint && currentWayPoint == 0) { // is comming backwards to 0 and its time to go forward again
                previewsWayPoint = currentWayPoint;
                currentWayPoint++;
            }
            enemy.targetTransform = wayPoints[currentWayPoint];
        }

    }
}
