using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent nav;
    private Animator anim;
    private EnemyHealth enemyHealth;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        enemyHealth = GetComponent<EnemyHealth>();
        target = GameManager.instance.Player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.instance.GameOver && enemyHealth.IsAlive)
        {
            nav.SetDestination(target.position);
        }
        else if (!GameManager.instance.GameOver || GameManager.instance.GameOver && !enemyHealth.IsAlive)
        {
            nav.enabled = false;
        }
        else
        {
            nav.enabled = false;
            anim.Play("Idle");
        }
    }
}
