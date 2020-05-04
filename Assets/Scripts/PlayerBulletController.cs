using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletController : MonoBehaviour
{

    public GameObject explosion;
    // Start is called before the first frame update


    void OnCollisionEnter(Collision collision){
        // print(other.name);
        string tag = collision.gameObject.tag;
        //print(tag);
        if(tag != gameObject.tag && !(tag.Contains("Player") || tag == "Ally" || tag == "ActionPlane" || tag == "Bomb") ){
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject); // destroy 
            Destroy(expl, 2); // delete the explosion after 3 seconds
        }
    }

    void OnTriggerEnter(Collider other){
        string tag = other.tag;
        //print("trigger "+tag);
        if(tag != gameObject.tag && !(tag.Contains("Player") || tag == "Ally" || tag == "ActionPlane" || tag == "Bomb") ){
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject); // destroy 
            Destroy(expl, 2); // delete the explosion after 3 seconds
        }
    }
}
