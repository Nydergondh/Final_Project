using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement_Test : MonoBehaviour
{
    private CharacterController playerControler;
    private PlayerAnimations playerAnim;

    [SerializeField]
    private Transform playerMesh;

    [SerializeField]
    private LayerMask groundMask;

    public float moveSpeed = 3f;
    [SerializeField]
    private float strafeTreshhold = 1.5f;

    // Start is called before the first frame update
    void Start() {
        playerControler = GetComponent<CharacterController>();
        playerAnim = GetComponent<PlayerAnimations>();
    }

    public void Move() {
        float xSpeed, zSpeed;
        Vector3 movement;

        Vector3 pointToLook;
        Vector3 velocityVector;
        Vector3 productVector;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundPoint;
        float rayLength = Mathf.Infinity;

        if (Physics.Raycast(cameraRay, out groundPoint, rayLength, groundMask)) {
            pointToLook = groundPoint.point;
        }
        else {
            pointToLook = transform.position;
        }

        if (!Player_Test.player.invertControls) {
            xSpeed = Input.GetAxis("Horizontal");
            zSpeed = Input.GetAxis("Vertical");
        }
        else {
            xSpeed = Input.GetAxis("Horizontal") * -1;
            zSpeed = Input.GetAxis("Vertical") * -1;
        }

        //do all the move logic and calculation if the player recive any movement input at all
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            movement = new Vector3(xSpeed * moveSpeed, 0, zSpeed * moveSpeed);
            movement *= (Mathf.Abs(xSpeed) == 1 && Mathf.Abs(zSpeed) == 1) ? 0.7f : 1; //set the movement vector to 0.7 if player is moving on both axis
            if (Player_Test.player.timeSlowed) {
                movement *= 2;//TODO IMPORTANT Change this to have more concistency (do math son)
            }
            velocityVector = new Vector3(movement.x + transform.position.x, 0, movement.z + transform.position.z);

            playerControler.SimpleMove(movement);

            //TODO change latter to just have the above or this condicion
            playerAnim.SetMoving(true);

        }
        else {
            velocityVector = Vector3.zero;
            //playerAnim.SetMoving(false);

            //TODO see the equivalent to this in the animator parameters
            //if (Player_Test.player.isAttacking) {
            //    //player.GetLegsAnim().SetAttack(false);
            //}

            playerAnim.SetMoving(false);

        }
        playerMesh.LookAt(pointToLook);

        productVector = Vector3.Cross(pointToLook - transform.position, velocityVector - transform.position);
        //if is moving and the y axis of the result cross product between velocity and pointToLook is bigger than a threshold then strafe 
        if (Mathf.Abs(velocityVector.magnitude) > 0.1f && Mathf.Abs(productVector.y) > strafeTreshhold) {
            //playerAnim.SetPlayerStrafe(true);
            //TODO change latter to just have the above or this condicion
            playerAnim.SetStrafe(true);
        }
        else {
            //playerAnim.SetPlayerStrafe(false);
            //TODO change latter to just have the above or this condicion
            playerAnim.SetStrafe(false);
        }

        Debug.DrawLine(playerMesh.position, pointToLook, Color.red); // player to mouse
        //Debug.DrawLine(transform.position, pointToLook, Color.red); // player to mouse
        //Debug.DrawLine(velocityVector, transform.position, Color.yellow); //player speed
        //Debug.DrawLine(velocityVector, pointToLook, Color.blue); // (mouse - player speed) vector
    }

    //pass a vector to shrink (targetVector) to a magnitude
    private Vector3 ShrinkVector(Vector3 targetVect, float magnitudeTarget) {
        float magnitudeDiference = Mathf.Abs(targetVect.magnitude - magnitudeTarget);
        targetVect /= magnitudeDiference;
        return targetVect;
    }

}
