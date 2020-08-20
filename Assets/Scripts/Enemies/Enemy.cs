using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected int health;
    public bool alive;

    public FieldOfView fov;
    public EnemyMovement enemyMovement;

    void Update() {
        fov.FindVisibleTarget();
        enemyMovement.FollowTarget();
    }

}
