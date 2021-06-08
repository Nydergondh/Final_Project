using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    public LayerMask targetLayer;
    public Enemy_Test enemy;

    private void Start() {
        enemy = GetComponentInParent<Enemy_Test>();
    }

    private void OnTriggerEnter(Collider other) {
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer) && enemy.alive) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage on parent (Enemy/Player) and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage(), enemy.transform.root.position - other.transform.root.position);
                //can make this more optimal by doing it on start and accessing variables on the trigger event
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer) && enemy.alive) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage on parent (Enemy/Player) and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage(), enemy.transform.root.position - other.transform.root.position);
                //can make this more optimal by doing it on start and accessing variables on the trigger event
            }
        }
    }
}
