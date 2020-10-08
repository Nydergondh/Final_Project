using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : HumanoidAnimations {

    public void SetStrafe(bool value) {
        objAnim.SetBool("IsStrafing", value);
    }

    public void SetCurretnWeapon(int value) {
        objAnim.SetInteger("CurrentWeapon", value);
    }

}
