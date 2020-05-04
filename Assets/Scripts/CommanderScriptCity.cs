using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/* 
    This script is the brains of the game. It commands when enemies spawn and where, When dialog plays, and when game UI pops up 

*/
public class CommanderScriptCity : MonoBehaviour
{
    private const int NORTH = 0;
    private const int SOUTH = 1; 
    private const int EAST = 2;
    private const int CENTER = 3;
    public int amountToSpawn = 20;
    public static CommanderScriptCity instance;
    public RectTransform mPanelGameOver;
    public Text mTextGameOver;
    private GameObject[] allies;
    private GameObject player;
    private AudioSource playerAudio;
    private AudioSource playerMusic;
    private EnemySpawner spawner;
    [SerializeField] GameObject [] objectives;
    [SerializeField] AudioClip aiStart;
    [SerializeField] AudioClip aiNorth;
    [SerializeField] AudioClip aiSouth;
    [SerializeField] AudioClip aiEast;
    [SerializeField] AudioClip aiLargeMass;
    [SerializeField] AudioClip aiMap;
    [SerializeField] AudioClip aiTutorial;
    [SerializeField] AudioClip swarm;
    [SerializeField] AudioClip fin;
    [SerializeField] AudioClip bossMusic;
    [SerializeField] AudioClip gameMusic;
    

    
    IEnumerator CheckLossState(){
        for(;;){
            allies = GameObject.FindGameObjectsWithTag("Ally");
            // print(allies.Length+" allies remaining");
            //print(player);
            if(player == null || allies.Length == 0){
                // lose
                // print("Lose");
                mPanelGameOver.gameObject.SetActive(true);
                mTextGameOver.text = "YOU LOSE";
            }

            yield return new WaitForSeconds(3);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("AvatarShip");
        playerAudio = GameObject.Find("Dialog").GetComponent<AudioSource>();
        playerMusic = GameObject.Find("Music").GetComponent<AudioSource>();
        allies = GameObject.FindGameObjectsWithTag("Ally");
        GameObject spawnerObj = GameObject.Find("Spawner");
        spawner = spawnerObj.GetComponent<EnemySpawner>();
        StartCoroutine(CheckLossState());
        StartCoroutine(RunGame());
    }


    IEnumerator RunGame(){
        playerMusic.Play();
        yield return new WaitForSeconds(2);
        int spawnsQueued = 0;
        // play intro ai dialog 
        playerAudio.PlayOneShot(aiStart,1);
        yield return new WaitForSeconds(aiStart.length + 1);

        playerAudio.PlayOneShot(aiMap,1);
        yield return new WaitForSeconds(aiMap.length + 1);

        playerAudio.PlayOneShot(aiTutorial,1);
        yield return new WaitForSeconds(aiTutorial.length + 1);

        spawner.SpawnNFlyingAtIndex(amountToSpawn,NORTH);
        spawnsQueued += amountToSpawn;
        spawner.SpawnNGroundAtIndex(amountToSpawn,NORTH);
        spawnsQueued += amountToSpawn;
        // Attack at north dialog
        playerAudio.PlayOneShot(aiNorth,1);
        yield return new WaitForSeconds(aiNorth.length + 5);
        // spawn a wave at the northern spawn point
        
        // wait for them to be dispatched
        while(spawner.totalEnemiesSpawned<spawnsQueued){
            yield return new WaitForSeconds(10);
        }
        // South Attack dialog
        playerAudio.PlayOneShot(aiSouth,1);
        yield return new WaitForSeconds(aiSouth.length + 5);
        // move objective
        objectives[NORTH].transform.SetPositionAndRotation(objectives[SOUTH].transform.position,objectives[SOUTH].transform.rotation) ;
        // spawn wave at South
        //spawner.maxEnemies = (int)(spawner.maxEnemies*1.5);
        //spawner.spawnCooldown = spawner.spawnCooldown*.8f;
        spawner.SpawnNFlyingAtIndex(amountToSpawn,SOUTH);
        spawnsQueued += amountToSpawn;
        spawner.SpawnNGroundAtIndex(amountToSpawn,SOUTH);
        spawnsQueued += amountToSpawn;
        // total spawned is 90
        while(spawner.totalEnemiesSpawned<spawnsQueued){
            yield return new WaitForSeconds(10);
        }
        
        playerAudio.PlayOneShot(aiLargeMass);
        yield return new WaitForSeconds (aiLargeMass.length);
        // spawn boss
        spawner.SpawnBossAtIndex(SOUTH);

        
        int enemyCount = 0;

        enemyCount += GameObject.FindGameObjectsWithTag("FlyingEnemy").Length;
        enemyCount += GameObject.FindGameObjectsWithTag("GroundEnemy").Length;

        while(enemyCount>1){
            yield return new WaitForSeconds(5);
            enemyCount = 0;
            enemyCount += GameObject.FindGameObjectsWithTag("FlyingEnemy").Length;
            enemyCount += GameObject.FindGameObjectsWithTag("GroundEnemy").Length;
            print(enemyCount+" enemies remaining");
        }

        
        
        //spawn wave at east
        //spawner.maxEnemies = (int)(spawner.maxEnemies*1.5);
        //spawner.spawnCooldown = spawner.spawnCooldown*.8f;
        playerAudio.PlayOneShot(aiEast);
        spawner.maxEnemies = (int)(spawner.maxEnemies*1.5);
        spawner.spawnCooldown = spawner.spawnCooldown*.8f;
        spawner.SpawnNFlyingAtIndex(amountToSpawn,EAST);
        spawnsQueued += amountToSpawn;
        spawner.SpawnNGroundAtIndex(amountToSpawn,EAST);
        spawnsQueued += amountToSpawn;
        

        while(spawner.totalEnemiesSpawned<spawnsQueued){
            yield return new WaitForSeconds(10);
        }

        while(enemyCount>10){
            yield return new WaitForSeconds(5);
            enemyCount = 0;
            enemyCount += GameObject.FindGameObjectsWithTag("FlyingEnemy").Length;
            enemyCount += GameObject.FindGameObjectsWithTag("GroundEnemy").Length;
            print(enemyCount+" enemies remaining");
        }

        // allies fall back to center
        playerAudio.PlayOneShot(swarm);
        // play boss music
        playerMusic.Stop();
        playerMusic.clip = bossMusic;// PlayOneShot(bossMusic,.6f);
        playerMusic.Play();
        playerMusic.volume = .1f;
        allies = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject ally in allies)
        {
            HoverTankBehavior behavior = ally.GetComponent<HoverTankBehavior>();
            behavior.positionIndex = behavior.positionIndex+1;
        }
         


        spawner.maxEnemies = (int)(spawner.maxEnemies*1.5);
        spawner.spawnCooldown = spawner.spawnCooldown*.8f;

        spawner.SpawnNFlyingAtIndex(amountToSpawn,EAST);
        spawnsQueued += amountToSpawn;
        spawner.SpawnNGroundAtIndex(amountToSpawn,EAST);
        spawnsQueued += amountToSpawn;

        spawner.SpawnNFlyingAtIndex(amountToSpawn,NORTH);
        spawnsQueued += amountToSpawn;
        spawner.SpawnNGroundAtIndex(amountToSpawn,NORTH);
        spawnsQueued += amountToSpawn;

        spawner.SpawnNFlyingAtIndex(amountToSpawn,SOUTH);
        spawnsQueued += amountToSpawn;
        spawner.SpawnNGroundAtIndex(amountToSpawn,SOUTH);
        spawnsQueued += amountToSpawn;
        
        while(spawner.totalEnemiesSpawned<spawnsQueued){
            yield return new WaitForSeconds(10);
        }
        spawner.SpawnBossAtIndex(EAST);
        spawner.SpawnBossAtIndex(NORTH);
        spawner.SpawnBossAtIndex(SOUTH);
       
        enemyCount=0;
        enemyCount += GameObject.FindGameObjectsWithTag("FlyingEnemy").Length;
        enemyCount += GameObject.FindGameObjectsWithTag("GroundEnemy").Length;
        while(enemyCount>0){
            yield return new WaitForSeconds(5);
            enemyCount = 0;
            enemyCount += GameObject.FindGameObjectsWithTag("FlyingEnemy").Length;
            enemyCount += GameObject.FindGameObjectsWithTag("GroundEnemy").Length;
            print(enemyCount+" enemies remaining");
        }

        // print("Win");
        mPanelGameOver.gameObject.SetActive(true);
        mTextGameOver.text = "YOU WIN";
        playerAudio.PlayOneShot(fin);
    }
}
