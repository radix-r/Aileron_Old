using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AircraftControles : MonoBehaviour
{
    [SerializeField] float thrust = 0.0f;
    [SerializeField] float turnSpeed = 0.0f;

    private GameObject craft;

    // Start is called before the first frame update
    void Start()
    {
        //craft = 
    }

    // Update is called once per frame
    void Update()
    {
        float turn = turnSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
        // if left or right input rotate about y-axis
        transform.Rotate(0, turn ,0);

        // move in the direction the air craft is facing 
        transform.position += transform.forward * Time.deltaTime * thrust;
    }

    void FixedUpdate(){

    }
}
