using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyPickup : MonoBehaviour
{
    public float chargeAmount = 25f;
    public float respawnTime = 5f;
    public GameObject pickupEffect;
    public AudioClip pickupSound;  
    public float soundVolume = 1f;
    private Collider2D col;
    private SpriteRenderer sr;
    private bool isActive = true;

    void Start()
    {
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.CompareTag("Player"))
        {
            DashSystem dash = other.GetComponent<DashSystem>();
            if (dash != null)
            {
                dash.AddCharge(chargeAmount);

               
                PlayPickupSound();

                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                StartCoroutine(RespawnPickup());
            }
        }
    }
    void PlayPickupSound()
    {
        if (pickupSound != null)
        {
          
            AudioSource playerAudio = FindObjectOfType<PlayerController>().GetComponent<AudioSource>();
            if (playerAudio != null)
            {
                playerAudio.PlayOneShot(pickupSound, soundVolume);
            }
           
            else
            {
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, soundVolume);
            }
        }
    }
    System.Collections.IEnumerator RespawnPickup()
    {
        isActive = false;
        col.enabled = false;
        sr.enabled = false;

        yield return new WaitForSeconds(respawnTime);

        isActive = true;
        col.enabled = true;
        sr.enabled = true;
    }
}