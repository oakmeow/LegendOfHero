using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] private bool gameOver = false;
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] spawnPoint;
    [SerializeField] GameObject tanker;
    [SerializeField] GameObject soldier;
    [SerializeField] GameObject ranger;
    [SerializeField] TextMeshProUGUI leveltext;
    private int currentLevel;
    private float generateSpawnTime = 1f;
    private float currentSpawnTime = 0f;
    private GameObject newEnemy;

    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private List<EnemyHealth> killEnemies = new List<EnemyHealth>();

    [SerializeField] GameObject arrow;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(spawn());
        currentLevel = 0;
    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;
    }

    public GameObject Player
    {
        get
        {
            return player;
        }
    }

    public bool GameOver
    {
        get
        {
            return gameOver;
        }
    }

    public GameObject Arrow
    {
        get
        {
            return arrow;
        }
    }

    public void PlayerHit(int currentHP)
    {
        if (currentHP > 0)
        {
            gameOver = false;
        }
        else
        {
            gameOver = true;
        }
    }

    public void RegisterEnemy(EnemyHealth enemy)
    {
        enemies.Add(enemy);
    }

    public void KillEnemy(EnemyHealth enemy)
    {
        killEnemies.Add(enemy);
    }

    IEnumerator spawn()
    {
        if (currentSpawnTime > generateSpawnTime)
        {
            currentSpawnTime = 0;
            if (enemies.Count < currentLevel)
            {
                int randomNumber = Random.Range(0, spawnPoint.Length);
                GameObject spawnLocation = spawnPoint[randomNumber];
                int randomEnemy = Random.Range(0, 3);

                if (randomEnemy == 0)
                {
                    newEnemy = Instantiate(soldier) as GameObject;
                }
                if (randomEnemy == 1)
                {
                    newEnemy = Instantiate(tanker) as GameObject;
                }
                if (randomEnemy == 2)
                {
                    newEnemy = Instantiate(ranger) as GameObject;
                }
                newEnemy.transform.position = spawnLocation.transform.position;
            }
        }

        if (killEnemies.Count == currentLevel)
        {
            enemies.Clear();
            killEnemies.Clear();
            yield return new WaitForSeconds(3f);
            currentLevel++;
            leveltext.text = "Level = " + currentLevel;
        }
        yield return null;
        StartCoroutine(spawn());
    }
}
