using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int startHealth = 100;
    [SerializeField] float timeLastHit = 2f;
    private float timer = 0f;
    private CharacterController characterController;
    private Animator anim;
    [SerializeField] private int currentHealth;
    private AudioSource sound;
    [SerializeField] private Slider healthSlider;
    private ParticleSystem blood;
    [SerializeField] AudioClip powerUpSound;

    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        sound = GetComponent<AudioSource>();
        currentHealth = startHealth;
        blood = GetComponentInChildren<ParticleSystem>();
    }

    private void Awake()
    {
        Assert.IsNotNull(healthSlider);
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
            blood.Play();
            currentHealth -= 10;
            healthSlider.value = currentHealth;
            sound.PlayOneShot(sound.clip);
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
        blood.Play();
        anim.SetTrigger("HeroDie");
        characterController.enabled = false;
    }

    public void PowerHealth()
    {
        if (currentHealth <= 70)
        {
            CurrentHealth += 30;
            sound.PlayOneShot(powerUpSound);
        }
        else if (currentHealth < startHealth)
        {
            currentHealth = startHealth;
        }
        healthSlider.value = currentHealth;
    }

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            if (value < 0)
                currentHealth = 0;
            else
                currentHealth = value;
        }
    }
}
