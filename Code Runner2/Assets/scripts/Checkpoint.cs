using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject checkpointEffect;
    private bool isActivated = false;
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            ActivateCheckpoint();
        }
    }

    void ActivateCheckpoint()
    {
        isActivated = true;

       
        sr.color = Color.green;

       
        CheckpointManager.Instance.SetCheckpoint(transform.position);

       
        if (checkpointEffect != null)
        {
            Instantiate(checkpointEffect, transform.position, Quaternion.identity);
        }

        Debug.Log("Checkpoint activated at: " + transform.position);
    }
}