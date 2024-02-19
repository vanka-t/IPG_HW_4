using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cosplayer : MonoBehaviour
{
    // Sanity Bar
    [SerializeField]
    Transform displayGroup;
    [SerializeField]
    Transform sanityBar;

    float _sanityValue = 0;


    float SanityValue
    { //if lower than 0, set value to zero, if above, set to value
        get { return _sanityValue;  }
        set { _sanityValue = Mathf.Max(0, value); }
    }
    const float SanityValueTotal = 1000;//default value

    //how fast sanity bar drops
    [SerializeField]
    float sanitySpeed = 20f;

    //check if dead, and stop moving if true
    bool isDying = false;


    Transform garden;

    //Visual related

    [SerializeField]
    Transform mainBody;
    [SerializeField]
    Animator anim;


    Transform mainCamera;

    NavMeshAgent nav;


    // which character is facing to
    string currentDirection = "Right";

    float timer_Direction = 0;
    float timer_DirectionTotal = 1;

    //Random movement timer
    float moveTimer = 0;
    float moveTimerTotal = 0;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        //nav.SetDestination(Vector3.zero);

        mainCamera = Camera.main.transform;
        SanityValue = SanityValueTotal;
        garden = GameObject.Find("GardenSpace").transform;

        ResetMoveTimer();
       // mainBody = GetComponent<Transform>();


    }

    // Update is called once per frame
    void Update()
    {

        if (!isDying)
        {
            if (SanityValue > 0)
            {
                //going insane
                SanityValue -= sanitySpeed * Time.deltaTime;



                // sanity bar UI (changing based on sanity value)
                if(SanityValue < SanityValueTotal / 2)
                {
                    displayGroup.localScale = Vector3.one;
                    sanityBar.localScale = new Vector3(SanityValue / (SanityValueTotal / 2), 1, 1);
                }
                else
                {
                    displayGroup.localScale = Vector3.zero;
                }



                if(moveTimer > moveTimerTotal)
                {
                    ResetMoveTimer(); //reset timer
                    nav.isStopped = false;

                    //if sanity drops to less than 50%
                    if ( SanityValue < SanityValueTotal / 2)
                    {
                        //feel hungry, start run to the garden
                        nav.SetDestination(garden.position);
                        nav.speed = 10;
                    }
                    else
                    {
                        //otherise just wander around
                        var ranPos = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
                        nav.SetDestination(transform.position + ranPos);
                        nav.speed = 3;
                    }
                }
                else
                {
                    moveTimer += Time.deltaTime;

                }

            }
            else
            {
                //sanity bar is at 0, character dies
                displayGroup.gameObject.SetActive(false);
                isDying = true;
                anim.SetTrigger("Dying");
                nav.isStopped = true;


            }
        }

        //animation part
        if(nav.velocity.magnitude <0.1f)
        {
            anim.SetBool("isWalking", false);

        }
        else
        {
            anim.SetBool("isWalking", true);
            anim.SetFloat("Velocity", nav.velocity.magnitude);

        }

        //change direction character is facing over time
        if (timer_Direction > timer_DirectionTotal)
        {
            timer_Direction = 0;
            timer_DirectionTotal = Random.Range(0.2f, 1.23f);
            float currentRelativeDirection = Vector3.Dot(nav.velocity, mainCamera.right);

            if (currentRelativeDirection > 0 && currentDirection == "Right")
            {
                currentDirection = "Left";
                var localS = mainBody.localScale;
                localS.x *= -1;
                mainBody.localScale = localS;

            }
            else if (currentRelativeDirection < 0 && currentDirection == "Left")
            {
                currentDirection = "Right";
                var localS = mainBody.localScale;
                localS.x *= -1;
                mainBody.localScale = localS;

            }


        }
        else
        {
            timer_Direction += Time.deltaTime;
        }

    }

    void ResetMoveTimer()
    {
        moveTimer = 0;
        moveTimerTotal = Random.Range(1.01f, 5.5f); //variety of sanity bar speeds for characters

    }

    //attack
    private void OnTriggerStay(Collider other)
    {
        //only go for power up boost when going crazy
        if(other.CompareTag("PowerUp") && SanityValue< SanityValueTotal/2)
        {
            anim.SetTrigger("Attack");
            ResetMoveTimer();
            nav.isStopped = true;

            //recover health
            SanityValue = SanityValueTotal;


        }

    }

}


