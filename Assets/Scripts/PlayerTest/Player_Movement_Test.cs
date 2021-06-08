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

    private float currentTimeToStopNoise = 0f;
    public float timeToStopNoise = 0.5f;

    // Start is called before the first frame update
    void Start() {
        playerControler = GetComponent<CharacterController>();
        playerAnim = GetComponent<PlayerAnimations>();
    }

    public void Move() {
        float xSpeed, zSpeed, angle;
        Vector3 movement;

        Vector3 pointToLook;

        Vector2 lookVector;
        Vector2 velocityVector;
        Vector2 currentVector2Pos = new Vector2(transform.position.x, transform.position.z);

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundPoint;
        float rayLength = Mathf.Infinity;

        if (Physics.Raycast(cameraRay, out groundPoint, rayLength, groundMask)) {
            pointToLook = new Vector3(groundPoint.point.x, transform.position.y, groundPoint.point.z);
        }
        else {
            pointToLook = transform.position;
        }

        lookVector = new Vector2(pointToLook.x , pointToLook.z);

        if (!Player_Test.player.invertControls) {
            xSpeed = Input.GetAxisRaw("Horizontal");
            zSpeed = Input.GetAxisRaw("Vertical");
        }
        else {
            xSpeed = Input.GetAxisRaw("Horizontal") * -1;
            zSpeed = Input.GetAxisRaw("Vertical") * -1;
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
        velocityVector = new Vector2(transform.position.x + _velocity.x, transform.position.z + _velocity.z);
        //add gravity
        _velocity.y += Physics.gravity.y;

        playerControler.Move(_velocity * Time.deltaTime);

        transform.rotation = Quaternion.LookRotation(pointToLook - transform.position, transform.up);
        if (transform.rotation.x  != 0 || transform.rotation.z != 0) {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y ,0);
        }
        //if is moving and the y axis of the result cross product between velocity and pointToLook is bigger than a threshold then strafe 
        angle = Vector2.Angle(velocityVector - currentVector2Pos, lookVector - currentVector2Pos);
        if (Mathf.Abs(_velocity.magnitude) > 0.1f && (angle >= 30 && angle <= 150)) {
            //TODO change latter to just have the above or this condicion
            playerAnim.SetStrafe(true);
        }
        else {
            //TODO change latter to just have the above or this condicion
            playerAnim.SetStrafe(false);
        }

        #region debugs_DrawLine
        Debug.DrawLine(playerMesh.position, pointToLook, Color.red); // player to mouse
        Debug.DrawLine(playerMesh.position, velocityVector, Color.blue);
        //Debug.DrawLine(transform.position, pointToLook, Color.red); // player to mouse
        //Debug.DrawLine(velocityVector, transform.position, Color.yellow); //player speed
        //Debug.DrawLine(velocityVector, pointToLook, Color.blue); // (mouse - player speed) vector
        #endregion
    }

    public void MakingNoise() {
        if (!_isMoving) {
            if (currentTimeToStopNoise < timeToStopNoise) {
                currentTimeToStopNoise += Time.deltaTime;
                makingNoise = true;
                Player_Test.player._isMakingNoise = true;
            }
            else {
                currentTimeToStopNoise = timeToStopNoise;
                makingNoise = false;
                Player_Test.player._isMakingNoise = false;
            }
        }
        else {
            currentTimeToStopNoise = 0;
            makingNoise = true;
            Player_Test.player._isMakingNoise = true;
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

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2) {
        Vector2 vec1Rotated90 = new Vector2(-vec1.y, vec1.x);
        float sign = (Vector2.Dot(vec1Rotated90, vec2) < 0) ? -1.0f : 1.0f;
        return Vector2.Angle(vec1, vec2) * sign;
    }

}
