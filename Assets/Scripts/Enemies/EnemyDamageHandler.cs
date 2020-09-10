using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHandler : MonoBehaviour
{
    public LayerMask targetLayer;

    private void OnTriggerEnter(Collider other) {
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage on parent (Enemy/Player) and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage());
                print("GotCalled");
                //can make this more optimal by doing it on start and accessing variables on the trigger event
            }
        }
    }
}
