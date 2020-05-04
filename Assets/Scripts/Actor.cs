using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
This is the base class that all characters, ententes that move, attack, and die, will inherit from 

This class handels targeting, HP,
*/
public class Actor : MonoBehaviour
{
    // needs colider, tags that do damage, hp
    protected DamageTable damageTable;
    public string[] damageTags;
    public int hp;

    // targeting and navigation
    [SerializeField] string[] targetTags;
    private GameObject target;
    public GameObject targetTracker; // where to look for targeting information

    void Start()
    {
        damageTable = GameObject.FindWithTag("DamageTable").GetComponent<DamageTable>();
        if (damageTable == null){
            print("Failed to find damage table");
        }
        targetTracker = new GameObject("targetTracker");
        
        StartCoroutine(UpdateTargeting());
    }

    public virtual void OnCollisionEnter(Collision collision){
        string tag = collision.gameObject.tag;
        // if tag in damage list deal damage
        if(Array.Exists(damageTags,s => s.Equals(tag))){
            int d = damageTable.GetDamage(tag);
            hp -= d;
            //print(d);
            if(hp <= 0){
                Die();
            }
        }
        ExtraCollisionEffects(collision);
    }
    
    public virtual void ExtraCollisionEffects(Collision collision){
        
    }
    public virtual void Die(){
        Destroy(gameObject);
    }

    GameObject PickNearestTarget(){
        // return GameObject.FindGameObjectWithTag("PlayerShip");
        // get all allies and player

        List<GameObject[]> targetTypes = new List<GameObject[]>();

        foreach (string tag in targetTags){
            targetTypes.Add(GameObject.FindGameObjectsWithTag(tag));
        }

        // get count of all potential targets
        int targetCount = 0;
        foreach(GameObject[] targetList in targetTypes){
            targetCount += targetList.Length;
        }

        GameObject[] targets= new GameObject[targetCount];

        int index = 0;
        foreach(GameObject[] targetList in targetTypes){
            targetList.CopyTo(targets,index);
            index += targetList.Length;
        }

        //get closest characters (to referencePos)
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject t in targets)
        {
            Vector3 diff = t.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = t;
                distance = curDistance;
            }
        }

        return closest;
    }

    void SetTargetTrackerPosition(Vector3 position){
        targetTracker.transform.SetPositionAndRotation(position,transform.rotation);
    }

   

    IEnumerator UpdateTargeting(){

        for(;;){

            // set target to nearest enemy
            target = PickNearestTarget();
            //while(target == null)
            if(target != null){
                SetTargetTrackerPosition(target.transform.position) ;
                //print("Target location: "+targetTracker.transform);
                
            }
            
            yield return new WaitForSeconds(.5f);
        }
    }



}
