using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class BossProjectile : MonoBehaviour, IDamage {

    [SerializeField]
    private LayerMask collidableLayer;
    private PathCreator pathCreator;

    //[SerializeField]
    //private LayerMask collidableObjects;

    public float projectileSpeed = 5f;
    private float distanceTravelled;
    private bool killProjectile = false;

    [HideInInspector] public float timeToDestroy;
    [HideInInspector] public ProjectileSpawner projectileSpawner;

    [SerializeField]
    private int damage = 1;

    private MeshRenderer meshRenderer;
    private Collider col;

    private void Start() {
        print("Spawned");
        pathCreator = GetComponentInParent<PathCreator>();
        meshRenderer = GetComponent<MeshRenderer>();
        col = GetComponentInChildren<Collider>();
    }

    // Update is called once per frame
    void Update() {
        distanceTravelled += projectileSpeed * Time.deltaTime;
        transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);

        //check if projectile spawner stopped shooting
        if (!projectileSpawner.isShooting) {
            if (!killProjectile) {
                killProjectile = true;
            }
        }
        if (Mathf.Abs(distanceTravelled) >= pathCreator.path.length && killProjectile) {
            projectileSpawner.projectiles.Remove(this);
            Destroy(gameObject);
        }

        //if distance travlled is bigger then the curve resset movement
        if (Mathf.Abs(distanceTravelled) >= pathCreator.path.length) {
            distanceTravelled = 0f;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (collidableLayer == (collidableLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage());
                Destroy(gameObject);
            }
            //else {
            //    Destroy(gameObject);
            //}
        }
    }

    public int GetDamage() {
        return damage;
    }


}
