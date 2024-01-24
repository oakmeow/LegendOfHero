using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startHealth = 100;
    [SerializeField] float timeLastHit = 2f;
    private float timer = 0f;
    private CharacterController characterController;
    private Animator anim;
    [SerializeField] private int currentHealth;
    private AudioSource audio;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        audio = GetComponent<AudioSource>();
        currentHealth = startHealth;
    }

    void Update()
    {
        timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!GameManager.instance.GameOver && timer >= timeLastHit)
        {
            if (other.tag == "weapon")
            {
                takeHit();
                timer = 0;
            }
        }
    }

    private void takeHit()
    {
        if (currentHealth > 0)
        {
            anim.Play("Hurt");
            currentHealth -= 10;
            audio.PlayOneShot(audio.clip);
            GameManager.instance.PlayerHit(currentHealth);
        }
        if (currentHealth <= 0)
        {
            KillPlayer();
        }
    }

    private void KillPlayer()
    {
        GameManager.instance.PlayerHit(currentHealth);
        anim.SetTrigger("HeroDie");
        characterController.enabled = false;
    }
}
