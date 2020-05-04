using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierOpacityController : MonoBehaviour
{

    private Material material;
    private MeshRenderer meshRenderer;
    private float maxAlpha;
    [SerializeField] float showDistance;// distance that the barier begines to show up
    private GameObject player;
    private float centerDiff = 0.0f;
    private float colliderDiff = 0.0f;
    private float rayDist1 = 0.0f;
    private float rayDist2 = 0.0f;
    private Vector3 drawlineOffset = Vector3.zero;
    private Color color;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        player = GameObject.FindGameObjectWithTag("PlayerShip");
        bool got = gameObject.TryGetComponent<Material>(out material); //gameObject.GetComponent<Material>();
        // gameObject.GetComponent<Material>();
        if (!got){
            print("Failed to get material");
        }
        color = material.color;
        maxAlpha = color.a;
        //color.a=0;
        material.color=color; //  SetColor(color);
        gameObject.SetActive(false);
        StartCoroutine(OpacityRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator OpacityRoutine(){
        yield return new WaitForSeconds(1);

        for(;;){
            // 
            centerDiff = (player.transform.position - gameObject.transform.position).magnitude;
            RaycastHit rayHit = new RaycastHit();

            if(Physics.Raycast(gameObject.transform.position,Vector3.zero-(gameObject.transform.position-player.transform.position).normalized)){
                Debug.DrawLine(gameObject.transform.position, rayHit.point, Color.red);
                rayDist1 = Vector3.Distance(rayHit.point, gameObject.transform.position);
            }

            if (Physics.Raycast( player.transform.position, Vector3.zero - (player.transform.position - gameObject.transform.position).normalized) ) 
            {
                Debug.DrawLine(player.transform.position + drawlineOffset, rayHit.point + drawlineOffset, Color.green); // offset so line can be seen
                rayDist2 = Vector3.Distance(rayHit.point, player.transform.position);
            }

            colliderDiff = centerDiff - ( centerDiff - rayDist1 ) - ( centerDiff - rayDist2 ); // colliderDiff - (collider2 radius) - (collider1 radius)

            float newOpacity = 0;

            print(colliderDiff);

            if(colliderDiff < showDistance){
                gameObject.SetActive(true);
                newOpacity = maxAlpha*(showDistance-(showDistance-colliderDiff/showDistance));
            }
            else{
                newOpacity = 0;
                gameObject.SetActive(false);
            }

            color.a=newOpacity;
            material.color=color;

            yield return new WaitForSeconds(1);
        }
    }

}
