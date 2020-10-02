using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlableMissile : MonoBehaviour, IDamage
{
    [SerializeField]
    private LayerMask groundMask;
    [SerializeField]
    private LayerMask collidableLayer;

    [SerializeField]
    private float projSpeed = 25f;
    [SerializeField]
    private float projRotateSpeed = 200f;

    public bool isActive = true;

    [SerializeField]
    private int damage = 1;

    private Vector3 pointToLook;

    private Collider col;

    private void Start() {
        col = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * projSpeed * Time.deltaTime;
        if (isActive) {
            //add restriction for when holding mouse botton
            HandleRotation();
            
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (collidableLayer == (collidableLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage());
                Destroy(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }


    private void HandleRotation() {

        SetMouseWorldPos();

        Vector3 dirToTarget = (pointToLook - transform.position).normalized; //direction to target
        Quaternion newRotation = Quaternion.LookRotation(dirToTarget); //rotation to achieve ideal look position

        if (transform.rotation != newRotation) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, projRotateSpeed * Time.deltaTime); //rotate towards the object direction
        }

    }

    private void SetMouseWorldPos() { 

        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundPoint;
        float rayLength = Mathf.Infinity;

        if (Physics.Raycast(cameraRay, out groundPoint, rayLength, groundMask)) {
            pointToLook = new Vector3(groundPoint.point.x, transform.position.y ,groundPoint.point.z);
        }
        else {
            pointToLook = transform.position;
        }

    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * projSpeed);
    }

    public int GetDamage() {
        return damage;
    }
}
