using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageHandler : MonoBehaviour
{
    private Collider col;
    public LayerMask targetLayer;

    private void Start() {
        col = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other) {
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage on parent (Enemy/Player) and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage());
                //can make this more optimal by doing it on start and accessing variables on the trigger event
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        //check if the layer triggered matches with the target layer
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer)) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage (Enemy/Player) on the parent and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(GetComponentInParent<IDamage>().GetDamage());
            }
        }
    }
}
