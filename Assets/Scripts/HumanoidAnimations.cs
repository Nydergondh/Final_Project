using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanoidAnimations : MonoBehaviour
{
    protected Animator objAnim;
    //TODO maybe refactor bellow (didn't know other way to do this at the time being)
    protected bool isPlayer;
    protected Player_Test player;
    protected Enemy_Test enemy;
    protected EnemyBoss bossEnemy = null;

    protected Player_Combat_Test playerCombat;
    protected EnemyCombat_Test enemyCombat;

    private Collider[] col;

    private void Awake() {
        objAnim = GetComponent<Animator>();
    }

    void Start() {

        if (GetComponent<Enemy_Test>() != null) {
            enemy = GetComponent<Enemy_Test>();
            enemyCombat = GetComponent<EnemyCombat_Test>();
            isPlayer = false;
        }
        else if (GetComponent<EnemyBoss>() != null) {
            bossEnemy = GetComponent<EnemyBoss>();
            isPlayer = false;
        }
        else {
            isPlayer = true;
            player = GetComponent<Player_Test>();
            playerCombat = GetComponent<Player_Combat_Test>();
        }

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

    public bool GetMoving() {
        return objAnim.GetBool("IsMoving");
    }

    public void SetHit(bool value) {
        objAnim.SetBool("Hitted", value);
    }

    public void SetAnimSpeed(float value) {
        objAnim.speed = value;
    }

    public void UnsetAnimAttack() {
        objAnim.SetBool("IsAttacking", false);
    }

    IEnumerator WaitAttackAgain() {
        yield return new WaitForSeconds(0.25f); //TODO change to a variable later
        if (isPlayer) {
            playerCombat.UnsetAttack();
        }
        else if (bossEnemy) {
            bossEnemy.UnsetAttack();
        }
        else {
            enemyCombat.UnsetAttack();
        }
    }

    public void SetStrafe(bool strafe) {
        objAnim.SetBool("IsStrafing", strafe);
    }

    public void SetAlive(bool value) {
        objAnim.enabled = value;
    }

    public void SetAnimTotalSpeed(float value) {
        objAnim.speed = value;
    }

    public void SetCurretnWeapon(int value) {
        objAnim.SetInteger("CurrentWeapon", value);
    }

    //used on animation event Cast Attack
    public void UseMagic() {
        MagicHandler magicHandler = GetComponentInParent<MagicHandler>();

        if (magicHandler.currentMagic.magicType == MagicHandler.MagicType.Projectile) {
            Instantiate(magicHandler.currentMagic.GetMagicPrefab(), magicHandler.magicSpawnPoint.position, magicHandler.magicSpawnPoint.rotation);
        }

        else {
            switch (magicHandler.currentMagic.GetMagicId()) {
                //invisibility
                case 3:
                    StartCoroutine(magicHandler.BecomeInvisible());
                    break;
                case 4:
                    StartCoroutine(magicHandler.WalkTroughWall());
                    break;
                case 5:
                    StartCoroutine(magicHandler.TimeStop());
                    break;
            }
        }
    }
}
