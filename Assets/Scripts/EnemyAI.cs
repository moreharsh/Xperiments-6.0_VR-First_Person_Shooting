using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    
    // Variables for Patrolling
    public GameObject[] waypoints;
    private int waypointInt = 0;
    public float patrolSpeed = 1f;

    // Variables for Chasing
    public float chaseSpeed = 3f;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //Animator
    Animator m_Animator;

    //Enemy Dead
    public bool enemyDead;

    //States
    [Header("State Variables")]
    public float sightRange, sightattackRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Muzzle Particles Shooting
    [Header("Muzzle Flash")]
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Transform barrelLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 0.02f;


    //Sound
    [Header("Audio Source")]
	//Audio source used for shoot sound
	public AudioSource shootAudioSource;

    [System.Serializable]
	public class soundClips
	{
		public AudioClip shootSound;
	}
    public soundClips SoundClips;

    private void Awake()
    {
        enemyDead = false;
        m_Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        shootAudioSource.clip = SoundClips.shootSound;

        if (barrelLocation == null)
            barrelLocation = transform;
    }

    private void Update()
    {
        if (!enemyDead)
        {
            //Check for sight and attack range
            playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
            playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

            if (playerInAttackRange && transform.GetComponent<LineOfSight_Rotation>().isInFov)
            { 
                AttackPlayer();
            }
            else if (playerInAttackRange && !transform.GetComponent<LineOfSight_Rotation>().isInFov)
            {
                Patrolling();
            }

            else if (playerInSightRange && !playerInAttackRange && transform.GetComponent<LineOfSight_Rotation>().isInFov)
            {
                ChasePlayer();
            }
            else if (playerInSightRange && !playerInAttackRange && !transform.GetComponent<LineOfSight_Rotation>().isInFov)
            {
                Patrolling();
            }
            
            else if (!playerInSightRange && !playerInAttackRange)
            {
                Patrolling();
            }
            else
            {
                StartCoroutine(waiter(10));
            }
        }
    }

    IEnumerator waiter(int waitTime)
    {
        agent.SetDestination(transform.position);
        m_Animator.SetBool("Shoot", false);
        m_Animator.SetFloat("Forward", 0f ,0.1f, Time.deltaTime);

        //Wait for 4 seconds
        yield return new WaitForSecondsRealtime(waitTime);

        Patrolling();
    }


    public void getEnemyDead()
    {
        enemyDead = true;

        m_Animator.SetBool("Shoot", false);
        m_Animator.SetFloat("Forward", 0f ,0.1f, Time.deltaTime);
    }

    private void Patrolling()
    {
        agent.speed = patrolSpeed;

        m_Animator.SetBool("Shoot", false);
        m_Animator.SetFloat("Forward", 0.5f ,0.1f, Time.deltaTime);

        if (Vector3.Distance(this.transform.position, waypoints[waypointInt].transform.position) >= 2)
        {
            agent.SetDestination(waypoints[waypointInt].transform.position);
        }
        else if (Vector3.Distance(this.transform.position, waypoints[waypointInt].transform.position) < 2)
        {
            waypointInt = Random.Range(0, waypoints.Length);
            /*
            waypointInt += 1;
            if (waypointInt > waypoints.Length)
            {
                waypointInt = 0;
            }*/
        }
    }

    private void ChasePlayer()
    {
        float playerInSightAttackRange = Vector3.Distance(player.transform.position, transform.position);
        
        agent.speed = chaseSpeed;

        m_Animator.SetBool("Shoot", false);
        m_Animator.SetFloat("Forward", 1f ,0.1f, Time.deltaTime);
        
        agent.SetDestination(player.position);
        this.transform.LookAt(player);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.speed = 0f;
        agent.SetDestination(transform.position);
        this.transform.LookAt(player);
        
        m_Animator.SetBool("Shoot", true);

        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        if (!alreadyAttacked)
        {
            //Attack code here
            shootAudioSource.clip = SoundClips.shootSound;
            shootAudioSource.Play ();
            
            GameObject playerObj = GameObject.Find("PlayerObj");
            
            PlayerHealth playerhealth = playerObj.GetComponent<PlayerHealth>();
            playerhealth.lowerPlayerHealth(10);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
