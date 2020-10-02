using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour, IDamage
{
    [SerializeField]
    private LayerMask collidableLayer;
    //[SerializeField]
    //private LayerMask collidableObjects;

    [SerializeField]
    private float fireBallSpeed = 5f;
    [SerializeField]
    private int damage = 1;

    private Collider col;

    private void Start() {
        col = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * fireBallSpeed * Time.deltaTime;
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * fireBallSpeed);
    }

    public int GetDamage() {
        return damage;
    }
}
