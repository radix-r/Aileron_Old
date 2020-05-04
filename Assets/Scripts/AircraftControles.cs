using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// This is the PlayerAttributes Class it will be filled by JsonUtility
[System.Serializable]
public class PlayerAttributes
{
    public int hp;
    public int collisionDamage;
    public int bumpForce;
}

public class AircraftControles : Actor
{
    [SerializeField] float acceleration = 1.0f;
    public float thrust = 20.0f;
    [SerializeField] float turnSpeed = 0.0f;
    [SerializeField] float rollSpeed = 1.0f;
    float roll = 0.0f;
    [SerializeField] float rollCap = 25.0f;
    //[SerializeField] float restoreScale = 2.0f;
    [SerializeField] float thrustMax = 55;
    [SerializeField] float thrustMin = 15;
    [SerializeField] float thrustDefault = 20;
    const float thrustScale = 10;
    [SerializeField] string[] collissionTagWhitelist; // list of tags that do not interact with this object
    [SerializeField] float bankMax = 1.25f;

    [SerializeField] float cameraRotateFactor = 0.25f;

    PlayerAttributes playerAttributes;
    public TextAsset jsonFilename;
    [SerializeField] GameObject explosion;
    [SerializeField] float invincibilityCooldown;
    private bool invincible;
    private int maxHP;
    public Image hpBar;
    private bool canTurnAround;
    public float smooth = 1;
    [SerializeField] GameObject shipModel;
    int flipBit = 0;

