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
    [SerializeField]
    private LayerMask enemyMask;

    public float moveSpeed = 3f;
    public float pushPower = 5f;
    [SerializeField]
    private float strafeTreshhold = 1.5f;

    public float GroundDistance = 0.2f;
    public bool makingNoise = false;

    private Vector3 _velocity;
    public bool _isGrounded = false;

    public bool _isMoving = false;
    public float timeToMakeNoise = 1f;
    private float currentNoiseTime = 0;

    // Start is called before the first frame update
    void Start() {
        playerControler = GetComponent<CharacterController>();
        playerAnim = GetComponent<PlayerAnimations>();
    }

    public void Move() {
        float xSpeed, zSpeed;
        Vector3 movement;

        Vector3 pointToLook;
        Vector3 productVector;

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundPoint;
        float rayLength = Mathf.Infinity;

        if (Physics.Raycast(cameraRay, out groundPoint, rayLength, groundMask)) {
            pointToLook = new Vector3(groundPoint.point.x, transform.position.y, groundPoint.point.z);
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


        //Grounded Logic (to Do gravity)
        _isGrounded = Physics.CheckSphere(transform.position, GroundDistance, groundMask, QueryTriggerInteraction.Ignore);
        if (_isGrounded && _velocity.y < 0)
            _velocity.y = 0f;

        //do all the move logic and calculation if the player recive any movement input at all
        if (Input.GetButton("Horizontal") || Input.GetButton("Vertical")) {
            _isMoving = true;
            movement = new Vector3(xSpeed * moveSpeed, 0, zSpeed * moveSpeed);
            movement *= (Mathf.Abs(xSpeed) == 1 && Mathf.Abs(zSpeed) == 1) ? 0.7f : 1; //set the movement vector to 0.7 if player is moving on both axis
            if (Player_Test.player.timeSlowed) {
                movement *= 2;//TODO IMPORTANT Change this to have more concistency (do math son)
            }

            _velocity = new Vector3(movement.x , 0, movement.z);
            //TODO change latter to just have the above or this condicion
            playerAnim.SetMoving(true);
        }
        else {
            _isMoving = false;
            _velocity = Vector3.zero;
            playerAnim.SetMoving(false);
        }
        //add gravity
        _velocity.y += Physics.gravity.y;

        playerControler.Move(_velocity * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(pointToLook - transform.position, transform.up);
        if (transform.rotation.x  != 0 || transform.rotation.z != 0) {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y ,0);
        }

        productVector = Vector3.Cross(pointToLook - transform.position, _velocity - transform.position);
        //if is moving and the y axis of the result cross product between velocity and pointToLook is bigger than a threshold then strafe 
        if (Mathf.Abs(_velocity.magnitude) > 0.1f && Mathf.Abs(productVector.y) > strafeTreshhold) {
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

    public void MakingNoise() {
        if (_isMoving && currentNoiseTime < timeToMakeNoise) {
            currentNoiseTime += Time.deltaTime;
        }
        else if (_isMoving && currentNoiseTime > timeToMakeNoise) {
            currentNoiseTime = timeToMakeNoise;
            Player_Test.player._isMakingNoise = true;
        }
        //TODO: Add time to stop making noise here
        else if (!_isMoving) {
            currentNoiseTime = 0;
            Player_Test.player._isMakingNoise = false;
        }
    }
    

    void OnControllerColliderHit(ControllerColliderHit hit) {

        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic) {
            return;
        }

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3) {
            return;
        }

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        //print(hit.transform.name);
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        if (enemyMask == (enemyMask | 1 << hit.transform.gameObject.layer)) {
            body.velocity = pushDir * pushPower * 5;
        }
        else {
            body.velocity = pushDir * pushPower;
        }
    }

    //pass a vector to shrink (targetVector) to a magnitude
    private Vector3 ShrinkVector(Vector3 targetVect, float magnitudeTarget) {
        float magnitudeDiference = Mathf.Abs(targetVect.magnitude - magnitudeTarget);
        targetVect /= magnitudeDiference;
        return targetVect;
    }

}
