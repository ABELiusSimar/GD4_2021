using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{

    [SerializeField] protected Vector3 m_from = new Vector3(0.0F, 45.0F, 0.0F);
    [SerializeField] protected Vector3 m_to = new Vector3(0.0F, -45.0F, 0.0F);
    [SerializeField] protected float m_frequency = 1.0F;


    [SerializeField]
    private Transform player;
    public float wanderRadius = 25f;
    public float wanderTimer = 0.02f;


    [SerializeField] private bool startWander = true;
    [SerializeField] private bool lockedOnPlayer;
    [SerializeField] private bool lockedLastLocation;
    [SerializeField] private bool lookingAround;
    private Vector3 lastLocation;



    NavMeshAgent agent;
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        timer = 0;
        lockedOnPlayer = false;



    }

    // Update is called once per frame
    void Update()
    {
        if (!lockedOnPlayer && !lockedLastLocation && startWander && !lookingAround)
        {
            timer += Time.deltaTime;

            if (timer >= wanderTimer)
            {
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, 29);
                agent.SetDestination(newPos);
                Debug.DrawLine(transform.position, newPos, Color.black, 10);
                timer = 0;
            }

        }
        if (lookingAround)
        {
            float dist = agent.remainingDistance;
           
            if (dist != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance < 1)

            {
                LookAround();

            }

        }

        if (agent.remainingDistance < 5f)
        {
            agent.speed = 6;
        }
        else
        {
            agent.speed = 6;
        }




        if (CheckForPlayer() && !lookingAround)
        {
            lookingAround = false;
            float maxRange = 25;
            RaycastHit hit;

            if (Vector3.Distance(transform.position, player.position) < maxRange)
            {

                if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, maxRange))
                {
                    if (hit.transform == player)
                    {
                        Debug.DrawRay(transform.position, (player.position - transform.position), Color.green, 0, true);
                        FaceTarget(player.position);
                        agent.SetDestination(player.position);
                        lockedLastLocation = false;
                        lockedOnPlayer = true;
                    }
                    else if (lockedOnPlayer)
                    {
                        if (!lockedLastLocation)
                        {
                            Vector3 lastPos = transform.position;
                            Vector3 lastSeenPlayer = player.position;
                            Debug.DrawRay(lastPos, lastSeenPlayer, Color.red, 10, true);
                            agent.SetDestination(lastSeenPlayer);
                            Debug.DrawLine(transform.position, lastSeenPlayer, Color.black, 10);
                            lockedOnPlayer = false;
                            lookingAround = true;


                        }




                    }









                }
            }
        }
        else
        {
            lockedOnPlayer = false;
        }
    }
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void FaceTarget(Vector3 destination)
    {
        Vector3 lookPos = destination - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 100);
    }
    private void BackToWoner()
    {
        lockedLastLocation = false;
        lockedOnPlayer = false;
    }

    private bool CheckForPlayer()
    {
        Vector3 targetDir = player.position - transform.position;
        float angleToPlayer = (Vector3.Angle(targetDir, transform.forward));
        if (angleToPlayer >= -60 && angleToPlayer <= 60)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void WonderFast()
    {
        startWander = true; ;
        lookingAround = false;
        BackToWoner();
        lockedOnPlayer = false;
    }
    private void LookAround()
    {
        startWander = false;
        //supposed to rotate but does not work for some reason
        Quaternion from = Quaternion.Euler(m_from);
        Quaternion to = Quaternion.Euler(m_to);

        float lerp = 0.5F * (1.0F + Mathf.Sin(Mathf.PI * Time.realtimeSinceStartup * this.m_frequency));
        this.transform.localRotation = Quaternion.Lerp(from, to, lerp);

        Invoke("WonderFast", 1f);

    }
}
