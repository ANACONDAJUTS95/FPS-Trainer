using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private bool chasing;
    private float distanceToChase = 10f, distanceToLose = 15f, distanceToStop = 2f;

    private Vector3 targetPoint, startPoint;

    public NavMeshAgent agent;

    public float keepChasingTime = 5f;
    private float chaseCounter;

    public GameObject bullet;
    public Transform firePoint;

    public float fireRate, waitBetweenShots = 2f, timeToShoot = 1f;
    private float fireCount, shotWaitCounter, shootTimeCounter;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;

        shootTimeCounter = timeToShoot;
        shotWaitCounter = waitBetweenShots;
    }

    // Update is called once per frame
    void Update()
    {
        targetPoint = PlayerController.instance.transform.position;
        targetPoint.y = transform.position.y;

        //function for Player is not active, return to initial position
        if (!PlayerController.instance.gameObject.activeInHierarchy)
        {
            agent.destination = startPoint;
            anim.SetBool("isMoving", true);

            // Exit the Update function to avoid executing the chasing logic
            return; 
        }

        if (!chasing)
        {
            if (Vector3.Distance(transform.position, targetPoint) < distanceToChase)
            {
                shootTimeCounter = timeToShoot;
                shotWaitCounter = waitBetweenShots;

                chasing = true;
            }

            if (chaseCounter > 0)
            {
                chaseCounter -= Time.deltaTime;

                if (chaseCounter <= 0)
                {
                    agent.destination = startPoint;
                }
            }

            if (agent.remainingDistance < .25f)
            {
                anim.SetBool("isMoving", false);
            }

            else
            {
                anim.SetBool("isMoving", true);
            }
        }

        else
        {
            //transform.LookAt(targetPoint);

            //theRB.velocity = transform.forward * moveSpeed;

            if (Vector3.Distance(transform.position, targetPoint) > distanceToStop)
            {
                agent.destination = targetPoint;
            }

            else
            {
                agent.destination = transform.position;
            }
            

            if (Vector3.Distance(transform.position, targetPoint) > distanceToLose)
            {
                chasing = false;

                chaseCounter = keepChasingTime;
            }

            if (shotWaitCounter > 0)
            {
                shotWaitCounter -= Time.deltaTime;

                if(shotWaitCounter <= 0)
                {
                    shootTimeCounter = timeToShoot;
                }

                anim.SetBool("isMoving", true);
            }

            else
            {
                if (PlayerController.instance.gameObject.activeInHierarchy)
                {

                    shootTimeCounter -= Time.deltaTime;

                    if (shootTimeCounter > 0)
                    {
                        fireCount -= Time.deltaTime;

                        if (fireCount <= 0)
                        {
                            fireCount = fireRate;

                            firePoint.LookAt(PlayerController.instance.transform.position + new Vector3(0f, 1.5f, 0f));

                            //check the angle to the player
                            Vector3 targetDir = PlayerController.instance.transform.position - transform.position;
                            float angle = Vector3.SignedAngle(targetDir, transform.forward, Vector3.up);

                            if (Mathf.Abs(angle) < 30f)
                            {
                                Instantiate(bullet, firePoint.position, firePoint.rotation);

                                anim.SetTrigger("fireShot");
                            }

                            else
                            {
                                shotWaitCounter = waitBetweenShots;
                            }
                        }

                        agent.destination = transform.position;
                    }

                    else
                    {
                        shotWaitCounter = waitBetweenShots;
                    }
                }

                anim.SetBool("isMoving", false);
            }
        }
    }
}
