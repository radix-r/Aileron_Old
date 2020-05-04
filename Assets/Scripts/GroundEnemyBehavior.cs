using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemyBehavior : NPC
{

    //private GameObject gEnemy;
    //public float moveSpeed = 0f;
    private Rigidbody rbod;
    
    private bool hitGroundYet = false;
    private bool canMelee;
    public float attackRate;

    [SerializeField] private float hitRange;
    
    [SerializeField] GameObject explosion;
    Animator animC;

    public override void Die(){
        GameObject e = Instantiate(explosion,gameObject.transform.position,gameObject.transform.rotation);
        Destroy(e,3);
        Destroy(targetTracker);
        Destroy(gameObject);

    }

    void Awake()
    {
        animC = gameObject.GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rbod = GetComponent<Rigidbody>();

        canMelee = false;
        animC.SetBool("Run Forward", true);

        Random.InitState((int)System.DateTime.Now.Ticks);

        StartCoroutine(meleeRoutine());
        StartCoroutine(UpdateMovementLocation());
        StartCoroutine(PursuitRoutine());
    }


    
    public override void ExtraCollisionEffects(Collision collision){
        if (collision.gameObject.tag == "Ground")
        {
            rbod.useGravity=false;
            rbod.velocity = Vector3.zero;
            hitGroundYet = true;
            nav.enabled=true;
        }
    }

    IEnumerator meleePause(){
        yield return new WaitForSeconds(attackRate);
        canMelee = true;
        yield return null;
    }

    IEnumerator hotPursuit(){
        transform.LookAt(targetTracker.transform);
        Vector3 moveLocation = targetTracker.transform.position;
        nav.SetDestination(moveLocation);
        yield return null;
    }

    IEnumerator meleeRoutine(){
        
        while (targetTracker == null)
            yield return new WaitForSeconds(1f);

        for(;;){
            if(canMelee){
                if(targetTracker != null){
                    animC.SetBool("Melee", true);
                    // TODO dmg bad boiz                
                }
                
                canMelee = false;
                StartCoroutine(meleePause());
                
            }
            yield return new WaitForSeconds(.5f);
        }
    }

    IEnumerator PursuitRoutine() {
        while(!hitGroundYet){
            yield return new WaitForSeconds(2);
        }

        for(;;){
            if(targetTracker != null) {
                if (Vector3.Distance(targetTracker.transform.position, transform.position) < hitRange) {
                    canMelee = true;
                    animC.SetBool("Run Forward", false);
                    StopCoroutine(UpdateMovementLocation());
                    StartCoroutine(hotPursuit());
                }
                else {
                    canMelee = false;
                    animC.SetBool("Run Forward", true);
                    animC.SetBool("Melee", false);
                    StopCoroutine(hotPursuit());
                    StartCoroutine(UpdateMovementLocation());
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
 

}
