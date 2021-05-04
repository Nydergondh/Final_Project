using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeapomUI : MonoBehaviour
{
    private GameObject weapomObj;
    public static WeapomUI INSTANCE;

    private void Awake() {
        if (INSTANCE != null) {
            Destroy(gameObject);
        }
        else {
            INSTANCE = this;
        }
    }

    public void SwitchWeapom(Weapom_SO newWeapom) {

        if (newWeapom.id > 0) {
            Destroy(weapomObj);
            weapomObj = Instantiate(newWeapom.weaponObj, transform.position, transform.rotation, transform);
            return;
        }
        
        Destroy(weapomObj);
    }

}
