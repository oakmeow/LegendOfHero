using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float range = 4f;
    [SerializeField] private float timeBetweenAttack = 1f;
    private Animator anim;
    private GameObject player;
    [SerializeField] private bool playerInRange;
    private BoxCollider[] weaponCollider;

    void Start()
    {
        player = GameManager.instance.Player;
        anim = GetComponent<Animator>();
        weaponCollider = GetComponentsInChildren<BoxCollider>();
        StartCoroutine(attack());
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < range)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }

    IEnumerator attack()
    {
        if(playerInRange && !GameManager.instance.GameOver)
        {
            anim.Play("Attack");
            yield return new WaitForSeconds(timeBetweenAttack);
        }
        yield return null;
        StartCoroutine(attack());
    }

    public void EnemyBeginAttack()
    {
        foreach (var weapon in weaponCollider)
        {
            weapon.enabled = true;
        }
    }

    public void EnemyEndAttack()
    {
        foreach (var weapon in weaponCollider)
        {
            weapon.enabled = false;
        }
    }
}
