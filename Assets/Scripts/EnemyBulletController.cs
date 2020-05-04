using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
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
        // print(other.name);
        if(collision.gameObject.tag != gameObject.tag && !(collision.gameObject.tag.Contains("Enemy") || collision.gameObject.tag == "ActionPlane")){
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            Destroy(gameObject); // destroy 
            Destroy(expl, 2); // delete the explosion after 3 seconds
        }
    }
}
