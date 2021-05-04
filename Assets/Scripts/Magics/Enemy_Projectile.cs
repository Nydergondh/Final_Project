using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour, IDamage
{
    [SerializeField]
    private LayerMask collidableLayer;
    //[SerializeField]
    //private LayerMask collidableObjects;

    [SerializeField]
    private float projectileSpeed = 5f;
    [SerializeField]
    private int damage = 1;
    public ProjectileType pType;
    private MeshRenderer meshRenderer;

    private Collider col;

    private void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponentInChildren<Collider>();
        Destroy(gameObject, 20f);
    }

    // Update is called once per frame
    void Update() {
        transform.position += transform.forward * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (collidableLayer == (collidableLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                if (pType == ProjectileType.Normal) {
                    other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage());
                    Destroy(gameObject);
                }
                else {
                    StopCoroutine(Player_Test.player.playerCombat.InvertControls());
                    StartCoroutine(Player_Test.player.playerCombat.InvertControls());
                    meshRenderer.enabled = false;
                    col.enabled = false;
                    Destroy(gameObject,5f);
                }
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

    public enum ProjectileType {
        Mantis,
        Normal
    }
}
