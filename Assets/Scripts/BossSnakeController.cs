using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossSnakeController : NPC
{

    [SerializeField] float attackCooldown;
    [SerializeField] GameObject explosion;
    //private NavMeshAgent nav;
    [SerializeField] GameObject projectile;
    private Rigidbody rbod;
    [SerializeField] GameObject firePoint;
    [SerializeField] GameObject lightObject;
    private Light spotLight;
    [SerializeField] float aimAngle;
    [SerializeField] int numProjectiles;
    [SerializeField] float shotDelay;
    [SerializeField] Animator m_Animator;
    /*
    IEnumerator Attack(){
        for(int i = 0; i< numProjectiles; i++){
            // generate random angle
            print("attacking");
            Vector3 tempRot = firePoint.transform.eulerAngles;

            float randY = Random.Range(0,spotLight.spotAngle/2)-spotLight.spotAngle/2; 
            float randX= Random.Range(0,spotLight.spotAngle/4)-spotLight.spotAngle/4;
            float randZ= Random.Range(0,spotLight.spotAngle/4)-spotLight.spotAngle/4;

            tempRot.y += randY;
            tempRot.x += randX;
            tempRot.z += randZ;

            Instantiate(projectile,firePoint.transform.position, Quaternion.Euler(tempRot));
            yield return new WaitForSeconds(shotDelay); 
        }
    }
    */

    IEnumerator AttackRoutine(){
        for(;;){
            spotLight.enabled = true;
            yield return new WaitForSeconds(3);
            spotLight.color=new Color(0,1,0);
            // attack
            m_Animator.SetBool("sssnake_attack", true);
            for(int i = 0; i < numProjectiles; i++){
                // generate random angle
                //print("attacking");
                Vector3 tempRot = firePoint.transform.eulerAngles;

                float randY = Random.Range(0,spotLight.spotAngle/4)-spotLight.spotAngle/8; 
                float randX= Random.Range(0,spotLight.spotAngle/8)-spotLight.spotAngle/16;
                //float randZ= Random.Range(0,spotLight.spotAngle/4)-spotLight.spotAngle/2;

                tempRot.y += randY;
                tempRot.x += randX;
                //tempRot.z += randZ;

                Instantiate(projectile,firePoint.transform.position, Quaternion.Euler(tempRot));
                yield return new WaitForSeconds(shotDelay);
            }
            yield return new WaitForSeconds(1);
            spotLight.enabled = false;
            m_Animator.SetBool("sssnake_attack", false);
            spotLight.color=new Color(1,1,1);
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    /* https://answers.unity.com/questions/141775/limit-local-rotation.html */
    /* needs work. limits to positive z direction*/
    float ClampAngle(float angle, float min, float max){
        if( angle < 90 || angle > 270){
            if (angle>180) angle -= 360;  // convert all angles to -180..+180
            if (max>180) max -= 360;
            if (min>180) min -= 360;
        }
        
        angle = Mathf.Clamp(angle, min, max);
        if (angle<0) angle += 360;  // if angle negative, convert to 0..360
        return angle;

    }
    

    public override void Die(){
        Destroy(targetTracker);
        Vector3 expPos = new Vector3(gameObject.transform.position.x,gameObject.transform.position.y+20,gameObject.transform.position.z);
        GameObject e = Instantiate(explosion,expPos,gameObject.transform.rotation);
        Destroy(e,3);
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Awake()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        rbod = GetComponent<Rigidbody>();
        rbod.useGravity=true;
        nav = GetComponent<NavMeshAgent>();
        //targetTracker = GetComponent<NPCTargeting>().targettargetTracker;
        //firePoint = GameObject.Find(name+"/FirePoint");
        //gameObject.
        spotLight = lightObject.GetComponent<Light>();//GameObject.Find(name+"/FirePoint/SpotLight").GetComponent<Light>();
        StartCoroutine(AttackRoutine());
        StartCoroutine(UpdateMovementLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTracker != null && firePoint != null){
            /* Eventual want small gimbaling of target cone*/
            firePoint.transform.LookAt(targetTracker.transform);

            //print("before"+firePoint.transform.localEulerAngles.y);
            
            float x = firePoint.transform.localEulerAngles.x;
            float y = ClampAngle(firePoint.transform.localEulerAngles.y ,-aimAngle,aimAngle);
            float z = firePoint.transform.localEulerAngles.z;

            
            //print("a"+y);
            // firePoint.transform.eulerAngles = new Vector3(ClampAngle(x,-aimAngle,aimAngle),ClampAngle(y,-aimAngle,aimAngle),ClampAngle(z,-aimAngle,aimAngle));
            firePoint.transform.localEulerAngles = new Vector3(x,y,z);
        }
        
    }

    public override void ExtraCollisionEffects(Collision collision){
        if(collision.gameObject.tag.CompareTo("Ground") == 0){
            rbod.useGravity=false;
            rbod.velocity = Vector3.zero;
            nav.enabled=true;
            
        }
        //print(collision.gameObject.tag);
    }
    


}
