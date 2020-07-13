using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAnimations : MonoBehaviour
{
    protected Animator objAnim;

    private void Awake() {
        objAnim = GetComponent<Animator>();
    }

    public void SetVelocity(Vector2 vel) {
        objAnim.SetFloat("xSpeed", vel.x);
        objAnim.SetFloat("zSpeed", vel.y);
    }

    public void SetMoving(bool value) {
        objAnim.SetBool("IsMoving", value);
    }

    public void SetAttack(bool value) {
        objAnim.SetBool("IsAttacking", value);
    }

    public void SetJump(bool value) {
        objAnim.SetBool("Jumping", value);
    }

    public void SetHit(bool value, int newHealth) {

        if (value) {
            objAnim.SetInteger("Health", newHealth);
        }

        if (objAnim.GetInteger("Health") > 0) {
            objAnim.SetBool("Hitted", value);
        }

    }

    public bool GetAttack() {
        return objAnim.GetBool("IsAttacking");
    }

    public void SetHit(bool value) {
        objAnim.SetBool("Hitted", value);
    }
}
