using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int startHealth = 30;
    [SerializeField] float timeLastHit = 2f;
    private float timer = 0f;
    private NavMeshAgent nav;
    private Animator anim;
    [SerializeField] private int currentHealth;
    private AudioSource sound;
    private float disappearSpeed = 2f;
    private bool dissppearEnemy = false;
    [SerializeField] private bool isAlive;
    private Rigidbody rigid;
    private CapsuleCollider capsuleCollider;
    private ParticleSystem blood;

    void Start()
    {
        currentHealth = startHealth;
        rigid = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();
        sound = GetComponent<AudioSource>();
        nav = GetComponent<NavMeshAgent>();
        blood = GetComponentInChildren<ParticleSystem>();
        GameManager.instance.RegisterEnemy(this);
        isAlive = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (dissppearEnemy)
        {
            transform.Translate(-Vector3.up * disappearSpeed * Time.deltaTime);
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (timer >= timeLastHit && !GameManager.instance.GameOver)
        {
            if (other.tag == "playerweapon")
            {
                TakeHit();
                timer = 0;
            }
        }
    }

    private void TakeHit()
    {
        if (currentHealth > 0)
        {
            sound.PlayOneShot(sound.clip);
            anim.Play("Hurt");
            blood.Play();
            currentHealth -= 10;
        }
        if (currentHealth <= 0)
        {
            isAlive = false;
            KillEnemy();
        }
    }

    private void KillEnemy()
    {
        capsuleCollider.enabled = false;
        nav.enabled = false;
        blood.Play();
        anim.SetTrigger("EnemyDie");
        rigid.isKinematic = true;
        GameManager.instance.KillEnemy(this);
        StartCoroutine(removeEnemy());
    }

    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds(4f);
        dissppearEnemy = true;
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
