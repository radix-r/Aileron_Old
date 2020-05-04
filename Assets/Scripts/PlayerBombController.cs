using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBombController : MonoBehaviour
{

    public GameObject explosion;
    public Collider d_Collider;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        // print(other.name);
        if(other.tag != gameObject.tag && !(other.tag.Contains("Player") || other.tag == "Ally" || other.tag == "ActionPlane")){
            GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
            expl.AddComponent<SphereCollider>();
            SphereCollider dmgShp = expl.GetComponent<SphereCollider>();
            dmgShp.radius = 5;
            expl.tag = "Bomb";
            //d_Collider.enabled = true;

            Destroy(gameObject); // destroy 
            Destroy(expl, 2); // delete the explosion after 3 seconds
        }
    }
}
