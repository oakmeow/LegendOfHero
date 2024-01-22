using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private bool gameOver = false;
    [SerializeField] GameObject player;

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

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
