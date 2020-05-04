using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombGun : MonoBehaviour
{
    public GameObject bombProjectile; // Bomb Prefab w/ Collider
    public float fireRate; // Higher is slower
    private bool canShoot;
    //public AudioSource mAudioSrc;
    private GameObject firePoint;

    void Start(){
        string name = gameObject.name;
        firePoint = GameObject.Find(name+"/ship_model/FirePoint");
        canShoot = true;
        StartCoroutine("Reload");
         //mAudioSrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Firing Shots
        // [Spacebar] = Fire2
        if ((Input.GetButton("Fire2") || Input.GetAxis("Fire2")!= 0) && canShoot)
        {
            //nextFire = Time.time + fireRate;
            //mAudioSrc.PlayOneShot(mAudioSrc.clip);
            GameObject bomb = Instantiate(bombProjectile, firePoint.transform.position - transform.up, transform.rotation);
            bombMover moveScript = bomb.GetComponent<bombMover>();
            float thrust = GameObject.Find("AvatarShip").GetComponent<AircraftControles>().thrust;

            if (moveScript)
            {
                // throw bomb forward a bit
                moveScript.SetInitSpeed(thrust);
            }

            canShoot = false;
            StartCoroutine(Reload());
            
        }
    }
     IEnumerator Reload(){
        //Debug.Log("Can Shoot");
        //new ;
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
        // Debug.Log("Can Shoot");
        yield return null;
        
        
    }
}
