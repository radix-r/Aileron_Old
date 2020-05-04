using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Actor
{
    [SerializeField] float targetRange; // distance from target we want to move
    // navigation
    protected NavMeshAgent nav;
    private Vector3 moveLocation;
     Vector3 PickLocation(){
        Vector3 temp = Random.insideUnitSphere*targetRange;
        // dont get too close to target
        temp = temp+temp.normalized*5;
        return new Vector3(temp.x,0,temp.z);
    }

    protected IEnumerator UpdateMovementLocation(){
        yield return new WaitForSeconds(1);
        while(!(nav.enabled && nav.isOnNavMesh)){
            // wait until we hit the ground
            yield return new WaitForSeconds(1);
        }
        
        //print("Tracking Active");
        
        moveLocation = PickLocation() + targetTracker.transform.position;
        nav.SetDestination(moveLocation);
        for(;;){
            
            // if within X unit of moveLocation pick new destination 
            //print("update target: " + Vector3.Distance(transform.position,moveLocation));
            if(Vector3.Distance(transform.position,moveLocation) <= (targetRange+20)){
                
                moveLocation = PickLocation() + targetTracker.transform.position;
                // Debug.Log("New Location: "+moveLocation);
                nav.SetDestination(moveLocation);
                //transform.SetPositionAndRotation(transform.position,rot);
            }
            // Debug.Log("Dist to new location: "+ (Vector3.Distance(transform.position,moveLocation)-targetRange));
            
            yield return new WaitForSeconds(1);
        }
    }
}
