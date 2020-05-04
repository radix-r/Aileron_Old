using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class HoverTankBehavior : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] float fireCooldown;
    public GameObject gunProjectile;

    private GameObject firePoint;
    // private GameObject turret;
    private GameObject aimPoint;
    private GameObject cannon;
    private GameObject mount;
    [SerializeField] float aimSpeed;
    private GameObject tracker;
    private NavMeshAgent nav;
    private DamageTable damageTable;

    [SerializeField] GameObject[] positions;

    [SerializeField] int hp;
    [SerializeField] GameObject explosion;
    [SerializeField] string[] damageTags;
    public int positionIndex;

    IEnumerator AimRoutine(){
        //print("Aim routine started");
        for(;;){
            if(tracker != null){
                /*
                Vector3 targetDir = tracker.transform.position - transform.position;
                float step = aimSpeed * Time.deltaTime;
                Vector3 newDir = Vector3.RotateTowards(transform.forward,targetDir,step,.1f);
                turret.transform.rotation = Quaternion.LookRotation(newDir);
                */
                mount.transform.LookAt(tracker.transform);
            }
        
               
            
            yield return new WaitForSeconds(.5f);
        }
    }


   void Die(){
       //Destroy(tracker);
       // explode
       GameObject e = Instantiate(explosion,transform.position, transform.rotation);
       Destroy(e,3);
       Destroy(gameObject);
   }



    IEnumerator MovementRoutine(){
        yield return new WaitForSeconds(1);
        for(;;){
            nav.SetDestination(positions[positionIndex].transform.position);
            yield return new WaitForSeconds(10);
        }
    }

    void OnTriggerEnter(Collider other){
        // if tag in damage list deal damage
        if(Array.Exists(damageTags,s => s.Equals(other.tag))){
           int d = damageTable.GetDamage(other.tag);
           hp -= d;
           //print(d);
           if(hp <= 0){
               Die();
           }
        }        
    }

    IEnumerator ShootRoutine(){
        //print("Start shoot routine");
        //GameObject tracker = null;
        //tracker = GetComponent<NPCTargeting>().targetTracker;
        while(tracker == null){
            yield return new WaitForSeconds(1f);
            //tracker = gameObject.GetComponent<NPCTargeting>().targetTracker;
        }

        //print("Target acquired");
        for(;;){
            // turret.transform.LookAt(tracker.transform);
            //print("firing");
            GameObject bullet = Instantiate(gunProjectile, firePoint.transform.position, firePoint.transform.rotation);
            bullet.transform.LookAt(tracker.transform);
            /* from player shoot script
            Mover moveScript = bullet.GetComponent<Mover>();
            float thrust = GameObject.Find("AvatarShip").GetComponent<AircraftControles>().thrust;

            if (moveScript)
            {
                moveScript.SetInitSpeed(thrust);
            }
            */
            yield return new WaitForSeconds(fireCooldown);
        }

    }

     void Start()
    {
        damageTable = GameObject.FindWithTag("DamageTable").GetComponent<DamageTable>();

        nav = GetComponent<NavMeshAgent>();
        
        string name = gameObject.name;
        //print(name);
        firePoint = GameObject.Find(name+"/CannonMount/Turret/Cannon/Cannon003/FirePoint");
        mount = GameObject.Find(name+"/CannonMount");
        cannon = GameObject.Find(name+"/CannonMount/Turret/Cannon");
        aimPoint = GameObject.Find(name+"/CannonMount/Turret/AimPoint");
        StartCoroutine(UpdateTracker());
        StartCoroutine(ShootRoutine());
        StartCoroutine(MovementRoutine());
        StartCoroutine(AimRoutine());
    }

    IEnumerator UpdateTracker(){
        for(;;){
            if(tracker == null){
                tracker = gameObject.GetComponent<NPCTargeting>().targetTracker;
                
            }
            yield return new WaitForSeconds(2);
        }
    }


}
