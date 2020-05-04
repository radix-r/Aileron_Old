using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/*
This script is depreciated, all of its functionality has been moved to aircraft controls
*/

// This is the PlayerAttributes Class it will be filled by JsonUtility
// [System.Serializable]
// public class PlayerAttributes
// {
//     public int hp;
//     public int collisionDamage;
//     public int bumpForce;
// }

public class playerCollision : MonoBehaviour
{
    // Json text file. Made public feed through Unity editor.
    public TextAsset jsonFilename;
    // Player Attribute Class.
    PlayerAttributes playerAttributes;

    public GameObject player;
    [SerializeField] GameObject explosion;
    //private GameObject collider;
    private bool invincible;
    [SerializeField] float invincibilityCooldown;
    public Image hpBar;
    private int maxHP;
    public string[] damageTags;

    void Awake()
    {
        // Gets the JSONParser script. The script only takes the json text file and transforms it to a string
        // to be read by the JsonUtility
        JSONParser jsonScript = gameObject.GetComponent<JSONParser>();
        string attributesJSON = jsonScript.jsonParser(jsonFilename);
        playerAttributes = new PlayerAttributes();
        playerAttributes = JsonUtility.FromJson<PlayerAttributes>(attributesJSON);

    }

    public int GetHP()
    {
        return playerAttributes.hp;
    }

    // void Bump(float bumpFactor)
    // {


    //     Vector3 bumpDirection = player.transform.forward * -1;

    //     Vector3 bump = bumpDirection.normalized * playerAttributes.bumpForce * player.GetComponent<AircraftControles>().GetThrust();
    //     bump.Set(bump.x, 0, bump.z);


    //     player.GetComponent<Rigidbody>().AddForce(bump * bumpFactor);
    // }

    void Die()
    {

        // find GameController game object
        GameObject gameController = GameObject.Find("CityGameController");

        // clone the main camera
        GameObject mainCamera = GameObject.Find("MainCamera");
        GameObject newCam = new GameObject("newCam");
        newCam.AddComponent(typeof(Camera));
        //gameController.
        newCam.transform.SetPositionAndRotation(mainCamera.transform.position, mainCamera.transform.rotation);
        newCam.transform.parent = gameController.transform;
        Destroy(player.gameObject);
        // explosion
        GameObject exp = Instantiate(explosion, transform.position, transform.rotation);
        Destroy(exp, 3);
    }

    IEnumerator InvincibilityCooldown()
    {
        yield return new WaitForSeconds(invincibilityCooldown);
        invincible = false;
    }

    void OnCollisionEnter(Collision collision){
        print(collision.gameObject.tag);
        player.SendMessage("OnCollisionEnter",collision);
    }
    // void OnTriggerEnter(Collider other)
    // {
    //     //Debug.Log(other.tag);
    //     foreach (string tag in damageTags)
    //     {
    //         if (tag.CompareTo(other.tag) == 0)
    //         {
    //             if (!invincible)
    //             {
    //                 playerAttributes.hp -= playerAttributes.collisionDamage;
    //                 invincible = true;
    //                 StartCoroutine(InvincibilityCooldown());
    //             }

    //             if (playerAttributes.hp <= 0)
    //             {
    //                 Die();
    //             }
    //             break;
    //         }
    //     }
    //     if (other.tag == "Ground" || other.tag.Contains("Enemy"))
    //     {
    //         // bump the player away from what they hit
    //         Bump(1);



    //     }

    //     if (other.tag.Contains("Barrier"))
    //     {
    //         Bump(1);

    //     }

    //     float fill = (float)playerAttributes.hp / (float)maxHP;

    //     // update hp bar
    //     hpBar.fillAmount = fill;
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

 
}
