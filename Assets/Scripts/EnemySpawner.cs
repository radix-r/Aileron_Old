using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class EnemySpawner : MonoBehaviour
{

    public GameObject groundEnemy;
    public GameObject flyingEnemy;
    public GameObject boss;
    private GameObject[] groundEnemies;
    private GameObject[] flyingEnemies;
    public SpriteRenderer spriteRenderer;
    public Sprite enemyMiniMapSprite;
    public GameObject miniMapIcon;

    public int maxEnemies;
    public float spawnRadius;
    public float spawnCooldown; // Higher is slower
    public float spawnHeight;
    
    private bool canSpawn; // spawn limiter tied to all enemies for performance reasons

    public int totalEnemiesSpawned;

    [SerializeField] GameObject[] spawnLocations;

    // Start is called before the first frame update
    void Start()
    {
        enemyMiniMapSprite = Resources.Load<Sprite>("miniMap/Circle");
        totalEnemiesSpawned = 0;
        canSpawn = true;
        // canSpawnG = true;
        Random.InitState((int)Time.time);
        //StartCoroutine(SpawnFlying(maxEnemies,1));
        //StartCoroutine(SpawnGround(maxEnemies,1));
    }

    Quaternion GenerateRotation(){
        Quaternion spawnRotation = new Quaternion();
        spawnRotation.eulerAngles.Set(0, Random.Range(0, 10), 0);
        return spawnRotation;
    }

    Vector3 GeneratePosition(){
        int spawnIndex = Random.Range(0,spawnLocations.Length);
        Vector3 spawnPosition = Random.insideUnitSphere * spawnRadius;

        spawnPosition.y = spawnHeight; // Needs to be changed with map
        return spawnPosition;
    }

    IEnumerator SpawnEnemy(GameObject enemy, int at){
        Vector3 spawnPosition;
        Quaternion spawnRotation;
        yield return new WaitForSeconds(.1f);
        spawnPosition = GeneratePosition();
        yield return new WaitForSeconds(.1f);
        spawnRotation = GenerateRotation();
        yield return new WaitForSeconds(.1f);

        
        //nextGSpawn = Time.time + spawnRate;
        Instantiate(enemy,spawnPosition+spawnLocations[at].transform.position ,spawnRotation);

        //Instantiate(miniMapIcon);
        //miniMapIcon = new GameObject("miniMapIcon");
        //miniMapIcon.transform.SetParent(enemy.transform);
        //miniMapIcon.transform.Rotate(90, 0, 0);
        //miniMapIcon.transform.localScale = new Vector3(100, 100, 5);
        //miniMapIcon.layer = 8;

        //if (miniMapIcon.GetComponent<SpriteRenderer>() == null) {
        //    miniMapIcon.AddComponent<SpriteRenderer>();

        //    spriteRenderer = miniMapIcon.GetComponent<SpriteRenderer>();
        //    spriteRenderer.color = Color.red;
        //    spriteRenderer.renderingLayerMask = 8; // miniMapIcons rendering layer

        //    if (spriteRenderer.sprite == null) {
        //        spriteRenderer.sprite = enemyMiniMapSprite;
        //    }
        //}
        

        // atomic increment
        System.Threading.Interlocked.Increment(ref totalEnemiesSpawned);
    }

    IEnumerator SpawnCooldown(){
        yield return new WaitForSeconds(spawnCooldown);
        canSpawn = true;
    }

    IEnumerator SpawnFlying(int count, int at){
        
        for(int i = count; i > 0;i--){
            flyingEnemies = GameObject.FindGameObjectsWithTag("FlyingEnemy");
            while(!(canSpawn && (flyingEnemies.Length < maxEnemies)) )
            {   
                yield return new WaitForSeconds(spawnCooldown*2+Random.Range(.1f,1f));
                flyingEnemies = GameObject.FindGameObjectsWithTag("FlyingEnemy");
            }
            StartCoroutine(SpawnEnemy(flyingEnemy,at));
            // SpawnEnemy(flyingEnemy);
            canSpawn = false;
            StartCoroutine(SpawnCooldown());
            
            
            //canSpawnF = true;
        }
    }

    IEnumerator SpawnGround(int count, int at){
        for(int i = count;i > 0;i--){
            groundEnemies = GameObject.FindGameObjectsWithTag("GroundEnemy");
            
            while(!(canSpawn && groundEnemies.Length < maxEnemies) )
            {
                //print("groundEnemies length is " + groundEnemies.Length);
                yield return new WaitForSeconds(spawnCooldown*2+Random.Range(.1f,1f));
                groundEnemies = GameObject.FindGameObjectsWithTag("GroundEnemy");
            }
            //nextGSpawn = Time.time + spawnRate;
            StartCoroutine(SpawnEnemy(groundEnemy,at));

            canSpawn = false;
            StartCoroutine(SpawnCooldown());
            

            //yield return new WaitForSeconds(spawnCooldown*2+Random.Range(.1f,1f));
            //canSpawnG = true;
        }
        
    }

    public void SpawnNFlyingAtIndex(int count, int index){
        //for(int i = 0; i < count; i+=2){
            StartCoroutine(SpawnFlying(count,index));
        //}
    }
    public void SpawnNGroundAtIndex(int count, int index){
        //for(int i = 0; i < count; i+=2){
            
            StartCoroutine(SpawnGround(count,index));
        //}
    }

    public void SpawnBossAtIndex(int at){
        print("Spawning boss");
        StartCoroutine(SpawnEnemy(boss,at));
    }

}
