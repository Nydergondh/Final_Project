using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    private PlayerAnimations playerAnim;

    private Player player;

    [SerializeField]
    private Transform playerMesh;

    [SerializeField]
    private LayerMask groundMask;

    public float moveSpeed = 3f;
    [SerializeField]
    private float strafeTreshhold = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnim = GetComponent<PlayerAnimations>();
        player = GetComponent<Player>();
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


        xSpeed = Input.GetAxis("Horizontal");
        zSpeed = Input.GetAxis("Vertical");

        //do all the move logic and calculation if the player recive any movement input at all
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            movement = new Vector3(xSpeed * moveSpeed, 0, zSpeed * moveSpeed);
            movement *= (Mathf.Abs(xSpeed) == 1 && Mathf.Abs(zSpeed) == 1) ? 0.7f : 1; //set the movement vector to 0.7 if player is moving on both axis
            velocityVector = new Vector3(movement.x + transform.position.x, 0, movement.z + transform.position.z);

            controller.SimpleMove(movement);

            //TODO change latter to just have the above or this condicion
            player.SetAnimMoving(true);

        }
        else {
            velocityVector = Vector3.zero;
            //playerAnim.SetMoving(false);

            //TODO change latter to just have the above or this condicion
            if (player.GetLegsAnim().GetAttack() && player.GetLegsAnim().GetMoving()) {
                player.GetLegsAnim().SetAttack(false);
            }

            player.SetAnimMoving(false);

        }

        playerMesh.LookAt(pointToLook);
        
        productVector = Vector3.Cross(pointToLook - transform.position, velocityVector - transform.position);
        //if is moving and the y axis of the result cross product between velocity and pointToLook is bigger than a threshold then strafe 
        if (Mathf.Abs(velocityVector.magnitude) > 0.1f && Mathf.Abs(productVector.y) > strafeTreshhold) {
            //playerAnim.SetPlayerStrafe(true);
            //TODO change latter to just have the above or this condicion
            player.SetAnimStrafing(true);
        }
        else {
            //playerAnim.SetPlayerStrafe(false);
            //TODO change latter to just have the above or this condicion
            player.SetAnimStrafing(false);
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
