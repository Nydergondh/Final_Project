﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProjectile : MonoBehaviour, IDamage {
    [SerializeField]
    private LayerMask collidableLayer;

    [SerializeField]
    private float projectileSpeed = 5f;
    [SerializeField]
    private int damage = 1;

    private Collider col;

    private void Start() {
        col = GetComponentInChildren<Collider>();
        Destroy(gameObject,3f);
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (collidableLayer == (collidableLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage(), col.transform.root.position - other.transform.root.position);
                Destroy(gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * projectileSpeed);
    }

    public int GetDamage() {
        return damage;
    }
}

