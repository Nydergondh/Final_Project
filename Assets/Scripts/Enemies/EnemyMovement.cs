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

    public Enemy enemy;

    private Transform targetTransform;

    [SerializeField]
    private List<Transform> wayPoints = new List<Transform>();
    private int currentWayPoint; // used has index for acess the list of way Points
    private int previewsWayPoint;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Enemy>();
        currentWayPoint = 0;
        previewsWayPoint = -1;

        targetTransform = wayPoints[0];
    }

    // Update is called once per frame

    public void FollowTarget() {
        //TODO probrably tehre is a better place to put ChangeTarget

        if (enemy.fov.seeingPlayer) { // see the player therefore follow him
            print(targetTransform +" + " + enemy.fov.currentTarget);
            if (targetTransform == enemy.fov.currentTarget) {
                if (Vector3.Distance(transform.position, targetTransform.position) > 0.25f) {
                    transform.position += (targetTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
                }
            }
            else {
                ChangeTarget();
            }
        }

        else if (!enemy.fov.seeingPlayer && targetTransform.gameObject.layer == 8) { //was seeing player and now lost track of it (go back to way points)
            ChangeTarget();
        }

        else if (Vector3.Distance(transform.position, targetTransform.position) > 0.25f) { //Go To WayPoints when not seeing player
            transform.position += (targetTransform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
        }
        else { // Change WayPoints if the IA reached a WayPoint
            ChangeTarget(); 
        }

        //TODO put logic for waitng after reaching waypoint
    }

    private void ChangeTarget() {
        if (enemy.fov.seeingPlayer) { // check if is seeing the player
            targetTransform = enemy.fov.currentTarget; // set player has target 
        }                                                      //TODO transform targetTransform in to a Vector3
        else {
            SwitchWayPoint();
        }
    }

    private void SwitchWayPoint() {

        if (targetTransform.gameObject.layer == 8) { //Called when the player was being followed and now need to go back to the old Way Point
            targetTransform = wayPoints[currentWayPoint];
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
            targetTransform = wayPoints[currentWayPoint];
        }

    }
}
