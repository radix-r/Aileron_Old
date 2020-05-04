using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombMover : MonoBehaviour
{
    public float speed;
    public float initialSpeed;
    void Start(){
        // StartCoroutine(MoveRoutine());
        
    }  
    void Update()
    {
        transform.position += transform.forward * (speed+initialSpeed) * Time.deltaTime;
        
        //transform.position -= transform.up * speed;
    }

   public void SetInitSpeed(float s){
        this.initialSpeed = s; 
    } 

}
