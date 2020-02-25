using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftControles : MonoBehaviour
{
    public float thrust = 20.0f;
    [SerializeField] float turnSpeed = 0.0f;
    [SerializeField] float rollSpeed = 1.0f;
    float roll = 0.0f;
    [SerializeField] float rollCap = 25.0f;
    //[SerializeField] float restoreScale = 2.0f;
    [SerializeField] float thrustMax = 55;
    [SerializeField] float thrustMin = 15;
    const float thrustScale = 10;

    [SerializeField] float bankMax = 1.25f;

    [SerializeField] float cameraRotateFactor = 0.25f;

    public GameObject gunProjectile; // Bullet Prefab w/ Collider
    public float fireRate; // Higher is slower
    private float nextFire; // The next time the gun is allowed to fire

    private GameObject craft;


    // Start is called before the first frame update
    void Start()
    {
        //craft = 
    }

    // Update is called once per frame
    void Update()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");
        float thrustRatio = thrust/thrustMax; // % of max thrust

        
        // calculate the roll of player ship
        roll = Mathf.MoveTowards(roll, rollCap*inputHorizontal*thrustRatio, Time.deltaTime * rollSpeed * thrustScale * thrustRatio);
        
        // roll children
        foreach (Transform child in transform){
            if (child.CompareTag("MainCamera")){
                // reduce camera roll
                child.transform.localEulerAngles = new Vector3(child.localEulerAngles.x, 0, -roll * cameraRotateFactor ); 
            }
            else{
                child.transform.localEulerAngles = new Vector3(child.localEulerAngles.x, 0, -roll);
                // bank ship
                float chX = Mathf.MoveTowards(child.transform.localPosition.x, -bankMax * inputHorizontal * thrustRatio, rollSpeed*Time.deltaTime*thrustRatio*.5f);
                child.transform.localPosition = new Vector3(chX, 0 , 0); 
            }
        }

        // if left or right input rotate about y-axis
        float turn = turnSpeed * inputHorizontal * Time.deltaTime;
        transform.Rotate(0, turn, 0);

        // speed up or slow down based on w/s updown input within bounds of thrustMin and thrustMax
        float deltaSpeed = thrust + thrustScale * (inputVertical * Time.deltaTime);
        thrust = Mathf.Clamp(deltaSpeed, thrustMin, thrustMax);

        // move in the direction the air craft is facing 
        transform.position += transform.forward * Time.deltaTime * thrust;


        // Firing Shots
        // [Spacebar] = Fire1
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(gunProjectile, transform.Find("vehicle_playerShip_collider").transform.position, transform.rotation);
        }
    }

    void FixedUpdate()
    {

    }
}
