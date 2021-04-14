using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HealthScript : MonoBehaviour
{
    private EnemyAnimator enemyAnime;
    private NavMeshAgent navAgent;
    private EnemyController enemyController;

    public float health = 100f;

    public bool isPlayer, isBoar, isCannibal;

    private bool isDead;

    private EnemyAudio enemyAudio;

    private PlayerStats playerStats;


    // Start is called before the first frame update
    void Awake()
    {
        if(isBoar || isCannibal)
        {
            enemyAnime = GetComponent<EnemyAnimator>();
            enemyController = GetComponent<EnemyController>();
            navAgent = GetComponent<NavMeshAgent>();

            // get enemy audio
            enemyAudio = GetComponentInChildren<EnemyAudio>();
        }

        if (isPlayer)
        {
            playerStats = GetComponent<PlayerStats>();
        }

        
    }

    public void ApplyDamage(float damage)
    {

        //if we die dont execute rest of code
        if (isDead)
            return;
        health -= damage;

        if (isPlayer)
        {
            // show the stats
            playerStats.DisplayHealthStats(health);
        }

        if(isBoar || isCannibal)
        {
            if(enemyController.EnemyState == EnemyState.PATROL)
            {
                enemyController.chaseDistance = 500f;
            }
        }
         if(health <=0)
        {
            PlayerDied();

            isDead = true;
        }
    }
    // end of apply damage

    void PlayerDied()
    {

        if (isCannibal)
        {
            GetComponent<Animator>().enabled = false;
            GetComponent<BoxCollider>().isTrigger = false;
            GetComponent<Rigidbody>().AddTorque(-transform.forward * 10f);

            enemyController.enabled = false;
            navAgent.enabled = false;
            enemyAnime.enabled = false;


            //startCoroutine

            // enemyManager spawn more enemy
            EnemyManager.instance.EnemyDied(true);
        }

        if (isBoar)
        {
            navAgent.velocity = Vector3.zero;
            navAgent.isStopped = true;
            enemyController.enabled = false;

            enemyAnime.Dead();

            StartCoroutine(DeadSound());

            // enemyManager spawn more enemy
            EnemyManager.instance.EnemyDied(false);
        }
        if (isPlayer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

            for(int i =0; i< enemies.Length; i++)
            {
                enemies[i].GetComponent<EnemyController>().enabled = false;
            }

            // call enemy manager to stop spawing enemies
            EnemyManager.instance.StopSpawning();

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<PlayerAttack>().enabled = false;
            GetComponent<WeaponManager>().GetCurrentSelectedWeapon().gameObject.SetActive(false);


        }

        if(tag == Tags.PLAYER_TAG)
        {
            Invoke("RestartGame", 3f);

        }
        else
        {
            Invoke("TurnOffGameObject", 3f);
        }

    }// player died

    void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }

    void TurnOffGameObject()
    {
        gameObject.SetActive(false);
    }

    IEnumerator DeadSound()
    {
        yield return new WaitForSeconds(0.3f);
        enemyAudio.PlayDeadSound();
    }
}// end of class

























