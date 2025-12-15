using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    private Vector3 lastCheckpointPosition;
    private string currentScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;
        lastCheckpointPosition = FindObjectOfType<PlayerController>().transform.position;
    }

    public void SetCheckpoint(Vector3 position)
    {
        lastCheckpointPosition = position;
        Debug.Log("Checkpoint saved at: " + position);
    }

    public Vector3 GetLastCheckpoint()
    {
        return lastCheckpointPosition;
    }

    public void RespawnPlayer(GameObject player)
    {
        player.transform.position = lastCheckpointPosition;

      
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.health = 3;
            playerStats.isImmune = false;
        }

        DashSystem dashSystem = player.GetComponent<DashSystem>();
        if (dashSystem != null)
        {
            dashSystem.currentCharge = dashSystem.maxCharge * 0.5f;
        }

        Debug.Log("Player respawned at checkpoint");
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != currentScene)
        {
            currentScene = scene.name;
            GameObject startPoint = GameObject.FindGameObjectWithTag("StartPoint");
            if (startPoint != null)
            {
                lastCheckpointPosition = startPoint.transform.position;
            }
        }
    }
}
