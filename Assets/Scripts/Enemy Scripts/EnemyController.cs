using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    PATROL,
    CHASE,
    ATTACK
}

public class EnemyController : MonoBehaviour
{
    private EnemyAnimator enemyAnime;
    private NavMeshAgent navAgent;

    private EnemyState enemyState;

    public float walkSpeed = 0.5f;
    public float runSpeed = 4f;

    public float chaseDistance = 7f;
    private float currentChaseDistance;
    public float attackDistance = 1.8f;
    public float chaseAfterAttackDistance = 2f;

    public float patrolRadiusMin = 20f, patrolRadiusMax = 60f;
    public float patrolForThisTime = 15f;
    private float patrolTimer;

    public float waitBeforeAttack = 2f;
    private float attackTimer;

    private Transform target;

    public GameObject attackPoint;

    private EnemyAudio enemyAudio;

    private void Awake()
    {
        enemyAnime = GetComponent<EnemyAnimator>();
        navAgent = GetComponent<NavMeshAgent>();

        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

        enemyAudio = GetComponentInChildren<EnemyAudio>();
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyState = EnemyState.PATROL;

        patrolTimer = patrolForThisTime;

        // attack right away
        attackTimer = waitBeforeAttack;

        // memorize the value of chase distance
        currentChaseDistance = chaseDistance;

    } // end of start

    // Update is called once per frame
    void Update()
    {
        if(enemyState == EnemyState.PATROL)
        {
            Patrol();
        }
        if(enemyState == EnemyState.CHASE)
        {
            Chase();
        }

        if (enemyState == EnemyState.ATTACK)
        {
            Attack();
        }
    } // end of update

    void Patrol()
    {
        navAgent.isStopped = false;
        navAgent.speed = walkSpeed;

        patrolTimer += Time.deltaTime;

        if (patrolTimer > patrolForThisTime)
        {
            SetNewRandomDestination();

            patrolTimer = 0f;
        }
        if(navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnime.Walk(true);
        }
        else
        {
            enemyAnime.Walk(false);
        }
        // test distance between the player and the enemy
        if(Vector3.Distance(transform.position, target.position) <= chaseDistance)
        {
            enemyAnime.Walk(false);
            enemyState = EnemyState.CHASE;


            // play spotted audio
            enemyAudio.PlayScreemSound();
        }



    }
    // end of Patrol

    void Chase()
    {
        // eneble the agent to move again

        navAgent.isStopped = false;
        navAgent.speed = runSpeed;

        // set the player's position as the destination
        // because we need to make it chase the player
        navAgent.SetDestination(target.position);
        if (navAgent.velocity.sqrMagnitude > 0)
        {
            enemyAnime.Run(true);
        }
        else
        {
            enemyAnime.Run(false);
        }
        // if the distance between enemy and player is less than attack distance
        if (Vector3.Distance(transform.position, target.position) <= attackDistance)
        {
            // stop the animations

            enemyAnime.Run(false);
            enemyAnime.Walk(false);
            enemyState = EnemyState.ATTACK;

            // reset the chase distance
            if(chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }
        }
        else if(Vector3.Distance(transform.position, target.position) > chaseDistance)
        {
            // player run away again

            //stop running
            enemyAnime.Run(false);

            enemyState = EnemyState.PATROL;

            // rest the patrol timer to calculate new patrol destination

            patrolTimer = patrolForThisTime;

            // reset the chase distance
            if (chaseDistance != currentChaseDistance)
            {
                chaseDistance = currentChaseDistance;
            }

        }// else

    }
    // end of  Chase

    void Attack()
    {
        navAgent.velocity = Vector3.zero;
        navAgent.isStopped = true;

        attackTimer += Time.deltaTime;

        if(attackTimer > waitBeforeAttack)
        {
            enemyAnime.Attack();

            attackTimer = 0f;

            // play attack sound
            enemyAudio.PlayAttackSound();

        }

        if(Vector3.Distance(transform.position, target.position)> attackDistance + chaseAfterAttackDistance)
        {
            enemyState = EnemyState.CHASE;
        }

    }
    // end of Attack

    void SetNewRandomDestination()
    {
        float randRadius = Random.Range(patrolRadiusMin, patrolRadiusMax);

        Vector3 randDir = Random.insideUnitSphere * randRadius;
        randDir += transform.position;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDir, out navHit, randRadius, -1);

        navAgent.SetDestination(navHit.position);


    }
    // end of set new random destination

    void TurnOnAttackPoint()
    {
        attackPoint.SetActive(true);
    }

    void TurnOffAttackPoint()
    {
        if (attackPoint.activeInHierarchy)
        {
            attackPoint.SetActive(false);
        }
    }

    public EnemyState EnemyState
    {
        get; set;
    }
}// end of class












