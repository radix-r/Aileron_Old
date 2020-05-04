using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    public float speed;
    public float initialSpeed=0;
    void Start(){
        // StartCoroutine(MoveRoutine());
        
    }  
    void Update()
    {
        transform.position += transform.forward * (speed+initialSpeed) * Time.deltaTime;
    }

   public void SetInitSpeed(float s){
        this.initialSpeed = s; 
    } 

    /* 
    IEnumerator MoveRoutine(){
        float waitTime = .07f;
        for(;;){
            transform.position += transform.forward * (speed+initialSpeed) * waitTime;
            // update position less than frame rate 
            yield return new WaitForSeconds(waitTime);
        }
    }
    */



}
