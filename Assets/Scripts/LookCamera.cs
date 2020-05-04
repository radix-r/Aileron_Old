using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    public GameObject target;
    public float rotateSpeed = 5;
    private float initXAngle;
    private float yAngle=0;
    private float xAngle=0;
    private Vector3 yAxis;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
        initXAngle=transform.localEulerAngles.x;
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate() {
        /* Horizontal Looking */
        float horizontal = Input.GetAxis("LookHorizontal");// * rotateSpeed;
        //print(horizontal);
        if(horizontal*yAngle < 0){
            // want to look the other way. rotate to 0 first
            //print("reverse");
            yAngle = Mathf.MoveTowardsAngle(yAngle,0,Time.deltaTime*rotateSpeed);
        }
        else{
            yAngle = Mathf.MoveTowardsAngle(yAngle,horizontal*179,Time.deltaTime*rotateSpeed);
        }


        /* Vertical Looking */
        float vertical = Input.GetAxis("LookVertical");
        //print(vertical);
        xAngle = Mathf.MoveTowardsAngle(xAngle,vertical*40,Time.deltaTime*rotateSpeed);


        
        transform.localEulerAngles=new Vector3(initXAngle+xAngle,yAngle,0);

    }
}
