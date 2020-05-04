using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
This script will controle the behavior of the flying enemy. Movement, shooting, death, 

 */

public class FlyingEnemyBehavior : NPC
{

    //private NavMeshAgent nav;
    //private Transform player;
    //public int hp=10;
    public GameObject explosion;
    // Max distance from target move location can be
    //public float targetRange;
    //private GameObject targetTracker;
    //private Vector3 moveLocation;
    // private float moveStarted;
    private bool canShoot;
    public GameObject projectile;
    public float fireRate;
    private Rigidbody rbod;
    private bool falling;

    void Awake(){
canShoot = false;
        Random.InitState((int)System.DateTime.Now.Ticks);
        rbod = GetComponent<Rigidbody>();
        rbod.useGravity=true;
        nav = GetComponent<NavMeshAgent>();
        nav.enabled = false;
        falling = true;
        
        StartCoroutine(ShootRoutine());
        StartCoroutine(UpdateMovementLocation());
        StartCoroutine(Activate());
    }
   
    void Update()
    {
        // targetTracker = GetComponent<NPCTargeting>().targetTracker;
        if(targetTracker != null){
             transform.LookAt(targetTracker.transform);
            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        }
       
        if(transform.position.y < 0){
            transform.position=new Vector3(transform.position.x,0,transform.position.z);
            falling = false;
        }
        
        
    }

    IEnumerator Activate(){
        while(falling){
            yield return new WaitForSeconds(1);
        }
        rbod.useGravity=false;
        canShoot = true;
        rbod.velocity = Vector3.zero;
        nav.enabled = true;
    }


    public override void  Die(){
        GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
        Destroy(targetTracker);
        Destroy(gameObject);
        Destroy(expl,3);
    }


 
    IEnumerator Reload(){
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        yield return null;
    }





    IEnumerator ShootRoutine(){
        
        while(targetTracker == null){
            yield return new WaitForSeconds(1f);
            //targetTracker = GetComponent<NPCTargeting>().targetTracker;
        }

        for(;;){
            if(canShoot){
                //InRange scripts
                //Shot scripts
                // instantiate new projectile in target's direction
                Vector3 bulletPos = transform.position+ (transform.forward * 2);
                bulletPos.y=0;
                GameObject bullet = Instantiate(projectile, bulletPos, transform.rotation);
                // lead shots eventualy
                if(targetTracker != null){
                    bullet.transform.LookAt(targetTracker.transform);
                }
                
                canShoot = false;
                StartCoroutine(Reload());
                
            }
            yield return new WaitForSeconds(.5f);
        }
    }
}
