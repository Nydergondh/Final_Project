using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBisectAnim : HumanoidBicectAnim {

    public void SetPlayerStrafe(bool value) {
        objAnim.SetBool("IsStrafing", value);
    }

    public void SetCurretnWeapon(int value) {
        objAnim.SetInteger("CurrentWeapon", value);
    }
}
