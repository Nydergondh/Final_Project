using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantKillWall : MonoBehaviour
{
    public LayerMask targetLayer;
    [HideInInspector]
    public bool isWallActive = false;

    private void OnTriggerStay(Collider other) {
        if (targetLayer == (targetLayer | 1 << other.gameObject.layer) && isWallActive) {
            if (other.GetComponent<IDamageable>() != null) {
                //get your damage on parent (Enemy/Player) and apply it on the target
                other.GetComponent<IDamageable>().OnDamage(10000000);
                //can make this more optimal by doing it on start and accessing variables on the trigger event
            }
        }
    }

}
