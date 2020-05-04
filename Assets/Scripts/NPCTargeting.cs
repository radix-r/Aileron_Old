using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCTargeting : MonoBehaviour
{
    
    
    
    

    // targeting and navigation
    [SerializeField] string[] targetTags;
    private GameObject target;
    public GameObject targetTracker = null; // where to look for targeting information
    

    // Start is called before the first frame update
    void Start()
    {
        
        targetTracker = new GameObject("targetTracker");
        

        StartCoroutine(UpdateTargeting());
        
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

        //target = PickNearestTarget();

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
