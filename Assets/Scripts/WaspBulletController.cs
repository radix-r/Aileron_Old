using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaspBulletController : MonoBehaviour
{

    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.tag != "FlyingEnemy" && collision.gameObject.tag != "ActionPlane" ){
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject); // destroy the bullet
            Destroy(expl, 2); // delete the explosion after 3 seconds
        }
    }

     void OnTriggerEnter(Collider other){
        string tag = other.tag;
        //print("trigger "+tag);
        if(tag != gameObject.tag && !(tag.Contains("Enemy") || tag == "ActionPlane" || tag == "ActionPlane") ){
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject); // destroy 
            Destroy(expl, 2); // delete the explosion after 3 seconds
        }
    }
}
