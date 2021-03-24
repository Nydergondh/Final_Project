using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView_Test : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Enemy_Test enemy { get; set; }
    public Transform currentTarget;

    public bool seeingPlayer;
    public bool hearingPlayer;
    public float hearingRange = 2f;

    void Start() {
        seeingPlayer = false;
        enemy = GetComponent<Enemy_Test>();
        StartCoroutine(FindTargetsWithDelay(0.2f));
    }

    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds(delay);
            FindVisibleTarget();
        }
    }

    public void FindVisibleTarget() {
        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        //check if there are any collisions inside the IA sphere of influence

        if (targetInViewRadius.Length > 0) {
            for (int i = 0; i < targetInViewRadius.Length; i++) {
                Transform target = targetInViewRadius[i].transform;
                if (target.GetComponent<IDamageable>() != null) { //if the target can be damaged (Done so that we can't target another object)
                    float dstToTarget = Vector3.Distance(transform.position, target.position);
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    //check if the player inside the sphere of influence is within the angle of vision of the AI
                    IsHearingPlayer(target);
                    if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2) {
                        //casts a ray to the players position to see if he is behind a wall
                        if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                            if (!seeingPlayer && !enemy.targetTransform == Player_Test.player.transform) {
                                enemy.GetNavAgent().Warp(transform.position);
                            }
                            currentTarget = target;
                            seeingPlayer = true;
                            //if the AI pass in every test it will return "see the player and follow him"
                            return;
                        }
                    }
                }
            }
        }
        //if the IA fail one of the tests it will be set to not see the player (so it will not follow him)
        if (!hearingPlayer) {
            currentTarget = null;
        }
        seeingPlayer = false;

    }

    public bool IsHearingPlayer(Transform playerTransform) {
        float dstToTarget = Vector3.Distance(transform.position, playerTransform.position);
        //check if the player inside the sphere of influence is within the angle of vision of the AI
        if (seeingPlayer) {
            hearingPlayer = true;
        }
        else {
            if (dstToTarget <= hearingRange && Player_Test.player._isMakingNoise) {
                hearingPlayer = true;
                currentTarget = playerTransform;
            }
            else {
                hearingPlayer = false;
            }
        }
        return hearingPlayer;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
