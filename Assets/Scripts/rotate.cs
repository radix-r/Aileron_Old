using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour
{
    public GameObject ship;
    public Vector3 xAngle, yAngle;

    // Start is called before the first frame update
    void Start()
    {
        yAngle = new Vector3(0.0f, -0.3f, 0.0f);

        if (ship == null)
            ship = GameObject.FindGameObjectWithTag("PlayerShip");
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.RotateAround(Vector3.zero, yAngle, 30 * Time.deltaTime);
    }
}
