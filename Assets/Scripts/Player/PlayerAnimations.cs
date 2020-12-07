using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : HumanoidAnimations {

    public void SetCurretnWeapon(int value) {
        objAnim.SetInteger("CurrentWeapon", value);
    }

}
