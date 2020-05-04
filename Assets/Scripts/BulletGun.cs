using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletGun : MonoBehaviour
{
    public GameObject gunProjectile; // Bullet Prefab w/ Collider
    public float fireRate; // Higher is slower
    // private float nextFire; // The next time the gun is allowed to fire
    private bool canShoot;
    public AudioSource mAudioSrc;
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
        // [Spacebar] = Fire1
        //print(Input.GetAxis("Fire1"));
        if ((Input.GetButton("Fire1") || Input.GetAxis("Fire1") != 0 ) && canShoot)
        {
            //nextFire = Time.time + fireRate;
            mAudioSrc.PlayOneShot(mAudioSrc.clip);
            Vector3 pos = firePoint.transform.position;
            pos.y = 0;
            GameObject bullet = Instantiate(gunProjectile, pos, transform.rotation);
            Mover moveScript = bullet.GetComponent<Mover>();
            float thrust = GameObject.Find("AvatarShip").GetComponent<AircraftControles>().thrust;

            if (moveScript)
            {
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
