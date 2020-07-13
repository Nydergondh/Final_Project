using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    private CharacterController controller;
    private PlayerAnimations playerAnim;
    [SerializeField]
    private Transform playerMesh;

    public float moveSpeed = 3f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAnim = GetComponent<PlayerAnimations>();
    }

    // Update is called once per frame
    void Update()
    {
        float xSpeed, zSpeed;
        Vector3 movement;

        //not sure how the z value works
        Vector3 pointToLook = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y));
        Vector3 velocityVector;
        Vector3 productVector;

        xSpeed = Input.GetAxis("Horizontal");
        zSpeed = Input.GetAxis("Vertical");

        //do all the move logic and calculation if the player recive any movement input at all
        if (xSpeed != 0 || zSpeed != 0) { 
            movement = new Vector3( xSpeed * moveSpeed, 0, zSpeed * moveSpeed);
            movement *= (Mathf.Abs(xSpeed) == 1 && Mathf.Abs(zSpeed) == 1) ? 0.7f : 1; //set the movement vector to 0.7 if player is moving on both axis
            velocityVector = new Vector3(movement.x + transform.position.x, 0 ,movement.z + transform.position.z);

            controller.SimpleMove(movement);
            playerAnim.SetMoving(true);
            //playerAnim.SetVelocity(new Vector2(xSpeed, zSpeed));

        }
        else {
            velocityVector = Vector3.zero;
            playerAnim.SetMoving(false);
        }

        playerMesh.LookAt(pointToLook);

        productVector = Vector3.Cross(pointToLook - transform.position, velocityVector - transform.position);
        //if is moving and the y of the cross product between velocity and pointToLook is bigger than a threshold hten strafe 
        if (Mathf.Abs(velocityVector.magnitude) > 0.1f && Mathf.Abs(productVector.y) > 1.5) {
                playerAnim.SetPlayerStrafe(true);
        }
        else {
            playerAnim.SetPlayerStrafe(false);
        }

        //clear console
        if (Input.GetKeyDown(KeyCode.Delete)) {
            Utils.ClearLogConsole();
        }

        Debug.DrawLine(transform.position, pointToLook, Color.red); // player to mouse
        Debug.DrawLine(velocityVector, transform.position, Color.yellow); //player speed
        Debug.DrawLine(velocityVector, pointToLook, Color.blue); // (mouse - player speed) vector
        
    }

    private void OnDrawGizmosSelected() {
        

    }

    //pass a vector to shrink (targetVector) to a magnitude
    private Vector3 ShrinkVector(Vector3 targetVect, float magnitudeTarget) {
        float magnitudeDiference = Mathf.Abs(targetVect.magnitude - magnitudeTarget);
        targetVect /= magnitudeDiference;
        return targetVect;
    }

}
