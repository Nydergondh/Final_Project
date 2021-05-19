using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ProjectileSpawner : MonoBehaviour
{
    public bool bossProtection;

    [SerializeField]
    private EnemyBoss enemyBoss;

    [Space]
    public GameObject projectilePrefab;
    [Space]

    public Transform[] spawnPoint;
    [HideInInspector] public List<BossProjectile> projectiles;
    [HideInInspector] public bool isShooting = false;

    [Space]
    public float spawnTime = 0.5f;
    public int maxNumofProj = 4;
    [HideInInspector] public int maxNumOfProjModifier = 0;
    [HideInInspector] public float shootingTime = 10f;

    [HideInInspector] public float collDownTimer = 0f;
    //private float colldown = 0f; //used to get the random between min and max colldown when needed
    public float minCoolDown = 0.5f;
    public float maxCoolDown = 1f;
    [HideInInspector] public float collDownModifier = 1f;

    //variables used in boss to change difficulty and control spawns
    [HideInInspector] public float projSpeedModifier = 1f;

    private PathCreator pathCreator;
    private Coroutine shootCoroutine = null;

    private void Start() {
        pathCreator = GetComponent<PathCreator>();
        projectiles = new List<BossProjectile>();
        enemyBoss.finalShowDown += StartSpawning;
        enemyBoss.endFight += StopSpawning;
    }

    public void StartSpawning() {
        isShooting = true;
        shootCoroutine = StartCoroutine(StartSpawnBehaviour());
    }

    public void StopSpawning() {
        isShooting = false;
        print("Stop");
        StopCoroutine(shootCoroutine);
    }

    public IEnumerator StartSpawnBehaviour() {
        if (!bossProtection) {
            yield return new WaitForSeconds(Random.Range(minCoolDown, maxCoolDown));//waits a random time in the beggining of the boss battle

            while (true) {
                //if (isShooting) {
                    GameObject projectile;
                    BossProjectile bossProj;

                    int i = 0;
                    int spawnIndex = Random.Range(0, 2); //select one of the spawn point to make the bullet travel
                    isShooting = true;
                    print(maxNumofProj + maxNumOfProjModifier);
                    //spawns projectiles until all of then are spawned
                    while (i < maxNumofProj + maxNumOfProjModifier) {

                        projectile = Instantiate(projectilePrefab, spawnPoint[spawnIndex].position, Quaternion.identity, transform);
                        bossProj = projectile.GetComponent<BossProjectile>();

                        bossProj.projectileSpawner = this;
                        bossProj.projectileSpeed *= projSpeedModifier;

                        projectiles.Add(bossProj);

                        if (spawnIndex == 1)
                            bossProj.projectileSpeed *= -1;

                        i++;
                        yield return new WaitForSeconds(spawnTime);

                    }
            
                    yield return new WaitForSeconds(shootingTime);
                    isShooting = false;

                    //while (projectiles.Count > 0) { //wait for all projectiles to be destroyed
                    //    yield return null;
                    //}
                    //wait for colldown and reset the whole process
                    collDownTimer = Random.Range(minCoolDown * collDownModifier, maxCoolDown * collDownModifier);
                    print("EndShoot");
                    yield return new WaitForSeconds(collDownTimer);
                //}
                //yield return null;
            }
        }
        else {
            while (true) {
                if (enemyBoss.activateProtection) {
                    //if (isShooting) {
                    GameObject projectile;
                    BossProjectile bossProj;

                    int i = 0;
                    int spawnIndex = Random.Range(0, 2); //select one of the spawn point to make the bullet travel
                    isShooting = true;
                    print(maxNumofProj + maxNumOfProjModifier);
                    //spawns projectiles until all of then are spawned
                    while (i < maxNumofProj + maxNumOfProjModifier) {

                        projectile = Instantiate(projectilePrefab, spawnPoint[spawnIndex].position, Quaternion.identity, transform);
                        bossProj = projectile.GetComponent<BossProjectile>();

                        bossProj.projectileSpawner = this;
                        bossProj.projectileSpeed *= projSpeedModifier;

                        projectiles.Add(bossProj);

                        if (spawnIndex == 1)
                            bossProj.projectileSpeed *= -1;

                        i++;
                        yield return new WaitForSeconds(spawnTime);

                    }

                    yield return new WaitForSeconds(enemyBoss.protectionDuration);
                    isShooting = false;

                    //while (projectiles.Count > 0) { //wait for all projectiles to be destroyed
                    //    yield return null;
                    //}
                    //wait for colldown and reset the whole process
                    collDownTimer = Random.Range(minCoolDown * collDownModifier, maxCoolDown * collDownModifier);
                    print("EndShoot");
                    yield return new WaitForSeconds(collDownTimer);
                    //}
                    //yield return null;
                }
                yield return null;
            }
        }
    }

    private void OnDisable() {
        enemyBoss.finalShowDown -= StartSpawning;
        enemyBoss.endFight -= StopSpawning;
    }
    
}