    void Awake()
    {
        // Gets the JSONParser script. The script only takes the json text file and transforms it to a string
        // to be read by the JsonUtility
        JSONParser jsonScript = gameObject.GetComponent<JSONParser>();
        string attributesJSON = jsonScript.jsonParser(jsonFilename);
        playerAttributes = new PlayerAttributes();
        playerAttributes = JsonUtility.FromJson<PlayerAttributes>(attributesJSON);
        thrustDefault = (thrustMax + thrustMin) / 2.0f;
        invincible = false;
        maxHP = playerAttributes.hp;
        hp = playerAttributes.hp;
        hpBar.fillAmount = 1;
        canTurnAround = true;
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
                float startY = child.transform.localPosition.y;
                float startZ = child.transform.localPosition.z;
                child.transform.localEulerAngles = new Vector3(child.localEulerAngles.x, child.localEulerAngles.y, -roll);
                // bank ship
                float chX = Mathf.MoveTowards(child.transform.localPosition.x, -bankMax * inputHorizontal * thrustRatio, rollSpeed*Time.deltaTime*thrustRatio*.5f);
                child.transform.localPosition = new Vector3(chX, startY , startZ); 
            }
        }

        // if left or right input rotate about y-axis
        float turn = turnSpeed * inputHorizontal * Time.deltaTime;
        transform.Rotate(0, turn, 0);

        // speed up or slow down based on w/s updown input within bounds of thrustMin and thrustMax
        //float deltaSpeed = thrust + thrustScale * (inputVertical * Time.deltaTime);

        //thrust = Mathf.Clamp(deltaSpeed, thrustMin, thrustMax);

        float target = (inputVertical * thrustMax) + thrustDefault;
        target = Mathf.Clamp(target, thrustMin, thrustMax);

        thrust =  Mathf.MoveTowards(thrust, target, acceleration);

        // move in the direction the air craft is facing 
        transform.position += transform.forward * Time.deltaTime * thrust;
        
        if(Input.GetButton("TurnAround") && canTurnAround){
            canTurnAround = false;
            StartCoroutine(TurnAround());
        }


    }


     void Bump(float bumpFactor)
    {
        Vector3 bumpDirection = transform.forward * -1;

        Vector3 bump = bumpDirection.normalized * playerAttributes.bumpForce * thrust;
        bump.Set(bump.x, 0, bump.z);

        gameObject.GetComponent<Rigidbody>().AddForce(bump * bumpFactor);
    }

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
        Destroy(gameObject);
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
        string tag = collision.gameObject.tag;
        // if tag in damage list deal damage
        if(Array.Exists(damageTags,s => s.Equals(tag))){
            if(!invincible){
                // special case for ground
                if(tag.Equals("Ground")||tag.Contains("Enemy")){
                    Bump(1.0f);
                    hp-=playerAttributes.collisionDamage;
                }else{
                    int d = damageTable.GetDamage(tag);
                    hp -= d;
                    
                }
                hpBar.fillAmount = (float)hp/(float)maxHP;
                if(hp <= 0){
                    Die();
                }
                invincible=true;
                StartCoroutine(InvincibilityCooldown());
            }
        }  
    }

    /*
    rotate entire game object 180 about the y axis. 
    rotate the ship model 180 on x, 180 on z, -180 on y
    */
    IEnumerator TurnAround(){
        
        print("turn around");
        // what the new angles should be
        //StartCoroutine(ModelFlip());
        Vector3 targetAngle = transform.eulerAngles + 179f * Vector3.up;
        //Vector3 modelTargetAngle = shipModel.transform.localEulerAngles - 179f * Vector3.up;
        targetAngle.y = targetAngle.y % 360;
        // if (modelTargetAngle.y < 0){
        //      modelTargetAngle.y  = modelTargetAngle.y + 360;
        // }
        // modelTargetAngle.y  = modelTargetAngle.y % 360;
        //print("from: "+shipModel.transform.localEulerAngles.y+" to: "+ modelTargetAngle.y);
        while(Math.Abs(targetAngle.y - transform.eulerAngles.y)>5.0f){
            //print("target: "+targetAngle.y+" current: "+ transform.eulerAngles.y);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, targetAngle, smooth * Time.deltaTime);//smooth * Time.deltaTime
            // rotate the ship model in opposite direction
            //shipModel.transform.eulerAngles = Vector3.Lerp(shipModel.transform.localEulerAngles, modelTargetAngle, smooth * Time.deltaTime);
            thrust = thrustMin;
            yield return null;// new WaitForSeconds(.1f); 
        }
        transform.eulerAngles = targetAngle;

        yield return null;
        canTurnAround = true;
    }
    /*flip the ship model. All for show*/
    IEnumerator ModelFlip(){
        /*
        yield return null;
        Vector3 targetAngle = shipModel.transform.localEulerAngles + 180f * Vector3.forward + 180 * Vector3.right; 
        targetAngle.x = targetAngle.x % 360;
        targetAngle.z = targetAngle.z % 360;

        while(Math.Abs(targetAngle.x - shipModel.transform.localEulerAngles.x)>1.0f){
            shipModel.transform.localEulerAngles = Vector3.Lerp(shipModel.transform.localEulerAngles,targetAngle,.8f*smooth*Time.deltaTime);
            yield return null;
        }
        shipModel.transform.localEulerAngles = targetAngle;
        */
        Vector3 startAngle = shipModel.transform.localEulerAngles;
        flipBit = (flipBit + 1) % 2;
        //Vector3 modelTargetAngle = shipModel.transform.localEulerAngles - 179f * Vector3.up;
        
        // if (modelTargetAngle.y < 0){
        //      modelTargetAngle.y  = modelTargetAngle.y + 360;
        // }
        // modelTargetAngle.y  = modelTargetAngle.y % 360;
        // print("from: "+shipModel.transform.localEulerAngles.y+" to: "+ modelTargetAngle.y);
        // while(Math.Abs(modelTargetAngle.y - shipModel.transform.eulerAngles.y)>5.0f){
        //     // rotate the ship model in opposite direction
        //     shipModel.transform.eulerAngles = Vector3.Lerp(shipModel.transform.localEulerAngles, modelTargetAngle,  Time.deltaTime);
        //     yield return null;// new WaitForSeconds(.1f); 
        // }
        print("y = "+180*flipBit);
        shipModel.transform.eulerAngles = new Vector3(startAngle.x,(180*flipBit),startAngle.z);
        yield return null;
    }
}
