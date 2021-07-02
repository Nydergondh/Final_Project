using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillWall : MonoBehaviour
{
    public LayerMask targetLayer;
    [SerializeField]
    private EnemyBoss boss;
    private ParticleSystem fireParticles;

    private void Start() {
        fireParticles = GetComponentInChildren<ParticleSystem>();
    }

    private void OnEnable() {
        boss.finalShowDown += StartFire;
    }

    private void OnTriggerEnter(Collider other) {
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer) && boss.startFight) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage on parent (Enemy/Player) and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(10000000);
                //can make this more optimal by doing it on start and accessing variables on the trigger event
            }
        }
    }

    private void OnDisable() {
        fireParticles.Stop();
        boss.finalShowDown -= StartFire;
    }

    public void StartFire() {
        fireParticles.Play();
    }
}
