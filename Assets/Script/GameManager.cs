using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField] TextMeshProUGUI victoryText;
    [SerializeField] TextMeshProUGUI defeatText;
    private int currentLevel;
    private int finalLevel = 3;
    private float generateSpawnTime = 1f;
    private float currentSpawnTime = 0f;
    private GameObject newEnemy;

    private List<EnemyHealth> enemies = new List<EnemyHealth>();
    private List<EnemyHealth> killEnemies = new List<EnemyHealth>();

    [SerializeField] GameObject arrow;

    // Health Power Up
    [SerializeField] GameObject healthPowerUp;
    [SerializeField] GameObject[] healthSpawnPoint;
    [SerializeField] int maxPowerUp = 4;

    private float powerUpSpawnTime = 1f;
    private float currentPowerUpSpawnTime = 0f;
    private int powerUp = 0;
    private GameObject newPowerUp;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(spawn());
        StartCoroutine(powerUpSpawn());
        currentLevel = 0;
    }

    void Update()
    {
        currentSpawnTime += Time.deltaTime;
        currentPowerUpSpawnTime += Time.deltaTime;
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
            StartCoroutine(defeat());
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

    public void RegisterPowerUp()
    {
        powerUp++;
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

        if (killEnemies.Count == currentLevel && currentLevel != finalLevel)
        {
            enemies.Clear();
            killEnemies.Clear();
            yield return new WaitForSeconds(3f);
            currentLevel++;
            leveltext.text = "Level = " + currentLevel;
        }
        if (killEnemies.Count == finalLevel)
        {
            StartCoroutine(victory());
        }
        yield return null;
        StartCoroutine(spawn());
    }

    IEnumerator victory()
    {
        //victoryText.enabled = true;
        victoryText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator defeat()
    {
        //defeatText.enabled = true;
        defeatText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator powerUpSpawn()
    {
        if (currentPowerUpSpawnTime > powerUpSpawnTime)
        {
            currentPowerUpSpawnTime = 0;
            if (powerUp < maxPowerUp)
            {
                int randomNumber = Random.Range(0, healthSpawnPoint.Length);
                GameObject spawnLocation = healthSpawnPoint[randomNumber];
                newPowerUp = Instantiate(healthPowerUp) as GameObject;
                newPowerUp.transform.position = spawnLocation.transform.position;
            }
        }
        yield return null;
        StartCoroutine(powerUpSpawn());
    }
}
